using System.ComponentModel.DataAnnotations;

namespace Liga_IT.WEB.Models;

public class ClubFormViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La ciudad es requerida")]
    [StringLength(50, ErrorMessage = "La ciudad no puede exceder 50 caracteres")]
    public string City { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El número de socios es requerido")]
    [Range(1, 10000, ErrorMessage = "El número de socios debe estar entre 1 y 10000")]
    public int NumberOfPartners { get; set; }
    
    [Required(ErrorMessage = "El teléfono es requerido")]
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    public string Address { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre del estadio es requerido")]
    [StringLength(100, ErrorMessage = "El nombre del estadio no puede exceder 100 caracteres")]
    public string StadiumName { get; set; } = string.Empty;
    
    public DateTime CreateAt { get; set; }
}
