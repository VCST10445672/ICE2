using System.ComponentModel.DataAnnotations;

namespace ProgIce2.Models;

public class Claim
{
    public int ClaimId { get; set; }

    [Required, StringLength(20), Display(Name = "Student number")]
    public string StudentNumber { get; set; } = string.Empty;

    [Required, StringLength(50), Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Surname { get; set; } = string.Empty;

    [Required, Range(typeof(decimal), "0.01", "999.99"), Display(Name = "Hours worked")]
    public decimal HoursWorked { get; set; }

    [Display(Name = "Hourly rate")]
    public decimal HourlyRate { get; set; } = 200.00m;

    [Display(Name = "Total claim amount")]
    public decimal TotalClaimAmount { get; set; }

    [Display(Name = "Date submitted")]
    public DateTime DateSubmitted { get; set; }

    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
}
