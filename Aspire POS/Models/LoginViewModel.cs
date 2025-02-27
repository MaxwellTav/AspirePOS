using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "El Usuario es obligatorio")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "La Contraseña es obligatoria")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
