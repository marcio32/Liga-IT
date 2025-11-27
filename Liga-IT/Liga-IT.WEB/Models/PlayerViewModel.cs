using System.ComponentModel.DataAnnotations;

namespace Liga_IT.WEB.Models;

public class PlayerViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La edad es requerida")]
    [Range(16, 50, ErrorMessage = "La edad debe estar entre 16 y 50 años")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "El número de camiseta es requerido")]
    [Range(1, 99, ErrorMessage = "El número debe estar entre 1 y 99")]
    public int JerseyNumber { get; set; }
    
    [Required(ErrorMessage = "La posición es requerida")]
    public string Position { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La nacionalidad es requerida")]
    public string Nationality { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La altura es requerida")]
    [Range(1.50, 2.20, ErrorMessage = "La altura debe estar entre 1.50 y 2.20 metros")]
    public decimal Height { get; set; }
    
    [Required(ErrorMessage = "El peso es requerido")]
    [Range(50, 120, ErrorMessage = "El peso debe estar entre 50 y 120 kg")]
    public decimal Weight { get; set; }
    
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int MatchesPlayed { get; set; }
    
    [Required(ErrorMessage = "El club es requerido")]
    public int ClubId { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "La fecha de ingreso al club es requerida")]
    public DateTime JoinedClubDate { get; set; }
    
    public string? ClubName { get; set; }
}
