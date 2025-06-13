using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly string _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {   
        Directory.CreateDirectory(_rootPath);
        
        var fileExtension = Path.GetExtension(file.FileName)?.TrimStart('.').ToLower();
        if (string.IsNullOrWhiteSpace(fileExtension))
            return BadRequest("File has no valid extension.");
        
        var typeDir = Path.Combine(_rootPath, fileExtension);
        Directory.CreateDirectory(typeDir);
        
        var originalFileName = Path.GetFileName(file.FileName);
        var sanitizedFileName = SanitizeFileName(originalFileName);
        var fullPath = Path.Combine(typeDir, sanitizedFileName);
        
        fullPath = EnsureUniqueFilePath(fullPath);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        var relativePath = fullPath[(fullPath.IndexOf("uploads")!..)].Replace("\\", "/");
        return Ok(new { path = "/" + relativePath });
        
    }
    
    private string EnsureUniqueFilePath(string filePath)
    {
        int counter = 1;
        string directory = Path.GetDirectoryName(filePath)!;
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        string extension = Path.GetExtension(filePath);

        while (System.IO.File.Exists(filePath))
        {
            filePath = Path.Combine(directory, $"{fileNameWithoutExt}_{counter}{extension}");
            counter++;
        }

        return filePath;
    }
    
    private string SanitizeFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c, '_');
        return fileName;
    }
}