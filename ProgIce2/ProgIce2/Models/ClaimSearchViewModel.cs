using System.ComponentModel.DataAnnotations;

namespace ProgIce2.Models;

public class ClaimSearchViewModel
{
    [Display(Name = "Student number")]
    public string? StudentNumber { get; set; }

    public IReadOnlyList<Claim> Claims { get; set; } = [];
}
