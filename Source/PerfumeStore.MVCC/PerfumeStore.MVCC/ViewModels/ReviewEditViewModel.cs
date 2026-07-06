using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class ReviewEditViewModel
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Оценка")]
    [Required(ErrorMessage = "Укажите оценку от 1 до 5.")]
    [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5.")]
    public int Rating { get; set; }

    [Display(Name = "Комментарий")]
    [StringLength(1000, ErrorMessage = "Комментарий не должен превышать 1000 символов.")]
    public string? Comment { get; set; }
}
