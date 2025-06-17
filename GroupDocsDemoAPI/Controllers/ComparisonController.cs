using GroupDocs.Comparison;
using GroupDocs.Comparison.Options;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class ComparisonController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();

    [HttpPost("compareDocx")]
    public async Task<IActionResult> CompareDocx([FromForm] IFormFile file1, [FromForm] IFormFile file2)
    {
        string filePath1 = await fileUploadService.UploadFile(file1);
        string filePath2 = await fileUploadService.UploadFile(file2);
        
        using (Comparer comparer = new Comparer(filePath1))
        {
            comparer.Add(filePath2);
            CompareOptions options = new CompareOptions() { ExtendedSummaryPage = true};
            comparer.Compare("extendedSummary.docx", options);
            return Ok("Comparison completed");
        }
    }
}