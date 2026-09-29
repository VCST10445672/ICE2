using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgIce2.Data;
using ProgIce2.Models;

namespace ProgIce2.Controllers;

public class ClaimsController(BursaryClaimsContext context) : Controller
{
    private const decimal FixedHourlyRate = 200.00m;

    [HttpGet]
    public IActionResult Create() => View(new Claim { HourlyRate = FixedHourlyRate });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Claim claim)
    {
        // Values that determine payment are always set on the server.
        claim.HourlyRate = FixedHourlyRate;
        claim.TotalClaimAmount = claim.HoursWorked * FixedHourlyRate;
        claim.DateSubmitted = DateTime.Now;
        claim.Status = ClaimStatus.Pending;

        if (!ModelState.IsValid)
            return View(claim);

        context.Claims.Add(claim);
        await context.SaveChangesAsync();
        TempData["Success"] = $"Claim #{claim.ClaimId} was submitted successfully and is pending approval.";
        return RedirectToAction(nameof(Index), new { studentNumber = claim.StudentNumber });
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? studentNumber)
    {
        var cutoff = DateTime.Now.AddMonths(-12);
        var query = context.Claims.AsNoTracking().Where(c => c.DateSubmitted >= cutoff);

        if (!string.IsNullOrWhiteSpace(studentNumber))
            query = query.Where(c => c.StudentNumber == studentNumber.Trim());

        var model = new ClaimSearchViewModel
        {
            StudentNumber = studentNumber,
            Claims = await query.OrderByDescending(c => c.DateSubmitted).ToListAsync()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, string? studentNumber)
    {
        var claim = await context.Claims.FindAsync(id);
        if (claim is null)
        {
            TempData["Error"] = "The selected claim could not be found.";
        }
        else if (claim.Status != ClaimStatus.Pending)
        {
            TempData["Error"] = claim.Status == ClaimStatus.Approved
                ? "Approved claims cannot be cancelled."
                : "This claim has already been cancelled.";
        }
        else
        {
            claim.Status = ClaimStatus.Cancelled;
            await context.SaveChangesAsync();
            TempData["Success"] = $"Claim #{claim.ClaimId} has been cancelled.";
        }

        return RedirectToAction(nameof(Index), new { studentNumber });
    }
}
