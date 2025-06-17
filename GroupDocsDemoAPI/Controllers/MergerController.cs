using GroupDocs.Merger;
using GroupDocs.Merger.Domain.Options;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class MergerController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost("merge")]
    public async Task<IActionResult> Merge()
    {
        string filePath = @"wwwroot/uploads/docx/8bea534f51ef00e35903940fc32aadfc.docx";
        string filePath2 = @"wwwroot/uploads/docx/DemoDoc.docx";
        
        string filePathOut = @"wwwroot/output/pdf/Merged_Test5.docx";

        PdfJoinOptions joinOptions = new PdfJoinOptions();

        using (Merger merger = new Merger(filePath))
        {
            merger.Join(filePath2, joinOptions);
            merger.Save(filePathOut);
        }
        return Ok();
    }
    
}