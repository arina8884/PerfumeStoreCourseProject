using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите ФИО.")]
    [StringLength(150, ErrorMessage = "ФИО не должно превышать 150 символов.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите email.")]
    [EmailAddress(ErrorMessage = "Введите корректный email.")]
    [StringLength(150, ErrorMessage = "Email не должен превышать 150 символов.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Введите пароль.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Повторите пароль.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
