using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class UserService : IUserService
{
    private readonly PerfumeStoreDbContext _context;

    public UserService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalizedEmail = email.Trim();

        return await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == normalizedEmail);
    }

    public async Task<int> RegisterClientAsync(
        string fullName,
        string email,
        string password,
        string? phone)
    {
        var user = new User
        {
            FullName = fullName.Trim(),
            Email = email.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            PasswordHash = PasswordHashHelper.ComputeSha256(password),
            Role = UserRoles.Client
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<ClientDashboardViewModel?> GetDashboardAsync(int userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.FullName,
                OrdersCount = u.Orders.Count,
                ActiveOrdersCount = u.Orders.Count(order =>
                    order.Status != OrderStatuses.Delivered &&
                    order.Status != OrderStatuses.Canceled),
                DeliveredOrdersCount = u.Orders.Count(order => order.Status == OrderStatuses.Delivered),
                CanceledOrdersCount = u.Orders.Count(order => order.Status == OrderStatuses.Canceled),
                FavoritesCount = u.Favorites.Count(favorite => favorite.Product.IsActive),
                CartItemsCount = u.Cart != null
                    ? u.Cart.CartItems
                        .Where(item =>
                            item.Product.IsActive &&
                            item.Product.StockQuantity > 0 &&
                            item.Quantity > 0 &&
                            item.Quantity <= item.Product.StockQuantity)
                        .Sum(item => item.Quantity)
                    : 0,
                ReviewsCount = u.Reviews.Count,
                LastOrder = u.Orders
                    .OrderByDescending(order => order.OrderDate)
                    .Select(order => new ClientOrderSummaryViewModel
                    {
                        OrderId = order.Id,
                        Status = order.Status,
                        TotalAmount = order.TotalAmount,
                        OrderDate = order.OrderDate
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var recentReviews = await _context.Reviews
            .AsNoTracking()
            .Where(review => review.UserId == userId)
            .OrderByDescending(review => review.ReviewDate)
            .Take(3)
            .Select(review => new ReviewItemViewModel
            {
                ReviewId = review.Id,
                ProductId = review.ProductId,
                ProductName = review.Product.Name,
                UserFullName = review.User.FullName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
                IsOwnReview = true
            })
            .ToListAsync();

        var activities = new List<ClientActivityItemViewModel>();

        if (user.LastOrder is not null)
        {
            activities.Add(new ClientActivityItemViewModel
            {
                Title = $"Заказ №{user.LastOrder.OrderId}",
                Description = $"Статус: {OrderStatusHelper.ToDisplayName(user.LastOrder.Status)} · {user.LastOrder.TotalAmount:N2} BYN",
                OccurredAt = user.LastOrder.OrderDate,
                Icon = "bi-bag-check"
            });
        }

        foreach (var review in recentReviews)
        {
            activities.Add(new ClientActivityItemViewModel
            {
                Title = $"Отзыв: {review.ProductName}",
                Description = $"Оценка {review.Rating}/5",
                OccurredAt = review.ReviewDate,
                Icon = "bi-chat-left-text"
            });
        }

        return new ClientDashboardViewModel
        {
            FullName = user.FullName,
            OrdersCount = user.OrdersCount,
            ActiveOrdersCount = user.ActiveOrdersCount,
            DeliveredOrdersCount = user.DeliveredOrdersCount,
            CanceledOrdersCount = user.CanceledOrdersCount,
            FavoritesCount = user.FavoritesCount,
            CartItemsCount = user.CartItemsCount,
            ReviewsCount = user.ReviewsCount,
            LastOrder = user.LastOrder,
            RecentReviews = recentReviews,
            RecentActivities = activities
                .OrderByDescending(activity => activity.OccurredAt)
                .Take(5)
                .ToList()
        };
    }

    public async Task<ClientProfileViewModel?> GetProfileAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new ClientProfileViewModel
            {
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EditProfileViewModel?> GetEditProfileAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new EditProfileViewModel
            {
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateProfileAsync(int userId, string fullName, string? phone)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return false;
        }

        user.FullName = fullName.Trim();
        user.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserListViewModel> GetAllUsersAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .OrderBy(user => user.FullName)
            .Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            })
            .ToListAsync();

        return new UserListViewModel { Users = users };
    }

    public async Task<IReadOnlyList<UserListItemViewModel>> GetRecentUsersAsync(int count)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderByDescending(user => user.CreatedAt)
            .Take(count)
            .Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<UserDetailsViewModel?> GetUserDetailsAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new UserDetailsViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                OrdersCount = user.Orders.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserManageViewModel?> GetUserForManageAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new UserManageViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateUserAsync(UserManageViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            throw new InvalidOperationException("Укажите пароль.");
        }

        if (await EmailExistsAsync(model.Email))
        {
            throw new InvalidOperationException("Пользователь с таким email уже существует.");
        }

        var user = new User
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim(),
            Role = model.Role,
            PasswordHash = PasswordHashHelper.ComputeSha256(model.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<bool> UpdateUserAsync(UserManageViewModel model)
    {
        if (model.Id is null)
        {
            return false;
        }

        var user = await _context.Users.FirstOrDefaultAsync(item => item.Id == model.Id);

        if (user is null)
        {
            return false;
        }

        var normalizedEmail = model.Email.Trim();

        var emailTaken = await _context.Users
            .AsNoTracking()
            .AnyAsync(item => item.Email == normalizedEmail && item.Id != model.Id);

        if (emailTaken)
        {
            throw new InvalidOperationException("Пользователь с таким email уже существует.");
        }

        user.FullName = model.FullName.Trim();
        user.Email = normalizedEmail;
        user.Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();
        user.Role = model.Role;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            user.PasswordHash = PasswordHashHelper.ComputeSha256(model.Password);
        }

        await _context.SaveChangesAsync();

        return true;
    }
}
