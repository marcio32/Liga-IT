using System.ComponentModel.DataAnnotations;

namespace Liga_IT.WEB.Models;

public class MatchFormViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La ronda es requerida")]
    [Range(1, 50, ErrorMessage = "La ronda debe estar entre 1 y 50")]
    public int Round { get; set; }
    
    [Required(ErrorMessage = "El club local es requerido")]
    public int HomeClubId { get; set; }
    
    [Required(ErrorMessage = "El club visitante es requerido")]
    public int AwayClubId { get; set; }
    
    [Range(0, 20, ErrorMessage = "El gol local debe estar entre 0 y 20")]
    public int? HomeScore { get; set; }
    
    [Range(0, 20, ErrorMessage = "El gol visitante debe estar entre 0 y 20")]
    public int? AwayScore { get; set; }
    
    [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
    public string Notes { get; set; } = string.Empty;
    
    public int? RefereeId { get; set; }
    
    [Required(ErrorMessage = "El estado es requerido")]
    public int Status { get; set; }
    
    [Required(ErrorMessage = "La fecha del partido es requerida")]
    public DateTime MatchDate { get; set; }
    
    public DateTime CreateAt { get; set; }
    public string? HomeClubName { get; set; }
    public string? AwayClubName { get; set; }
    public string? RefereeName { get; set; }
}
