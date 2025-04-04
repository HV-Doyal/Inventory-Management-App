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
        DatabaseService databaseUserService = new DatabaseService(Constants.databasePath);

        //Initialise the table in the databse
        public async Task initialiseUsersTable()
        {
            // Create the table for User
            await databaseUserService.CreateTableAsync<User>();
        }

        //Add user to table in database
        public async Task addUserToDatabase(User user)
        {
            await initialiseUsersTable();
            await databaseUserService.InsertDataAsync(user);
        }

        public async Task<bool> createUser(string username, string email, string password)
        {
            User user = new User(username, email, password);
            Console.WriteLine("User created successfully!");
            Console.WriteLine($"UserID: {user.userId}, Username: {user.Username}, Email: {user.Email}, Password: {user.Password}");
           
            if (!await IsUserPresentInDatabase(user))
            {
                // Add user to the database
                await addUserToDatabase(user);
                Console.WriteLine("User added to database successfully!");
                return true;
            }
            else
            {
                Console.WriteLine("User already present in database!");
                return false;
            }
        }

        //returns all users from database
        public async Task<List<User>> getAllUsersFromDatabase()
        {
            List<User> users = await databaseUserService.GetDataAsync<User>();

            foreach (var u in users)
            {
                Console.WriteLine($"UserID: {u.userId}, Username: {u.Username}, Email: {u.Email}, Password: {u.Password}");
            }

            return users;
        }

        //Checks is user is present in Databse
        public async Task<bool> IsUserPresentInDatabase(User user)
        {
            var users = await getAllUsersFromDatabase();
            return users.Any(u =>
                string.Equals(u.Email, user.Email, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase));
        }
    }
}