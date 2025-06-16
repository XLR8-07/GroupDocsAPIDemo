using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using GroupDocsDemoAPI.Services;
using GroupDocsDemoAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewController : ControllerBase
{
       private FileUtilities fileUtilities = new FileUtilities();
       private readonly FileUploadService fileUploadService = new FileUploadService();

       [HttpPost()]
       public async Task<IActionResult> ViewFile([FromForm] IFormFile file)
       {
              string uploadedRelativePath = await fileUploadService.UploadFile(file);
              string outputPath = fileUploadService.getFileOutputPath(file.FileName, "html");
              
              using var viewer = new Viewer(uploadedRelativePath);
              var viewOptions = HtmlViewOptions.ForEmbeddedResources(outputPath);
              
              viewer.View(viewOptions);
              
              var resultBytes = System.IO.File.ReadAllBytes(outputPath);
              return File(resultBytes, "application/octet-stream", outputPath);
       }

       [HttpPost("{type}")]
       public async Task<IActionResult> View(string type, [FromForm] IFormFile file)
       {
              string filePath = await fileUtilities.SaveUploadedFile(file);
              string outputDir = fileUtilities.CreateOutputDir(filePath);
              string outputPath = Path.Combine(outputDir, "rendered.pdf");
              
              using var viewer = new Viewer(filePath);
              var options = getViewOptions(type , outputPath);

              viewer.View(options);

              var relativeUrl = $"/output/{Path.GetFileNameWithoutExtension(filePath)}/rendered.pdf";
              return Ok(relativeUrl);
              
       }
       
       [HttpPost("word/to/html")]
       public async Task<IActionResult> GetInfo([FromForm] IFormFile file)
       {
              string uploadedRelativePath = await fileUploadService.UploadFile(file);
              // string outputPath = fileUploadService.getFileOutputPath(file.FileName, "html");
              
        
              using (Viewer viewer = new Viewer(uploadedRelativePath))
              {
                     var viewOptions = HtmlViewOptions.ForExternalResources("page_{0}.html" , "page_{0}/resource_{0}_{1}" , "page_{0}/resource_{0}_{1}");
                     viewer.View(viewOptions);
                     // var resultBytes = System.IO.File.ReadAllBytes(outputPath);
                     
                     // return File(resultBytes, "application/octet-stream", outputPath);
                     return Ok();
              }
       }
       
       [HttpPost("word/to/pdf")]
       public async Task<IActionResult> ViewWordToPDF([FromForm] IFormFile file)
       {
              string uploadedRelativePath = await fileUploadService.UploadFile(file);
              // string outputPath = fileUploadService.getFileOutputPath(file.FileName, "pdf");
              
        
              using (Viewer viewer = new Viewer(uploadedRelativePath))
              {
                     var viewOptions = new PdfViewOptions();
                     viewOptions.DefaultFontName = "Nikosh";
                     viewer.View(viewOptions);
                     return Ok();
              }
       }
       
       [HttpPost("word/to/jpg")]
       public async Task<IActionResult> ViewWordToPNG([FromForm] IFormFile file)
       {
              string uploadedRelativePath = await fileUploadService.UploadFile(file);
              // string outputPath = fileUploadService.getFileOutputPath(file.FileName, "jpg");
              
        
              using (Viewer viewer = new Viewer(uploadedRelativePath))
              {
                     var viewOptions = new JpgViewOptions();
                     viewer.View(viewOptions);
                     return Ok();
              }
       }
       
       [HttpPost("pdf/to/html")]
       public async Task<IActionResult> ViewPDFToHTML([FromForm] IFormFile file)
       {
              string uploadedRelativePath = await fileUploadService.UploadFile(file);
              // string outputPath = fileUploadService.getFileOutputPath(file.FileName, "jpg");
              
        
              using (Viewer viewer = new Viewer(uploadedRelativePath))
              {
                     var viewOptions = HtmlViewOptions.ForEmbeddedResources("page_{0}.html");
                     viewer.View(viewOptions);
                     return Ok();
              }
       }
       
       private ViewOptions getViewOptions(string type, string filePath)
       {
              return type.ToLower() switch
              {
                     "pdf" => new PdfViewOptions(filePath),
                     // "html" => new HtmlViewOptions(filePath),
                     _ => throw new NotSupportedException($"Format {type} not supported.")
              };
       }
}