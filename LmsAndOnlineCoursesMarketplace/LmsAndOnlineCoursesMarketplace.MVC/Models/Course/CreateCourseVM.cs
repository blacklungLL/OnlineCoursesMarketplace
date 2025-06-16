using System.ComponentModel.DataAnnotations;
namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Course;

public class CreateCourseVM
{
    [Required(ErrorMessage = "Title Required")]
    [StringLength(100, ErrorMessage = "Max 100 symbols")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Short Description Required")]
    [StringLength(220, ErrorMessage = "Max 220 symbols")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description Required")]
    [StringLength(500, ErrorMessage = "Max 500 symbols")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Requirements")]
    [Required(ErrorMessage = "Required")]
    public string Requirements { get; set; } = string.Empty;

    [Display(Name = "Duration")]
    [Required(ErrorMessage = "Required")]
    public string Duration { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category Required")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Language")]
    [Required(ErrorMessage = "Language Required")]
    public string Language { get; set; } = string.Empty;

    [Display(Name = "Price")]
    [Required(ErrorMessage = "Price Required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }
    
    public IFormFile? Image { get; set; }

    public int LastUpdate { get; set; } = 2024;
    public decimal Rating { get; set; } = 4;
    public int RatingCnt { get; set; } = 2544;
    public int Views { get; set; } = 0;
    public int LikesCnt { get; set; } = 0;
    public int DislikesCnt { get; set; } = 0;
    public int SharedCnt { get; set; } = 0;
}