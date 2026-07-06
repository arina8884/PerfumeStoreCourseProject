using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Введите ФИО.")]
    [StringLength(150, ErrorMessage = "ФИО не должно превышать 150 символов.")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов.")]
    public string? Phone { get; set; }
}
