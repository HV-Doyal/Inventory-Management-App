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
        DatabaseService databaseUserService = new DatabaseService(Constants.usersDatabasePath);
        Validation validation = new Validation();
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
            string hashedPassword = validation.HashPassword(password);

            User user = new User(username, email, hashedPassword);
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

        //Checks if account is valid
        public async Task<bool> isAccountValid(string username, string enteredPassword)
        {
            var users = await getAllUsersFromDatabase();

            // Find the user by username
            var existingUser = users.FirstOrDefault(u => string.Equals(u.Email, username, StringComparison.OrdinalIgnoreCase));

            if (existingUser != null)
            {
                // Verify the entered password against the stored hashed password
                bool isPasswordValid = validation.VerifyPassword(enteredPassword, existingUser.Password);
                Preferences.Set("username", existingUser.Username);
                return isPasswordValid;
            }

            // Return false if no user is found with the provided username
            return false;
        }

        //print users table 
        public async void printUsersTable()
        {
            List<User> users = await databaseUserService.GetDataAsync<User>();

            foreach (var u in users)
            {
                Console.WriteLine($"UserID: {u.userId}, Username: {u.Username}, Email: {u.Email}, Password: {u.Password}");
            }
        }
    }
}