using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace UndergradProject.Data_Access_Layer
{
    public class User
    {
        // Properties
        [PrimaryKey, AutoIncrement]
        public int userId { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(10)]
        public string InventoryId { get; set; }

        // Parameterless constructor
        public User() { }

        // Constructor with auto InventoryId generation
        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            InventoryId = GenerateInventoryId();
        }

        private string GenerateInventoryId()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 999); // You can enhance this logic
            return $"INV{number.ToString("D3")}";
        }
    }
}