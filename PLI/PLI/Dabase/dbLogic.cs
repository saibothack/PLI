using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLI.Dabase
{
    public class dbLogic
    {
        readonly SQLiteAsyncConnection database;

        public dbLogic(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);

            dropTables();

            database.CreateTableAsync<Models.Shipment>().Wait();
            database.CreateTableAsync<Models.Remission>().Wait();
            database.CreateTableAsync<Models.Product>().Wait();
        }

        public void dropTables()
        {
            database.ExecuteAsync("DELETE FROM Shipment");
            database.ExecuteAsync("DELETE FROM Remission");
            database.ExecuteAsync("DELETE FROM Product");
        }

        public Task<int> SaveShipment(Models.Shipment shipment)
        {
            return database.InsertAsync(shipment);
        }

        public Task<Models.Shipment> GetShipmentActive()
        {
            return database.Table<Models.Shipment>().Where(i => i.status_id != 6).FirstOrDefaultAsync();
        }

        public Task<int> UpdateShipment(Models.Shipment shipment)
        {
            return database.UpdateAsync(shipment);
        }

        public Task<int> SaveRemission(Models.Remission remission)
        {
            return database.InsertAsync(remission);
        }

        public Task<List<Models.Remission>> GetRemissionsByIdShipment(int idShipment)
        {
            return database.Table<Models.Remission>().Where(i => i.shipment_id != idShipment).ToListAsync();
        }

        public Task<int> UpdateRemission(Models.Remission remission)
        {
            return database.UpdateAsync(remission);
        }

        public Task<int> SaveProduct(Models.Product product)
        {
            return database.InsertAsync(product);
        }

        public Task<List<Models.Product>> GetProductsByIdRemission(int idRemission)
        {
            return database.Table<Models.Product>().Where(i => i.remission_id != idRemission).ToListAsync();
        }

        public Task<int> UpdateProduct(Models.Product product)
        {
            return database.UpdateAsync(product);
        }
    }
}
