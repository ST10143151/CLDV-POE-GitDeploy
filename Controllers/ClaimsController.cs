using ABCRetailers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ABCRetailers.Data;
using Microsoft.EntityFrameworkCore;


namespace ABCRetailers.Controllers
{
    [Authorize] // Ensures that only authenticated users can access this controller
    public class ClaimsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ClaimsController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context=context;
        }

        // Sample claims (this should be fetched from the database in production)
        private static List<Claim> _claims = new List<Claim>
        {
            new Claim
            {
                Id = 1,
                Name = "John Doe",
                Description = "Annual Leave",
                Status = "Pending",
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
                Reason = "Family event"
            },
            new Claim
            {
                Id = 2,
                Name = "Jane Smith",
                Description = "Sick Leave",
                Status = "Approved",
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                StartDate = DateTime.Now.AddDays(-3),
                EndDate = DateTime.Now,
                Reason = "Flu recovery"
            }
        };

       // GET: Claims
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Fetch claims from the database
            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Coordinator"))
            {
                // Admins and Coordinators see all claims
                var claims = await _context.Claims.ToListAsync();
                return View(claims);
            }
            else
            {
                // Lecturers see only their own claims
                var userClaims = await _context.Claims
                    .Where(c => c.Name == user.UserName)
                    .ToListAsync();
                return View(userClaims);
            }
        }

        // GET: Claims/Create
        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claim claim, IFormFile Document)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            claim.Name = user.UserName;  // Set claim owner
            claim.Status = "Pending";

            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            if (claim.EndDate < claim.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be earlier than start date");
                return View(claim);
            }

            // Handle file upload
            if (Document != null)
            {
                // Ensure a unique file name (using a timestamp or GUID)
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = $"{Guid.NewGuid()}_{Document.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Document.CopyToAsync(stream);
                }

                // Store the file path in the claim
                claim.DocumentPath = uniqueFileName;
            }

            // Add the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





        // GET: Claims/Edit/5
        [Authorize(Roles = "Lecturer,Admin,Coordinator")]
        public async Task<IActionResult> Edit(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Ensure lecturers can only edit their own claims
            if (user == null || (await _userManager.IsInRoleAsync(user, "Lecturer") && claim.Name != user.UserName))
            {
                return Forbid(); // Prevents access to claims not belonging to the logged-in lecturer
            }

            return View(claim);
        }

        // POST: Claims/Edit/5
        [HttpPost]
        [Authorize(Roles = "Lecturer,Admin,Coordinator")]
        public async Task<IActionResult> Edit(int id, Claim updatedClaim)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Ensure lecturers can only edit their own claims
            if (user == null || (await _userManager.IsInRoleAsync(user, "Lecturer") && claim.Name != user.UserName))
            {
                return Forbid(); // Prevents access to claims not belonging to the logged-in lecturer
            }

            if (!ModelState.IsValid)
            {
                return View(updatedClaim);
            }

            // Update the properties of the existing claim
            claim.Description = updatedClaim.Description;
            claim.StartDate = updatedClaim.StartDate;
            claim.EndDate = updatedClaim.EndDate;
            claim.Reason = updatedClaim.Reason;

            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Details/5
        [Authorize(Roles = "Lecturer,Admin,Coordinator")]
        public async Task<IActionResult> Details(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Ensure lecturers can only view details of their own claims
            if (user == null || (await _userManager.IsInRoleAsync(user, "Lecturer") && claim.Name != user.UserName))
            {
                return Forbid();
            }

            return View(claim);
        }

        // GET: Claims/Delete/5
        [Authorize(Roles = "Lecturer,Admin,Coordinator")]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Ensure lecturers can only delete their own claims
            if (user == null || (await _userManager.IsInRoleAsync(user, "Lecturer") && claim.Name != user.UserName))
            {
                return Forbid();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Lecturer,Admin,Coordinator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                var user = await _userManager.GetUserAsync(User);

                // Ensure lecturers can only delete their own claims
                if (user != null && await _userManager.IsInRoleAsync(user, "Lecturer") && claim.Name != user.UserName)
                {
                    return Forbid();
                }

                _claims.Remove(claim);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Approve/5
        [Authorize(Roles = "Admin,Coordinator")]
        public IActionResult Approve(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Approved";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Reject/5
        [Authorize(Roles = "Admin,Coordinator")]
        public IActionResult Reject(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = "Rejected";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
