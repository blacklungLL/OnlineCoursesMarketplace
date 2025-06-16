namespace LmsAndOnlineCoursesMarketplace.Application.S3;

public class S3Settings
{
    public string BucketName { get; set; } = "my-courses-media";
    public string Region { get; set; } = "us-east-1";
    public string AccessKey { get; set; } = "my-access-key";
    public string SecretKey { get; set; } = "my-secret-key";
    public string Endpoint { get; set; } = "http://localhost:9000";
}