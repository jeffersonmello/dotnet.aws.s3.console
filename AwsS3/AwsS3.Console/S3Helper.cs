using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace AwsS3.Console;

public class S3Helper
{
    private string _access_key;
    private string _secrete_key;
    private string _bucket_name;

    private RegionEndpoint _regionEndpoint;
    private S3CannedACL _acl;

    private AmazonS3Client _client;

    public S3Helper(
        string accessKey, 
        string secreteKey, 
        string bucketName,
        RegionEndpoint regionEndpoint,
        S3CannedACL acl)
    {
        _access_key = accessKey;
        _secrete_key = secreteKey;
        _bucket_name = bucketName;

        _regionEndpoint = regionEndpoint;
        _acl = acl;

        _client = new AmazonS3Client(_access_key, _secrete_key, _regionEndpoint);
    }

    public async Task UploadTestFile()
    {
        try
        {
            await using var stream = new MemoryStream();
            await File.AppendAllTextAsync("teste.txt", "teste");

            var file = File.Open(Directory.GetCurrentDirectory() + "//teste.txt", FileMode.Open);
        
            await file.CopyToAsync(stream);
        
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = "teste.txt",
                BucketName = _bucket_name,
                CannedACL = _acl
            };

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task UploadTestFile(RegionEndpoint regionEndpoint, S3CannedACL acl)
    {
        try
        {
            using var client = new AmazonS3Client(_access_key, _secrete_key, regionEndpoint); 
            await using var stream = new MemoryStream();
            await File.AppendAllTextAsync("teste.txt", "teste");

            var file = File.Open(Directory.GetCurrentDirectory() + "//teste.txt", FileMode.Open);
        
            await file.CopyToAsync(stream);
        
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = "teste.txt",
                BucketName = _bucket_name,
                CannedACL = acl
            };

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            throw;
        }
    }
    
    public string GeneratePreSignedURL(double minutes, string objectKey)
    {
        var urlString = "";
        
        try
        {
            var request1 = new GetPreSignedUrlRequest
            {
                BucketName = _bucket_name,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            };
            
            urlString = _client.GetPreSignedURL(request1);
        }
        catch (AmazonS3Exception e)
        {
            System.Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        }
        
        return urlString;
    }
}