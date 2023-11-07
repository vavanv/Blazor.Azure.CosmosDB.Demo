// Ignore Spelling: Upsert

using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Blazor.Azure.CosmosDB.Demo.Data
{
    public class EngineerService : IEngineerService
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

        public async Task UpsertEngineer(Engineer engineer)
        {
            try
            {
                if (engineer.id == null)
                {
                    engineer.id = Guid.NewGuid();
                }
                var container = GetContainerClient();
                var response = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception("UpsertEngineer", ex);
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


        public async Task<List<Engineer>> GetEngineerDetails()
        {
            List<Engineer> engineers = new List<Engineer>();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Engineer engineer in currentResultSet)
                    {
                        engineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetEngineerDetails", ex);
            }
            return engineers;
        }

        public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {
                throw new Exception("GetEngineerDetailsById", ex);
            }
        }
    }

}

///
