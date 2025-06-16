using System.Drawing;
using GroupDocs.Signature;
using GroupDocs.Signature.Domain;
using GroupDocs.Signature.Options;
using GroupDocsDemoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupDocsDemoAPI.Controllers;

[Route("api/[controller]")]
public class SignatureController: ControllerBase
{
    private readonly FileUploadService fileUploadService = new FileUploadService();
    
    [HttpPost("addImageSignature")]
    public async Task<IActionResult> AddImageSignature([FromForm] IFormFile file, [FromForm] IFormFile sign)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        string uploadSignPath = await fileUploadService.UploadFile(sign);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            ImageSignOptions options = new ImageSignOptions(uploadSignPath)
            {
                // set signature position
                Left = 500,
                Top = 650,
                //
                AllPages = true
            };
            signature.Sign("ImageSigned.pdf", options);
            return Ok("Signed Successfully, Check ImageSigned.pdf");
        }
    }
    
    [HttpPost("addTextSignature")]
    public async Task<IActionResult> AddTextSignature([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            TextSignOptions options = new TextSignOptions("iBAS++")
            {
                // set signature position
                Left = 500,
                Top = 650,
                // set signature rectangle
                Width = 100,
                Height = 30,
                Font = new SignatureFont { Size = 24, FamilyName = "Comic Sans MS" },
                ForeColor = Color.Brown
            };
            signature.Sign("TextSigned.pdf", options);
            return Ok("Signed Successfully, Check TextSigned.pdf");
        }
    }
    
    [HttpPost("addBarCodeSignature")]
    public async Task<IActionResult> AddBarcodeSignature([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            BarcodeSignOptions options = new BarcodeSignOptions("iBAS++")
            {
                EncodeType = BarcodeTypes.Code128,
                Left = 100,
                Top = 100
            };
            signature.Sign("BarcodeSigned.pdf", options);
            return Ok("Signed Successfully, Check BarcodeSigned.pdf");
        }
    }
    
    [HttpPost("addQRCodeSignature")]
    public async Task<IActionResult> AddQRCodeSignature([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            QrCodeSignOptions options = new QrCodeSignOptions("iBAS++")
            {
                EncodeType = QrCodeTypes.QR,
                Left = 100,
                Top = 100
            };
            signature.Sign("QRCodeSigned.pdf", options);
            return Ok("Signed Successfully, Check QRCodeSigned.pdf");
        }
    }
    
    [HttpPost("addDigitalSignature")]
    public async Task<IActionResult> AddDigitalSignature([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            DigitalSignOptions options = new DigitalSignOptions("self-signed-cert.pfx")
            {
                Left = 100,
                Top = 100,
                Password = "abc123"
            };
            signature.Sign("DigitallySigned.pdf", options);
            return Ok("Signed Successfully, Check DigitallySigned.pdf");
        }
    }
    
    [HttpPost("verifySignature")]
    public async Task<IActionResult> VerifyDigitalSignature([FromForm] IFormFile file)
    {
        string uploadedRelativePath = await fileUploadService.UploadFile(file);
        
        using (Signature signature = new Signature(uploadedRelativePath))
        {
            DigitalVerifyOptions options = new DigitalVerifyOptions("self-signed-cert.pfx")
            {
                Password = "abc123"
            };
            VerificationResult result = signature.Verify(options);
            Console.WriteLine("Verification Result: " + result);
            if (result.IsValid)
            {
                return Ok("Signature Verified");
            }
            else
            {
                return BadRequest("Signature Not Matched");
            }
            
        }
    }
}