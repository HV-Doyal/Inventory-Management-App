using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Business_Logic_Layer
{

    class SaleManagement
    {
        DatabaseService databaseSaleService = new DatabaseService(Constants.salesDatabasePath);
        public async Task initialiseSaleTable()
        {
            // Create the table for sale
            await databaseSaleService.CreateTableAsync<Sale>();
        }

        //Add sale to table in database
        public async Task addSaleToDatabase(Sale sale)
        {
            await initialiseSaleTable();
            await databaseSaleService.InsertDataAsync(sale);
        }

    }
}
