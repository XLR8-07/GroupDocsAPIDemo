namespace GroupDocsDemoAPI.Utilities;

public class FileUtilities
{
    private readonly string _storageRoot;

    public FileUtilities()
    {
        this._storageRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }
    // Utility: Save uploaded file
    public async Task<string> SaveUploadedFile(IFormFile file)
    {
        string uploadDir = Path.Combine(_storageRoot, "uploads");
        Directory.CreateDirectory(uploadDir);

        string filePath = Path.Combine(uploadDir, Path.GetRandomFileName() + Path.GetExtension(file.FileName));
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return filePath;
    }

    // Utility: Create output directory per upload
    public string CreateOutputDir(string filePath)
    {
        string outputDir = Path.Combine(_storageRoot, "output", Path.GetFileNameWithoutExtension(filePath));
        Directory.CreateDirectory(outputDir);
        return outputDir;
    }
}