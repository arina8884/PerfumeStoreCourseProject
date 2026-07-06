using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите email.")]
    [EmailAddress(ErrorMessage = "Введите корректный email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите пароль.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
