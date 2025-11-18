using System.ComponentModel.DataAnnotations;

namespace Liga_IT.WEB.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
