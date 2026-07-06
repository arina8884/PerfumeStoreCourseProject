using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class CategoryEditViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Введите название категории.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
