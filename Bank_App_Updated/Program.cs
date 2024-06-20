using BankApp.Classes;
using BankApp.Database;
using BankApp.Methods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            bool loggedIn = false;
            User sessionUser = null;

            while (!loggedIn)
            {
                Console.Clear();
                Console.WriteLine("Welcome to UrLedger Bank");
                Console.WriteLine("=========================");
                Console.WriteLine("Press 1 to Sign Up");
                Console.WriteLine("Press 2 to Login");
                string response = Console.ReadLine();
                Guid userId;

                if (response == "1")
                {

                    UserMethods userMethod = new UserMethods();
                    userId = userMethod.RegisterUser();

                    Console.WriteLine("would you like to open an account now?  Y/N");
                    string choice = Console.ReadLine().ToLower();

                    if ( choice == "y")
                    {
                        AccountMethods.OpenAccount(userId);
                    }
                    else
                    {
                        Console.WriteLine("Thank you for registering consider creating an account");
                        
                    }
                    
                }
                else if (response == "2") 
                {
                    Console.Write("Enter Username(your first name): ");
                    string username = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string password = ReadPassword();

                    if (AuthenticateUser(username, password, out sessionUser))
                    {
                        loggedIn = true;
                        Console.WriteLine("\nLogin Successful!");
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid username or password. Please try again.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect Response");
                }


            }

            while (loggedIn)
            {
                Console.Clear();
                ShowMenu(sessionUser);

                string userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Please input a valid option.");
                }
                //else if (userInput == "1")
                //{
                //    AccountMethods.RegisterAndOpenAccount();
                //}
                else if (userInput == "1")
                {
                    new Transactions().WithdrawMoney(sessionUser);
                    Console.WriteLine("\nPress any key to continue ");
                    Console.ReadKey();
                }
                else if (userInput == "2")
                {
                    new Transactions().DepositMoney(sessionUser);
                    Console.WriteLine("\nPress any key to continue ");
                    Console.ReadKey();
                }
                else if (userInput == "3")
                {
                    AccountMethods.DisplayAccountInfo(sessionUser);
                    Console.WriteLine("\nPress any key to continue ");
                    Console.ReadKey();
                }
                else if (userInput == "4")
                {
                    Console.WriteLine("Enter amount to transfer");
                    string readAmount = Console.ReadLine();
                    float amount = float.Parse(readAmount);
                    new Transactions().Transfer(sessionUser);
                    Console.WriteLine("\nPress any key to continue ");
                    Console.ReadKey();
                }
                else if (userInput == "5")
                {
                    using (var db = new BankApp_DbContext())
                    {
                        List<User> users = db.GetAllEntities<User>();
                        List<Account> accounts = db.GetAllEntities<Account>();

                        Console.WriteLine("What Database would you like to see");
                        string dbType = Console.ReadLine().ToLower();
                        var show = new AccountMethods();

                        if (dbType == "users")
                        {
                            show.showAllDb(users);
                        }
                        else if(dbType == "accounts")
                        {
                            show.showAllDb(accounts);
                        }
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                }
                else if (userInput == "6")
                {
                    new TransactionHistoryMethods().ViewTransactionHistory();
                    Console.WriteLine("\nPress any key to continue ");
                    Console.ReadKey();
                }
                else if (userInput == "7")
                {
                    Console.WriteLine("Thank you for banking with us.");
                    loggedIn = false; // End the session
                }
                else
                {
                    Console.WriteLine("Invalid option, please try again.");
                }
            }
        }


        
        private void ShowMenu(User sessionUser)
        {
            
            Console.WriteLine($"Welcome, {Validations.Capitalize(sessionUser.FirstName)}");
            Console.WriteLine("\nWhat would you like to do today\n");
            //Console.WriteLine("Press 1 to RegisterUser and Open an Account.");
            Console.WriteLine("Press 1 to Withdraw money.");
            Console.WriteLine("Press 2 to Deposit money.");
            Console.WriteLine("Press 3 to Display Account info.");
            Console.WriteLine("Press 4 to transfer.");
            Console.WriteLine("Press 5 to Show All Accounts.");
            Console.WriteLine("Press 6 to Show User Transaction History.");
            Console.WriteLine("Press 7 to Exit.");
        }

        static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return password;
        }

        static bool AuthenticateUser(string username, string password, out User sessionUser)
        {
            using (var db = new BankApp_DbContext())
            {
                sessionUser = db.Users.SingleOrDefault(u => u.FirstName == username && u.Password == password);
                return sessionUser != null;
            }
        }
    }
}

