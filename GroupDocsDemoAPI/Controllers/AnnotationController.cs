using GroupDocs.Annotation;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class AnnotationController: ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpGet("supportedFileTypes")]
    public async Task<IActionResult> GetSupportedFileTypes()
    {
        IEnumerable<FileType> supportedFileTypes = FileType
            .GetSupportedFileTypes()
            .OrderBy(f => f.Extension);
        
        return Ok(supportedFileTypes);
    }

    [HttpPost("fileInfo")]
    public async Task<IActionResult> UploadFileInfo([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);

        using (Annotator annotator = new Annotator(uploadedRelativePath))
        {
            IDocumentInfo info = annotator.Document.GetDocumentInfo();
            return Ok(info);
        }
    }
}