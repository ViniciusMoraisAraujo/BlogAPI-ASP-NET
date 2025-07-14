namespace BlogAs.ViewModels.Posts;

public class PostResponseViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string CategoryName { get; set; }
    public string AuthorName { get; set; }
}