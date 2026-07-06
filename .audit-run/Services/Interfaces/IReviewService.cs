using PerfumeStore.MVCC.ViewModels;



namespace PerfumeStore.MVCC.Services.Interfaces;



public interface IReviewService

{

    Task<ReviewListViewModel> GetUserReviewsAsync(int userId);



    Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(int productId, int? currentUserId = null);

    Task<IReadOnlyList<ReviewItemViewModel>> GetRecentReviewsAsync(int count);



    Task<bool> HasUserReviewAsync(int userId, int productId);



    Task<bool> CreateReviewAsync(int userId, int productId, int rating, string? comment);



    Task<ReviewEditViewModel?> GetReviewForEditAsync(int userId, int reviewId);



    Task<bool> UpdateReviewAsync(int userId, int reviewId, int rating, string? comment);



    Task<bool> DeleteReviewAsync(int userId, int reviewId);

}


