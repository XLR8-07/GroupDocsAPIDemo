using System.Drawing;
using System.Text;
using GroupDocs.Parser;
using GroupDocs.Parser.Data;
using GroupDocs.Parser.Options;
using GroupDocs.Parser.Templates;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class ParserController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();

    [HttpPost("extractText")]
    public async Task<IActionResult> ExtractText([FromForm] IFormFile file)
    {
        string uploadFilePath = await fileUploadService.UploadFile(file);

        using (Parser parser = new Parser(uploadFilePath))
        {
            TextReader reader = parser.GetText();
            return Ok(reader.ReadToEnd());
        }
    }

    [HttpPost("fetchEncoding")]
    public async Task<IActionResult> FetchEncoding([FromForm] IFormFile file)
    {
        string uploadFilePath = await fileUploadService.UploadFile(file);
        
        LoadOptions loadOptions = new LoadOptions(FileFormat.WordProcessing , null, null , Encoding.GetEncoding(1251));
        
        using (Parser parser = new Parser(uploadFilePath , loadOptions))
        {
            TextDocumentInfo info = parser.GetDocumentInfo() as TextDocumentInfo;
            return Ok(info.Encoding.WebName);
        }
    }

    [HttpPost("extractTable")]
    public async Task<IActionResult> ExtractTable([FromForm] IFormFile file)
    {
        string uploadFilePath = await fileUploadService.UploadFile(file);

        using (Parser parser = new Parser(uploadFilePath))
        {
            if (!parser.Features.Tables)
            {
                return BadRequest("Document Doesn't support table extraction");
            }
            
            TemplateTableLayout layout = new TemplateTableLayout(
                    new double[] { 50, 95, 275, 415, 485, 545 },
                    new double[] { 325, 340, 365, 395 }
                );
            
            PageTableAreaOptions options = new PageTableAreaOptions(layout);
            IEnumerable<PageTableArea> tables = parser.GetTables(options);
            return Ok(tables);
        }
    }
}