using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Loto.modules;


namespace Loto.Validations
{

    public class UserValidation
    {
        static Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9_]{3,16}$");
        static Regex PasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");

        public static void ValidateUsernameAtRegister(string username) {
            if (username.Trim().Length == 0) throw new Exception("Username can not be empty!");
            if (!UsernameRegex.IsMatch(username)) throw new Exception("Username must consist of letters (upper and lower case), numbers, and underscores, and be between 3 to 16 characters long");
        }
        public static void ValidatePasswordAtRegister(string password, string confirmedPassword)
        {
            if (password.Trim().Length == 0) throw new Exception("Password can not be empty!");
            if (!PasswordRegex.IsMatch(password)) throw new Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character\r\n");
            if (password != confirmedPassword) throw new Exception("Passwords do not match");
        }
        public static void ValidateUsernameAtLogin(string username)
        {
            if (username.Trim().Length == 0) throw new Exception("Username can not be empty!");
        }
        public static void ValidatePasswordAtLogin(string password)
        {
            if (password.Trim().Length == 0) throw new Exception("Password can not be empty!");
        }
    }

}
