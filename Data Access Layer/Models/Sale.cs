using System;
using SQLite;

namespace UndergradProject.Data_Access_Layer.Models
{
    public class Sale
    {
        // Properties
        [PrimaryKey, AutoIncrement]
        public int saleId { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        public int quantity { get; set; }

        [MaxLength(100)]
        public string category { get; set; }

        public decimal unitPrice { get; set; }

        // Calculated property, not mapped to the database
        [Ignore]
        public decimal totalPrice
        {
            get { return quantity * unitPrice; }
        }

        public string date { get; set; }

        // Parameterless constructor
        public Sale() { }

        // Constructor
        public Sale(string name, int quantity, string category, decimal unitPrice, string date)
        {
            this.name = name;
            this.quantity = quantity;
            this.category = category;
            this.unitPrice = unitPrice;
            this.date = date;
        }
    }
}
