using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace UndergradProject.Business_Logic_Layer
{
    public class Validation
    {
        public bool isEmailValid(string email)
        {
            // Simple email validation logic
            return email.Contains("@") && email.Contains(".");
        }

        public bool isPasswordValid(string password)
        {
            // Simple password validation logic
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
        }

        public bool isNull(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        // Function to hash the password using SHA256
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the password string to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash
                byte[] hashedBytes = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Function to verify if a password matches the stored hash
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Hash the entered password and compare with stored hash
            string hashedEnteredPassword = HashPassword(enteredPassword);
            return hashedEnteredPassword.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
