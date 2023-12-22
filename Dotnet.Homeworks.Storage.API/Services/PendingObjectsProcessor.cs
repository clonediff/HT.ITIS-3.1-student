using Dotnet.Homeworks.Storage.API.Constants;
using Minio.DataModel.Args;

namespace Dotnet.Homeworks.Storage.API.Services;

public class PendingObjectsProcessor : BackgroundService
{
    private readonly IStorageFactory _storageFactory;

    public PendingObjectsProcessor(IStorageFactory storageFactory)
    {
        _storageFactory = storageFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var storage = await _storageFactory.CreateImageStorageWithinBucketAsync(Buckets.Pending);
        while (!stoppingToken.IsCancellationRequested)
        {
            var pendingItems = await storage.EnumerateItemNamesAsync(stoppingToken);
            foreach (var itemName in pendingItems)
            {
                var item = await storage.GetItemAsync(itemName, stoppingToken);
                if (item is null)
                    continue;

                var copyResponse = await storage.CopyItemToBucketAsync(itemName, item.Metadata[MetadataKeys.Destination], stoppingToken);
                if (copyResponse.IsFailure)
                    Console.WriteLine(copyResponse.Error);
                
                var removeResponse = await storage.RemoveItemAsync(itemName, stoppingToken);
                if (removeResponse.IsFailure)
                    Console.WriteLine(removeResponse.Error);
            }
            
            await Task.Delay(PendingObjectProcessor.Period, stoppingToken);
        }
    }
}