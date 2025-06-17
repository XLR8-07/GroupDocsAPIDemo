using GroupDocs.Redaction;
using GroupDocs.Redaction.Redactions;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class RedactionController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost("hideText")]
    public async Task<IActionResult> HideText([FromForm] IFormFile file)
    {
        string uploadFilePath = await fileUploadService.UploadFile(file);

        using (Redactor redactor = new Redactor(uploadFilePath))
        {
            RedactorChangeLog result =
                redactor.Apply(new ExactPhraseRedaction("QUICK BROWN FOX", true, new ReplacementOptions("[Sensitive]")));
            if (result.Status != RedactionStatus.Failed)
            {
                redactor.Save();
            }
            return Ok(result.Status);
        }
    }

    [HttpPost("hideMetadata")]
    public async Task<IActionResult> HideMetadata([FromForm] IFormFile file)
    {
        string uploadFilePath = await fileUploadService.UploadFile(file);
        using (Redactor redactor = new Redactor(uploadFilePath))
        {
            redactor.Apply(new EraseMetadataRedaction(MetadataFilters.All));
            redactor.Save();
            return Ok();
        }
        
    }
}