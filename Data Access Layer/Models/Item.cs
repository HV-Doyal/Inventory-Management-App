using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace UndergradProject.Data_Access_Layer.Models
{
    public class Item
    {
        // Properties
        [PrimaryKey, AutoIncrement]
        public int itemId { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        [MaxLength(100)]
        public string category { get; set; }

        [MaxLength(100)]
        public string barcode { get; set; }

        public int quantity { get; set; }
        public decimal unitPrice { get; set; }

        [MaxLength(10)]
        public string InventoryId { get; set; }  // Reference to the inventory

        // Parameterless constructor
        public Item() { }

        // Constructor with inventory reference
        public Item(string name, string category, string barcode, int quantity, decimal unitPrice, string inventoryId)
        {
            this.name = name;
            this.category = category;
            this.barcode = barcode;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
            this.InventoryId = inventoryId;
        }
    }
}
