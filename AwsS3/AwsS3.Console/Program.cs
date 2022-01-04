using Amazon;
using Amazon.S3;
using AwsS3.Console;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);


Console.WriteLine("Enviando Arquivo para o S3!");
var config = builder.Build();

var s3 = new S3Helper(
    config["access_key"], 
    config["secret_key"], 
    config["bucket_name"], 
    RegionEndpoint.USEast1, 
    S3CannedACL.Private);


var url = s3.GeneratePreSignedURL(1.0, "teste.txt");
Console.WriteLine(url);

// await s3.UploadTestFile();