namespace GroupDocsDemoAPI.Models;

public class AnnotationAreaModel
{
    public string filePath { get; set; }
    public int BackgroundColor { get; set; } = 65535;
    public DateTime CreatedOn {get; set;} = DateTime.UtcNow;
    public string Message {get; set;} = "This is a message";
    public int? Opacity {get; set;}
    public int PageNumber { get; set; } = 0;
    public int PenColor { get; set; } = 65535;
    // Replies = new List<Reply>
    // {
    //     new Reply
    //     {
    //         Comment = "First comment",
    //         RepliedOn = DateTime.Now
    //     },
    //     new Reply
    //     {
    //         Comment = "Second comment",
    //         RepliedOn = DateTime.Now
    //     }
    // }
}