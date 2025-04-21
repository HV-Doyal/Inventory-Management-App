using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndergradProject.Data_Access_Layer;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Business_Logic_Layer
{
    class ItemMangement
    {
        DatabaseService databaseItemService = new DatabaseService(Constants.itemsDatabasePath);
        Validation validation = new Validation();

        //Initialise the table in the databse
        public async Task initialiseItemTable()
        {
            // Create the table for item
            await databaseItemService.CreateTableAsync<Item>();
        }

        //Add item to table in database
        public async Task addItemToDatabase(Item item)
        {
            await initialiseItemTable();
            await databaseItemService.InsertDataAsync(item);
        }

        public async Task<bool> createItem(string itemName, string category, string barcode, int quantity, decimal unitPrice)
        {
            await initialiseItemTable();
            Item item = new Item(itemName, category, barcode, quantity, unitPrice);
            Console.WriteLine("Item created successfully!");
            Console.WriteLine($"ItemID: {item.itemId}, Name: {item.name}, Category: {item.category}, Barcode: {item.barcode}, Quantity: {item.quantity}, Unit Price: {item.unitPrice}");

            if (!await IsItemPresentInDatabase(item))
            {
                // Add item to the database
                await addItemToDatabase(item);
                Console.WriteLine("Item added to database successfully!");
                return true;
            }
            else
            {
                Console.WriteLine("Item already present in database!");
                return false;
            }
        }

        //returns all items from database
        public async Task<List<Item>> getAllItemsFromDatabase()
        {
            List<Item> items = await databaseItemService.GetDataAsync<Item>();

            foreach (var item in items)
            {
                Console.WriteLine($"ItemID: {item.itemId}, Name: {item.name}, Category: {item.category}, Barcode: {item.barcode}, Quantity: {item.quantity}, Unit Price: {item.unitPrice}");
            }

            return items;
        }

        //Checks is item is present in Databse
        public async Task<bool> IsItemPresentInDatabase(Item item)
        {
            var items = await getAllItemsFromDatabase();
            return items.Any(i =>
                string.Equals(i.name, item.name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(i.barcode, item.barcode, StringComparison.OrdinalIgnoreCase));
        }

        public async Task updataItemDB(Item item)
        {
            await databaseItemService.UpdateDataAsync(item);
        }
    }
}
