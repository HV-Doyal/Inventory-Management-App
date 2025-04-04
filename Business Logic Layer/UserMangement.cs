using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndergradProject.Data_Access_Layer;

namespace UndergradProject.Business_Logic_Layer
{
    public class UserMangement
    {
        public User createUser(string username, string email, string password)
        {
            User user = new User(username,email, password);
            Console.WriteLine("User created successfully!");
            Console.WriteLine($"UserID: {user.userId}, Username: {user.Username}, Email: {user.Email}, Password: {user.Password}");
            return user;
        }
    }
}
