using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Classes;
using BankApp.Database;

namespace BankApp.Methods
{
    public  class UserMethods
    {
        
        public Guid RegisterUser()
        {
            var id = Guid.NewGuid();
            //var validEmail = new Email();
            string firstName = GetValidInput("Enter your FirstName:", Validations.IsValidName, "Name cannot be empty or contain digits. Please try again.");
            string lastName = GetValidInput("Enter your LastName:", Validations.IsValidName, "Name cannot be empty or contain digits. Please try again.");
            string email = GetValidInput("Enter your email:", Validations.IsValidEmail, "Incorrect email address. Please try again.");
            string password = GetValidInput("Enter your password:", Validations.IsValidPassword, "Password must contain at least 6 characters, one uppercase letter, one digit, and one special character. Please try again.");


            User newUser = new User(id, firstName, lastName, email, password);

            // BankDataBase_liteDb.UserDatabase.Add(newUser);

            BankApp_DbContext db = new BankApp_DbContext();

            db.AddEntity(newUser);
            Console.WriteLine("User Registered Successfully");
            return id;
        }

        private static string GetValidInput(string prompt, Func<string, bool> validationFunc, string errorMessage)
        {
            string input;
            bool isValid;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                isValid = validationFunc(input);
                if (!isValid)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!isValid);

            return input;
        }
    }
}

