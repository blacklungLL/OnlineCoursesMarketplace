using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using LmsAndOnlineCoursesMarketplace.Application.S3;
using Microsoft.Extensions.Options;

namespace LmsAndOnlineCoursesMarketplace.Infrastructure.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _endpoint;

    public S3Service(IOptions<S3Settings> s3Settings)
    {
        var settings = s3Settings.Value;

        _bucketName = settings.BucketName;
        _endpoint = settings.Endpoint;

        var config = new AmazonS3Config
        {
            ServiceURL = settings.Endpoint,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, config);
    }

    public async Task<bool> DoesBucketExistAsync()
    {
        try
        {
            var buckets = await _s3Client.ListBucketsAsync();
            return buckets.Buckets.Any(b => b.BucketName == _bucketName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking the bucket: {ex.Message}");
            return false;
        }
    }
    
    public async Task EnsureBucketExistsAsync()
    {
        try
        {
            var exists = await _s3Client.ListBucketsAsync();
            if (!exists.Buckets.Any(b => b.BucketName == _bucketName))
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucketName
                });

                Console.WriteLine($"Bucket '{_bucketName}' succsessfully added.");
            }
            else
            {
                Console.WriteLine($"Bucket '{_bucketName}' already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking/creating the bucket: {ex.Message}");
        }
    }

    public async Task<string> UploadFileAsync(string keyName, Stream fileStream, string contentType)
    {
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = keyName,
                BucketName = _bucketName,
                ContentType = contentType
            };

            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"{_endpoint}/{_bucketName}/{keyName}";
        }
        catch (Amazon.S3.AmazonS3Exception s3Ex)
        {
            Console.WriteLine($"S3 Error: {s3Ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while uploading the file: {ex.Message}");
            throw;
        }
    }
    

    public async Task<Stream> DownloadFileAsync(string keyName)
    {
        var getObjectResponse = await _s3Client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        });

        using var memoryStream = new MemoryStream();
        await getObjectResponse.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task DeleteFileAsync(string keyName)
    {
        await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        });
    }
}