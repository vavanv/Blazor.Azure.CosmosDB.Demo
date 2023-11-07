using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Blazor.Azure.CosmosDB.Demo.Data
{
    public class EngineerService
    {
        private readonly string CosmosDbConnectionString = "AccountEndpoint=https://azure-dev-vvv-cosmos-db.documents.azure.com:443/;AccountKey=YLfiOx57pZMim9nxj7UvfAZEPLX8wJEjNFAggXOh86FWiW7yEgedfBQ5TETmeFKzAMovL2o4hOOxACDbB9O7XA==;";
        private readonly string CosmosDbName = "Contractors";
        private readonly string CosmosDbContainerName = "Engineers";
        public EngineerService() { }

        private Container GetContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDbConnectionString);
            var container = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return container;
        }

        public async Task AddEngineer(Engineer engineer)
        {
            try
            {
                engineer.id = Guid.NewGuid();
                var container = GetContainerClient();
                var response = await container.CreateItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                ex.Message.ToString().Trim();
            }
        }

        public async Task DeleteEngineer(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
            }
            catch (Exception ex)
            {
                throw new Exception("Delete", ex);
            }
        }
    }

}

///
