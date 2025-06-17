using GroupDocs.Merger;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class MergerController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost("merge")]
    public async Task<IActionResult> Merge([FromForm] IFormFile file1, [FromForm] IFormFile file2)
    {
        string file1Path = await fileUploadService.UploadFile(file1);
        string file2Path = await fileUploadService.UploadFile(file2);

        using (Merger merger = new Merger(file1Path))
        {
            merger.Join(file2Path);
            merger.Save("TestPDF.pdf");
            return Ok("File Merged Successfully");
        }
    }
    
}