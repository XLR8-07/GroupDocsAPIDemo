using GroupDocs.Editor;
using GroupDocs.Editor.Formats;
using GroupDocs.Editor.Metadata;
using GroupDocs.Editor.Options;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class EditorController : ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost("loadDocument")]
    public async Task<IActionResult> LoadDocument([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);

        Editor editor = new Editor(uploadedRelativePath);
        
        WordProcessingEditOptions editOptions = new WordProcessingEditOptions();
        editOptions.EnableLanguageInformation = true;
        
        EditableDocument readyToEdit = editor.Edit(editOptions);
        
        return Ok(readyToEdit.GetEmbeddedHtml());
    }

    [HttpPost("saveDocument")]
    public async Task<IActionResult> SaveDocument([FromBody] string content)
    {
        
        Editor editor = new Editor("wwwroot/uploads/docx/8bea534f51ef00e35903940fc32aadfc_7.docx");
        EditableDocument afterEdit = EditableDocument.FromMarkup(content , null);
        WordProcessingSaveOptions saveOptions = new WordProcessingSaveOptions(WordProcessingFormats.Docx);
        PdfSaveOptions pdfSaveOptions = new PdfSaveOptions();
        
        editor.Save(afterEdit, "EditedDoc.docx" , saveOptions);
        editor.Save(afterEdit,"EditedDoc.pdf", pdfSaveOptions);
        return Ok("Edited File Saved Successfully");
    }

    [HttpPost("getDocumentMetadata")]
    public async Task<IActionResult> GetDocumentMetadata([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        Editor editor = new Editor(uploadedRelativePath);
        IDocumentInfo infoWordDoc = editor.GetDocumentInfo(null);
        return Ok(infoWordDoc);
    }
    
}