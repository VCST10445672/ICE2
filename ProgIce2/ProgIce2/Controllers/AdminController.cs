using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgIce2.Data;
using ProgIce2.Models;

namespace ProgIce2.Controllers;

public class AdminController(BursaryClaimsContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Claims
        .AsNoTracking().OrderByDescending(c => c.DateSubmitted).ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var claim = await context.Claims.FindAsync(id);
        if (claim?.Status == ClaimStatus.Pending)
        {
            claim.Status = ClaimStatus.Approved;
            await context.SaveChangesAsync();
            TempData["Success"] = $"Claim #{claim.ClaimId} has been approved.";
        }
        else
            TempData["Error"] = "Only pending claims can be approved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var claim = await context.Claims.FindAsync(id);
        if (claim?.Status == ClaimStatus.Pending)
        {
            claim.Status = ClaimStatus.Cancelled;
            await context.SaveChangesAsync();
            TempData["Success"] = $"Claim #{claim.ClaimId} has been rejected.";
        }
        else
            TempData["Error"] = "Only pending claims can be rejected.";
        return RedirectToAction(nameof(Index));
    }
}
