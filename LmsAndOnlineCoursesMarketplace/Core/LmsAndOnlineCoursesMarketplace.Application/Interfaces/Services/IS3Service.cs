using System.IO;
using System.Threading.Tasks;

namespace LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;

public interface IS3Service
{
    Task<bool> DoesBucketExistAsync();
    Task EnsureBucketExistsAsync();
    Task<string> UploadFileAsync(string keyName, Stream fileStream, string contentType);
    Task<Stream> DownloadFileAsync(string keyName);
    Task DeleteFileAsync(string keyName);
}