using System.Diagnostics;
using GroupDocs.Conversion;
using GroupDocs.Conversion.Caching;
using GroupDocs.Conversion.Contracts;
using Microsoft.AspNetCore.Mvc;
using GroupDocs.Conversion.Options.Convert;
using GroupDocs.Conversion.Options.Load;
using GroupDocsDemoAPI.Services;

namespace GroupDocsDemoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConvertController: ControllerBase
{   
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
            converter.Convert(outputPath , saveOptions);
        }

        var resultBytes = System.IO.File.ReadAllBytes(outputPath);
        return File(resultBytes, "application/octet-stream", outputPath);
    }
    
    [HttpPost("excel/to/{type}")]
    public async  Task<IActionResult> ExcelConvertToFormat([FromRoute] string type, [FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        string outputPath = fileUploadService.getFileOutputPath(file.FileName, type);
        FileCache cache = new FileCache("wwwroot");

        Func<ConverterSettings> settingsFactory = () => new ConverterSettings
        {
            Cache = cache
        };
        
        using (var converter = new Converter(uploadedRelativePath , settingsFactory))
        {
            var saveOptions = GetConvertOptions(type);
            Stopwatch stopwatch = Stopwatch.StartNew();
            converter.Convert(outputPath, saveOptions);
            stopwatch.Stop();
            return Ok($"Conversion completed in {stopwatch.ElapsedMilliseconds}ms");    
        }
        
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