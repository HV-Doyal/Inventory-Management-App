using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndergradProject.Business_Logic_Layer
{
    public class Validation
    {
        public bool validateEmail(string email)
        {
            // Simple email validation logic
            return email.Contains("@") && email.Contains(".");
        }

        public bool validatePassword(string password)
        {
            // Simple password validation logic
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
        }
    }
}
