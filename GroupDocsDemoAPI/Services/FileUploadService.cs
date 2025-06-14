namespace GroupDocsDemoAPI.Services;

public class FileUploadService
{
    private readonly string _rootUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    private readonly string _rootOutputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "output");

    public async Task<string> UploadFile(IFormFile file)
    {
        Directory.CreateDirectory(_rootUploadPath);
        
        var fileExtension = Path.GetExtension(file.FileName)?.TrimStart('.').ToLower();
        // if (string.IsNullOrWhiteSpace(fileExtension))
        //     return BadRequest("File has no valid extension.");
        
        var typeDir = Path.Combine(_rootUploadPath, fileExtension);
        Directory.CreateDirectory(typeDir);
        
        var originalFileName = Path.GetFileName(file.FileName);
        var sanitizedFileName = SanitizeFileName(originalFileName);
        var fullPath = Path.Combine(typeDir, sanitizedFileName);
        
        fullPath = EnsureUniqueFilePath(fullPath);
        
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        string relativePath = fullPath[fullPath.IndexOf("wwwroot")!..].Replace("\\", "/");
        return relativePath;
    }
    
    private string EnsureUniqueFilePath(string filePath)
    {
        int counter = 1;
        string directory = Path.GetDirectoryName(filePath)!;
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        string extension = Path.GetExtension(filePath);

        while (File.Exists(filePath))
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

    public string getRootUploadPath()
    {
        return _rootUploadPath;
    }

    public string getRootOutputPath()
    {
        return _rootOutputPath;
    }

    public string getFileOutputPath(string fileName, string fileExtension)
    {
        Directory.CreateDirectory(_rootOutputPath);
        
        var typeDir = Path.Combine(_rootOutputPath, fileExtension);
        Directory.CreateDirectory(typeDir);
        
        var originalFileName = Path.GetFileName(fileName);
        var sanitizedFileName = SanitizeFileName(originalFileName);
        var fullPath = Path.Combine(typeDir, sanitizedFileName);
        
        fullPath = EnsureUniqueFilePath(fullPath);
        string relativePath = fullPath[fullPath.IndexOf("wwwroot")!..].Replace("\\", "/");
        relativePath = Path.ChangeExtension(relativePath, fileExtension);
        return relativePath;
    }
}