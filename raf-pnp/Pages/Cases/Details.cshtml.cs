using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Cases
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DetailsModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        [BindProperty]
        public DocumentType UploadDocumentType { get; set; }

        [BindProperty]
        public string? UploadDescription { get; set; }

        public RafCase? RafCase { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            RafCase = await _context.Cases
                .Include(c => c.Client)
                .Include(c => c.Documents)
                .Include(c => c.ExpertAppointments)
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (RafCase == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, CaseStatus newStatus)
        {
            var rafCase = await _context.Cases.FindAsync(id);
            
            if (rafCase == null)
            {
                return NotFound();
            }

            var oldStatus = rafCase.Status;
            rafCase.Status = newStatus;

            // Add activity for status change
            var activity = new CaseActivity
            {
                CaseId = id,
                ActivityType = ActivityType.StatusChanged,
                Title = "Status Updated",
                Description = $"Status changed from {oldStatus} to {newStatus}",
                ActivityDate = DateTime.Now,
                CreatedBy = "System"
            };

            _context.CaseActivities.Add(activity);

            // Update relevant dates based on status
            switch (newStatus)
            {
                case CaseStatus.ComplianceLodgement:
                    rafCase.ComplianceLodgementDate = DateTime.Today;
                    rafCase.StatutoryExpiryDate = DateTime.Today.AddDays(120);
                    break;
                case CaseStatus.SummonsIssued:
                    rafCase.SummonsIssueDate = DateTime.Today;
                    break;
                case CaseStatus.Finalised:
                case CaseStatus.Closed:
                    rafCase.DateClosed = DateTime.Today;
                    break;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostUploadDocumentAsync(int id)
        {
            if (UploadFile == null)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return await OnGetAsync(id);
            }

            // Validate file size (10 MB max)
            const long maxFileSize = 10 * 1024 * 1024;
            if (UploadFile.Length > maxFileSize)
            {
                ModelState.AddModelError("", "File size cannot exceed 10 MB.");
                return await OnGetAsync(id);
            }

            try
            {
                // Generate secure filename
                var fileExtension = Path.GetExtension(UploadFile.FileName);
                var secureFileName = $"{Guid.NewGuid()}{fileExtension}";
                
                // Create directory path
                var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads", "cases", id.ToString());
                Directory.CreateDirectory(uploadsDir);
                
                // Full file path
                var filePath = Path.Combine(uploadsDir, secureFileName);
                
                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadFile.CopyToAsync(stream);
                }
                
                // Create database record
                var document = new CaseDocument
                {
                    CaseId = id,
                    DocumentName = UploadFile.FileName,
                    DocumentType = UploadDocumentType,
                    FilePath = $"/uploads/cases/{id}/{secureFileName}",
                    Description = UploadDescription,
                    DateUploaded = DateTime.Now,
                    UploadedBy = "System" // TODO: Replace with actual user when auth is implemented
                };

                _context.CaseDocuments.Add(document);
                await _context.SaveChangesAsync();

                // Add activity log
                var activity = new CaseActivity
                {
                    CaseId = id,
                    ActivityType = ActivityType.DocumentUploaded,
                    Title = "Document Uploaded",
                    Description = $"Uploaded {UploadFile.FileName} ({UploadDocumentType})",
                    ActivityDate = DateTime.Now,
                    CreatedBy = "System"
                };
                _context.CaseActivities.Add(activity);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Document uploaded successfully!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                return await OnGetAsync(id);
            }

            return RedirectToPage(new { id });
        }
    }
}
