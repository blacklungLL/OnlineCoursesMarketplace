namespace LmsAndOnlineCoursesMarketplace.MVC.Models.ShoppingCart;

public class ShoppingCartVM
{
    public List<CourseSummaryVM> Courses { get; set; } = new();
    public decimal Balance { get; set; }
    public decimal TotalPrice => Courses.Sum(c => c.Price);
}

public class CourseSummaryVM
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public string ImageLink { get; set; }
    public string Category { get; set; }
    public string Language { get; set; }
    public string AuthorName { get; set; }
    public decimal Rating { get; set; }
    public string Duration { get; set; }
    public int Views {get; set; }
    public decimal Price { get; set; }
}