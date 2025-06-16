using GroupDocsDemoAPI.Models;
using GroupDocs.Annotation;
using GroupDocs.Annotation.Models;
using GroupDocs.Annotation.Models.AnnotationModels;
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

    [HttpPost("addAreaAnnotation")]
    public async Task<IActionResult> AddAnnotationArea([FromBody] AnnotationAreaModel areaAnnotation)
    {
        if (areaAnnotation.filePath == null)
            return BadRequest("File path is required");
        
        using (Annotator annotator = new Annotator(areaAnnotation.filePath))
        {
            AreaAnnotation area = new AreaAnnotation
            {
                BackgroundColor = areaAnnotation.BackgroundColor,
                Box = new Rectangle(100, 100, 100, 100),
                CreatedOn = areaAnnotation.CreatedOn,
                Message = areaAnnotation.Message,
                Opacity = areaAnnotation.Opacity,
                PageNumber = areaAnnotation.PageNumber,
                PenColor = areaAnnotation.PenColor,
                PenStyle = PenStyle.Dot,
                PenWidth = 3
            };
            annotator.Add(area);
            annotator.Save("result.docx");
            return Ok("File Annotated Successfully, Check Result.pdf");
        }
    }
}