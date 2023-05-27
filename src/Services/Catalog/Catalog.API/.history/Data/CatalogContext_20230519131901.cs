using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configurationContext)
        {
            var client = new MongoClient(configurationContext.GetValue<string>("DatabaseSettings.ConnectionString"));
            var database = client.GetDatabase(configurationContext.GetValue<string>("DatabaseSettings.DatabaseName"));

            Products = database.GetCollection<Product>(configurationContext.GetValue<string>("DatabaseSettings.CollectionName"));
        }
        public IMongoCollection<Product> Products => throw new NotImplementedException();
    }
}