using BankApp.Classes;
using BankApp.Database;
using ConsoleTableExt;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Methods
{
    internal class AccountMethods
    {

        public void UpdateMyAccountProfile()
        {

        }


        //public static void RegisterAndOpenAccount()
        //{
        //    BankApp_DbContext db = new BankApp_DbContext();

        //    string email;
        //    string firstName;
        //    string lastName;
        //    string password;

        //    bool validEmail;
        //    bool validName;
        //    bool validPassword;

        //    // Prompt for and validate first name
        //    do
        //    {
        //        Console.WriteLine("Enter your FirstName:");
        //        firstName = Console.ReadLine();
        //        validName = Validations.IsValidName(firstName);
        //        if (!validName)
        //        {
        //            Console.WriteLine("Name cannot be empty or contain digits. Please try again.");
        //        }
        //    } while (!validName);

        //    // Prompt for and validate last name
        //    do
        //    {
        //        Console.WriteLine("Enter your LastName:");
        //        lastName = Console.ReadLine();
        //        validName = Validations.IsValidName(lastName);
        //        if (!validName)
        //        {
        //            Console.WriteLine("Name cannot be empty or contain digits. Please try again.");
        //        }
        //    } while (!validName);

        //    // Prompt for and validate email
        //    do
        //    {
        //        Console.WriteLine("Enter your email:");
        //        email = Console.ReadLine();
        //        validEmail = Validations.IsValidEmail(email);
        //        if (!validEmail)
        //        {
        //            Console.WriteLine("Incorrect email address. Please try again.");
        //        }
        //    } while (!validEmail);

        //    // Prompt for and validate password
        //    do
        //    {
        //        Console.WriteLine("Enter your password:");
        //        password = Console.ReadLine();
        //        validPassword = Validations.IsValidPassword(password);
        //        if (!validPassword)
        //        {
        //            Console.WriteLine("Password must contain at least 6 characters, one uppercase letter, one digit, and one special character. Please try again.");
        //        }
        //    } while (!validPassword);

        //    // RegisterUser the user
        //    UserMethods userMethods = new UserMethods();
        //    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //    firstName = textInfo.ToTitleCase(firstName);
        //    lastName = textInfo.ToTitleCase(lastName);
        //    userMethods.RegisterUser(firstName, lastName, email, password);

        //    // Retrieve the newly registered user
        //    List<User> users = db.GetAllEntities<User>();
        //    Guid userId = users[^1].Id; // Takes the last recently registered userId

        //    // Prompt for account type
        //    Console.WriteLine("Enter the account type you want to open:");
        //    string accountType = Console.ReadLine();

        //    // Open the account for the newly registered user
        //    OpenAccount(userId, accountType);
        //}

        //public static void OpenAccount()
        //{
        //    BankApp_DbContext db = new BankApp_DbContext();
        //    List<User> users = db.GetAllEntities<User>();
        //    List<Account> accounts = db.GetAllEntities<Account>();

        //    Console.WriteLine("Enter Account Type you want to open: ");
        //    string accountType =Console.ReadLine();

        //    Guid userId = users.Last().Id;
        //    string accountNumber = GenerateRandomAccountNumber();  // Generate the 10-digit account number
        //    float initialAccountBalance = 10;
        //    bool userFound = false;

        //    foreach (User user in users)
        //    {
        //        if (user.Id.Equals(userId))
        //        {
        //            userFound = true;

        //            // Check if the user already has an account
        //            foreach (Account account in accounts)
        //            {
        //                if (account.userId.Equals(userId))
        //                {
        //                    Console.WriteLine("You already have an account with us.");
        //                    return;
        //                }
        //            }

        //            // If no account exists, create a new one
        //            Account newAccount = new Account(userId, initialAccountBalance, accountNumber, accountType);
        //            db.AddEntity(newAccount);
        //            Console.WriteLine($"\nAccount created successfully for user {user.FirstName} with account number {accountNumber}.");
        //            break;
        //        }
        //    }

        //    if (!userFound)
        //    {
        //        Console.WriteLine("Please register first before trying to create an account.");
        //    }
        //}


    public static void OpenAccount(Guid userId)
    {
        BankApp_DbContext db = new BankApp_DbContext();
        List<User> users = db.GetAllEntities<User>();
        List<Account> accounts = db.GetAllEntities<Account>();

        Console.WriteLine("Enter Account Type you want to open: ");
        string accountType = Console.ReadLine();

        // Assume the last user in the list is the one to open the account for
        //Guid userId = users.Last().Id;
        string accountNumber = GenerateRandomAccountNumber();  // Generate the 10-digit account number
        float initialAccountBalance = 10;

        // Check if the user already has an account
        var user = users.SingleOrDefault(u => u.Id == userId);
        if (user != null)
        {
            var existingAccount = accounts.SingleOrDefault(a => a.userId == userId);
            if (existingAccount != null)
            {
                Console.WriteLine("You already have an account with us.");
                return;
            }

            // If no account exists, create a new one
            Account newAccount = new Account(userId, initialAccountBalance, accountNumber, accountType);
            db.AddEntity(newAccount);
            Console.WriteLine($"\nAccount created successfully for user {user.FirstName} with account number {accountNumber}.");
            Console.WriteLine("\nPress any key to continue after copying your account number...");
            Console.ReadKey();
            }
        else
        {
            Console.WriteLine("User not found.");
        }
    }




private static string GenerateRandomAccountNumber()
        {
            Random random = new Random();
            // Generate two 5-digit numbers and concatenate them
            int part1 = random.Next(10000, 100000); // Generates a 5-digit number
            int part2 = random.Next(10000, 100000); // Generates another 5-digit number
            return part1.ToString() + part2.ToString(); // Concatenate them to form a 10-digit number
        }





        public static void DisplayAccountInfo(User sessionUser)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<Account> accounts = db.GetAllEntities<Account>();
            List<User> users = db.GetAllEntities<User>();

            //Console.WriteLine("Enter account identifier (first name for now):");
            //string identifier = Console.ReadLine();

            //TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //identifier = textInfo.ToTitleCase(identifier);

            // Use LINQ to filter users and join with accounts
            var accountDisplays = users
                .Where(user => user.FirstName.Equals(sessionUser.FirstName))
                .Join(accounts,
                      user => user.Id,
                      account => account.userId,
                      (user, account) => new AccountDisplay
                      {
                          FullName = $"{user.FirstName} {user.LastName}",
                          AccountNumber = account.accountNumber,
                          AccountType = account.accountType,
                          AccountBalance = account.accountBalance,
                          UserId = account.userId
                      })
                .ToList();

            if (accountDisplays.Any())
            {
                ConsoleTableBuilder
                    .From(accountDisplays)
                    .WithFormat(ConsoleTableBuilderFormat.Alternative)
                    .ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("User not found in database");
            }
        }



        public void showAllDb<T>(List<T> obj) where T : class
        {
            ConsoleTableBuilder
                .From(obj)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }

        private class AccountDisplay
        {
            public string FullName { get; set; }
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public float AccountBalance { get; set; }
            public Guid UserId { get; set; }
        }

    }
}
