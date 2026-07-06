using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class UserManageViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Введите ФИО.")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите email.")]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Выберите роль.")]
    public string Role { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool IsEdit => Id.HasValue;
}
