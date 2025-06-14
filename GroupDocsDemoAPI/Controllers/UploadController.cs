using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {   
        string relativePath = await fileUploadService.UploadFile(file);
        return Ok(new { path = "/" + relativePath });
    }
}