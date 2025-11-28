using System.ComponentModel.DataAnnotations;

namespace Liga_IT.WEB.Models;

public class RefereeViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El número de licencia es requerido")]
    [StringLength(20, ErrorMessage = "El número de licencia no puede exceder 20 caracteres")]
    public string LicenseNumber { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    [Required(ErrorMessage = "La categoría es requerida")]
    public int Category { get; set; }
    
    public DateTime CreateAt { get; set; }
}
