using GroupDocs.Conversion;
using Microsoft.AspNetCore.Mvc;
using GroupDocs.Conversion.Options.Convert;

namespace GroupDocsDemoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConvertController: ControllerBase
{
    [HttpPost("to/{type}")]
    public IActionResult ConvertToFormat([FromRoute] string type, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        if (string.IsNullOrWhiteSpace(type))
            return BadRequest("Target format type is required (e.g. pdf, docx, html).");

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(uploadsPath);

        var inputPath = Path.Combine(uploadsPath, file.FileName);
        using (var stream = new FileStream(inputPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var outputFileName = Path.GetFileNameWithoutExtension(file.FileName) + $".{type.ToLower()}";
        var outputPath = Path.Combine(uploadsPath, outputFileName);

        try
        {
            using (var converter = new Converter(inputPath))
            {
                var saveOptions = GetConvertOptions(type);
                converter.Convert(outputPath , saveOptions);
            }
        }
        catch
        {
            return BadRequest("Unsupported format or conversion error.");
        }

        var resultBytes = System.IO.File.ReadAllBytes(outputPath);
        return File(resultBytes, "application/octet-stream", outputFileName);
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