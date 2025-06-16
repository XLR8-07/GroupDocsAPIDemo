using System.Xml.Serialization;
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
            return Ok("File Annotated Successfully, Check result.docx");
        }
    }
    
    [HttpPost("addArrowAnnotation")]
    public async Task<IActionResult> AddArrowAnnotation([FromBody] AnnotationAreaModel areaAnnotation)
    {
        if (areaAnnotation.filePath == null)
            return BadRequest("File path is required");
        
        using (Annotator annotator = new Annotator(areaAnnotation.filePath))
        {
            ArrowAnnotation area = new ArrowAnnotation
            {
                Box = new Rectangle(400, 650, 200, 150),
                CreatedOn = areaAnnotation.CreatedOn,
                Message = areaAnnotation.Message,
                Opacity = areaAnnotation.Opacity,
                PageNumber = areaAnnotation.PageNumber,
                PenColor = areaAnnotation.PenColor,
                PenStyle = PenStyle.Dot,
                PenWidth = 3,
                Replies = new List<Reply>
                {
                    new Reply
                    {
                        Comment = "This is my Reply",
                        RepliedOn = DateTime.Now,
                    }
                }
            };
            annotator.Add(area);
            annotator.Save("result.docx");
            return Ok("File Annotated Successfully, Check result.docx");
        }
    }
    
    [HttpPost("addHighlightAnnotation")]
    public async Task<IActionResult> AddHighlightAnnotation([FromBody] AnnotationAreaModel areaAnnotation)
    {
        if (areaAnnotation.filePath == null)
            return BadRequest("File path is required");
        
        using (Annotator annotator = new Annotator(areaAnnotation.filePath))
        {
            HighlightAnnotation area = new HighlightAnnotation
            {
                BackgroundColor = 16776960,
                CreatedOn = areaAnnotation.CreatedOn,
                Message = areaAnnotation.Message,
                Opacity = areaAnnotation.Opacity,
                PageNumber = 0,
                Points = new List<Point>
                {
                    new Point(80 , 730), new Point(240 , 730), new Point(80, 650), new Point(240 , 650)
                },
                Replies = new List<Reply>
                {
                    new Reply
                    {
                        Comment = "This is my Reply",
                        RepliedOn = DateTime.Now,
                    }
                }
            };
            annotator.Add(area);
            annotator.Save("result.docx");
            return Ok("File Annotated Successfully, Check result.docx");
        }
    }
    
    [HttpPost("addPolyLineAnnotation")]
    public async Task<IActionResult> AddPolyLineAnnotation([FromBody] AnnotationAreaModel areaAnnotation)
    {
        if (areaAnnotation.filePath == null)
            return BadRequest("File path is required");
        
        using (Annotator annotator = new Annotator(areaAnnotation.filePath))
        {
            PolylineAnnotation area = new PolylineAnnotation
            {
                Box = new Rectangle(250, 35, 102, 12),
                CreatedOn = areaAnnotation.CreatedOn,
                Message = areaAnnotation.Message,
                Opacity = areaAnnotation.Opacity,
                PageNumber = 0,
                Replies = new List<Reply>
                {
                    new Reply
                    {
                        Comment = "This is my Reply",
                        RepliedOn = DateTime.Now,
                    }
                },
                SvgPath = "M250.8280751173709,48.209295774647885l0.6986854460093896,0l0.6986854460093896,-1.3973708920187793l0.6986854460093896,0l0.6986854460093896,-1.3973708920187793l1.3973708920187793,-0.6986854460093896l0.6986854460093896,-0.6986854460093896l0.6986854460093896,0l2.096056338028169,-1.3973708920187793l3.493427230046948,-1.3973708920187793l0.6986854460093896,-0.6986854460093896l1.3973708920187793,-1.3973708920187793l0.6986854460093896,0l1.3973708920187793,-0.6986854460093896l0.6986854460093896,0l0.6986854460093896,-0.6986854460093896l0.6986854460093896,0l0.6986854460093896,0l0,-0.6986854460093896l0.6986854460093896,0l0.6986854460093896,0l1.3973708920187793,0l0,-0.6986854460093896l0.6986854460093896,0l1.3973708920187793,0l0.6986854460093896,0l1.3973708920187793,0l0.6986854460093896,0l2.096056338028169,-0.6986854460093896l1.3973708920187793,0l0.6986854460093896,0l0.6986854460093896,0l1.3973708920187793,0l1.3973708920187793,0l1.3973708920187793,0l2.096056338028169,0l5.589483568075117,0l1.3973708920187793,0l2.096056338028169,0l0.6986854460093896,0l1.3973708920187793,0l0.6986854460093896,0l1.3973708920187793,0l1.3973708920187793,0l0.6986854460093896,0.6986854460093896l1.3973708920187793,0l2.096056338028169,1.3973708920187793l0.6986854460093896,0l0.6986854460093896,0l0,0.6986854460093896l1.3973708920187793,0l0.6986854460093896,0.6986854460093896l1.3973708920187793,0.6986854460093896l0,0.6986854460093896l0.6986854460093896,0l1.3973708920187793,0.6986854460093896l1.3973708920187793,0.6986854460093896l3.493427230046948,0.6986854460093896l1.3973708920187793,0.6986854460093896l2.096056338028169,0.6986854460093896l1.3973708920187793,0.6986854460093896l1.3973708920187793,0l1.3973708920187793,0.6986854460093896l0.6986854460093896,0l0.6986854460093896,0.6986854460093896l1.3973708920187793,0l0.6986854460093896,0l0.6986854460093896,0l2.7947417840375586,0l1.3973708920187793,0l0.6986854460093896,0l1.3973708920187793,0l0.6986854460093896,0l0.6986854460093896,0l1.3973708920187793,0l0.6986854460093896,0l2.7947417840375586,0l0.6986854460093896,0l2.7947417840375586,0l1.3973708920187793,0l0.6986854460093896,0l0.6986854460093896,0l0.6986854460093896,0l0.6986854460093896,0l0.6986854460093896,0l0.6986854460093896,0l0.6986854460093896,-0.6986854460093896l0.6986854460093896,0"
            };
            annotator.Add(area);
            annotator.Save("result.docx");
            return Ok("File Annotated Successfully, Check result.docx");
        }
    }
    
    [HttpPost("extractAnnotations")]
    public async Task<IActionResult> ExtractAnnotation([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Annotator annotator = new Annotator(uploadedRelativePath))
        {
            List<AnnotationBase> annotations = annotator.Get();
            XmlSerializer formatter = new XmlSerializer(typeof(List<AnnotationBase>));
            using (FileStream fs = new FileStream("annotations.xml", FileMode.Create))
            {
            	fs.SetLength(0);
                formatter.Serialize(fs, annotations);
            }
            return Ok(annotations);
        }
    }
    
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveAnnotation([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Annotator annotator = new Annotator(uploadedRelativePath))
        {
            var annotations = annotator.Get();
            annotator.Remove(annotations[0]);
            annotator.Save("removed_Annotation.docx");
            return Ok("Successfully removed annotation");
        }
    }
}