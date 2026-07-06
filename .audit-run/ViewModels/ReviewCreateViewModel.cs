using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class ReviewCreateViewModel
{
    [Required]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Укажите оценку от 1 до 5.")]
    [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5.")]
    public int Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Комментарий не должен превышать 1000 символов.")]
    public string? Comment { get; set; }
}
