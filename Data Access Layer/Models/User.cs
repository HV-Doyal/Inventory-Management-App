using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndergradProject.Data_Access_Layer
{
    class User
    {
        // Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Constructor
        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
