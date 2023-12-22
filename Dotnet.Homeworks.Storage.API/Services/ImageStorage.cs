using System.Reactive.Linq;
using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Dotnet.Homeworks.Storage.API.Services;

public class ImageStorage : IStorage<Image>
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public ImageStorage(IMinioClient minioClient, string bucketName)
    {
        _minioClient = minioClient;
        _bucketName = bucketName;
    }

    public async Task<Result> PutItemAsync(Image item, CancellationToken cancellationToken = default)
    {
        var getItem = await GetItemAsync(item.FileName, cancellationToken);
        if (getItem is not null)
            return $"Item with name {item.FileName} already exists";

        item.Metadata[MetadataKeys.Destination] = _bucketName;

        var args = new PutObjectArgs()
            .WithBucket(Buckets.Pending)
            .WithStreamData(item.Content)
            .WithObject(item.FileName)
            .WithObjectSize(item.Content.Length)
            .WithContentType(item.ContentType)
            .WithHeaders(item.Metadata);

        var response = await _minioClient.PutObjectAsync(args, cancellationToken);
        return response is not null
            ? true
            : $"Can't put image {item.FileName} to bucket {Buckets.Pending}";
    }

    public async Task<Image?> GetItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var ms = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(itemName)
            .WithCallbackStream(stream => stream.CopyToAsync(ms, cancellationToken));

        try
        {
            var response = await _minioClient.GetObjectAsync(args, cancellationToken);
            ms.Position = 0;

            return new Image(ms, response.ObjectName, response.ContentType, response.MetaData);
        }
        catch (MinioException)
        {
            return null;
        }
    }

    public async Task<Result> RemoveItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(itemName);

        try
        {
            await _minioClient.RemoveObjectAsync(args, cancellationToken);

            return true;
        }
        catch (MinioException e)
        {
            return e.Message;
        }
    }

    public async Task<IEnumerable<string>> EnumerateItemNamesAsync(CancellationToken cancellationToken = default)
    {
        var args = new ListObjectsArgs()
            .WithBucket(_bucketName);

        return await _minioClient.ListObjectsAsync(args, cancellationToken)
            .Select(x => x.Key)
            .ToArray();
    }

    public async Task<Result> CopyItemToBucketAsync(string itemName, string destinationBucketName,
        CancellationToken cancellationToken = default)
    {
        var args = new CopyObjectArgs()
            .WithBucket(destinationBucketName)
            .WithObject(itemName)
            .WithCopyObjectSource(new CopySourceObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(itemName));

        try
        {
            await _minioClient.CopyObjectAsync(args, cancellationToken);

            return true;
        }
        catch (MinioException e)
        {
            return e.Message;
        }
    }
}