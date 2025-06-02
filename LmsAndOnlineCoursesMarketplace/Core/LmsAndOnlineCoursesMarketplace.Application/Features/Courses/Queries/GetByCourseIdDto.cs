namespace LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;

public class GetByCourseIdDto
{
    public int Id { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Краткое описание курса
    /// </summary>
    public string ShortDescription { get; set; }
    
    /// <summary>
    /// Id автора курса
    /// </summary>
    public int UserId { get; set; }
    
    
    /// <summary>
    /// Ссылка
    /// </summary>
    public string ImageLink { get; set; }
    
    /// <summary>
    /// Оценка курса
    /// </summary>
    public decimal Rating { get; set; }
    
    /// <summary>
    /// Количество оценок 
    /// </summary>
    public int RatingsCnt { get; set; }
    
    /// <summary>
    /// Язык курса
    /// </summary>
    public string Language { get; set; }
    
    /// <summary>
    /// Последнее обновление курса
    /// </summary>
    public int LastUpdate { get; set; }
    
    /// <summary>
    /// Кол-во просмотров
    /// </summary>
    public int Views { get; set; }
    
    /// <summary>
    /// Кол-во лайков
    /// </summary>
    public int LikesCnt { get; set; }
    
    /// <summary>
    /// Кол-во дизлайков
    /// </summary>
    public int DislikesCnt { get; set; }
    
    /// <summary>
    /// Кол-во репостов
    /// </summary>
    public int SharedCnt { get; set; }
    
    /// <summary>
    /// Требования
    /// </summary>
    public string Requirements { get; set; }
    
    /// <summary>
    /// Описания
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Время курса
    /// </summary>
    public string Duration { get; set; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public string Category { get; set; }
    
    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; set; }
}