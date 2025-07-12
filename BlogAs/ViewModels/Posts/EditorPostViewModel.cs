namespace BlogAs.ViewModels.Posts;

public class EditorPostViewModel
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
    public int CategoryId { get; set; }
    public int TagId { get; set; }
    public int AuthorId { get; set; }
}