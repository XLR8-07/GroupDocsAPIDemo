using GroupDocs.Conversion;
using GroupDocs.Conversion.Contracts;
using Microsoft.AspNetCore.Mvc;
using GroupDocs.Conversion.Options.Convert;
using GroupDocsDemoAPI.Services;

namespace GroupDocsDemoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConvertController: ControllerBase
{   
    // private readonly ConversionService conversionService;
    private readonly FileUploadService fileUploadService = new FileUploadService();

    [HttpPost("info")]
    public async Task<IActionResult> GetInfo([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Converter converter = new Converter(uploadedRelativePath))
        {
            IDocumentInfo info = converter.GetDocumentInfo();

            return Ok(info);
        }
    }
    [HttpPost("formats")]
    public async Task<IActionResult> getConversionTypes([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Converter converter = new Converter(uploadedRelativePath))
        {
            PossibleConversions possibleConversions = converter.GetPossibleConversions();
            return Ok(possibleConversions.All);
        }
    }
    
    [HttpPost("word/to/{type}")]
    public async Task<IActionResult> WordConvertToFormat([FromRoute] string type, [FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        string outputPath = fileUploadService.getFileOutputPath(file.FileName, type);
        
        using (var converter = new Converter(uploadedRelativePath))
        {
            var saveOptions = GetConvertOptions(type);
            Console.WriteLine($"Converting {type} to {outputPath}");
            converter.Convert(outputPath , saveOptions);
        }

        var resultBytes = System.IO.File.ReadAllBytes(outputPath);
        return File(resultBytes, "application/octet-stream", outputPath);
    }

    [HttpPost("pdf/to/{type}")]
    public async  Task<IActionResult> PDFConvertToFormat([FromRoute] string type, [FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        string outputPath = fileUploadService.getFileOutputPath(file.FileName, type);
        
        using (var converter = new Converter(uploadedRelativePath))
        {
            var saveOptions = GetConvertOptions(type);
            Console.WriteLine($"Converting {type} to {outputPath}");
            converter.Convert(outputPath , saveOptions);
        }

        var resultBytes = System.IO.File.ReadAllBytes(outputPath);
        return File(resultBytes, "application/octet-stream", outputPath);
    }
    
    private static ConvertOptions GetConvertOptions(string format)
    {
        return format.ToLower() switch
        {
            "pdf" => new PdfConvertOptions(),
            "docx" => new WordProcessingConvertOptions(),
            "xlsx" => new SpreadsheetConvertOptions(),
            "pptx" => new PresentationConvertOptions(),
            _ => throw new NotSupportedException($"Format {format} not supported.")
        };
    }
}