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

        // Parameterless constructor
        public Item() { }

        // Constructor without itemId
        public Item(string name, string category, string barcode, int quantity, decimal unitPrice)
        {
            this.name = name;
            this.category = category;
            this.barcode = barcode;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
        }
    }
}
