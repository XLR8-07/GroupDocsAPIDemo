using Microsoft.AspNetCore.Mvc;

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

        return Ok();
    }
}