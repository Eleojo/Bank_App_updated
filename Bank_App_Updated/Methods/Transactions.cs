using BankApp.Classes;
using BankApp.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BankApp.Methods
{
    public class Transactions
    {
       
       // public float accountBalance;
        public void WithdrawMoney(User sessionUser)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            //List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();

            //Console.WriteLine("Enter your account number:");
            //string accNumber = Console.ReadLine();
            

            // Find the user with the specified account number
            Account foundAccount = accounts.FirstOrDefault(account => account.userId.Equals(sessionUser.Id));
           
            // Prompt for the amount to withdraw
            Console.WriteLine("Enter amount to withdraw:");
            if (float.TryParse(Console.ReadLine(), out float withdrawAmount))
            {

                // Check if the account was found
                if (foundAccount != null)
                {
                    // Check if there are sufficient funds in the account
                    if (withdrawAmount > foundAccount.accountBalance)
                    {
                        Console.WriteLine("Insufficient Funds");
                    }
                    else
                    {
                        // Perform the withdrawal
                        foundAccount.accountBalance -= withdrawAmount;
                        db.UpdateEntities(accounts); // Update database
                        TransactionHistory RecordedTransaction = new TransactionHistory(foundAccount.userId, "Debit", withdrawAmount);
                        db.AddEntity(RecordedTransaction);
                        Console.WriteLine($"Success !! You have withdrawn {withdrawAmount} and your new balance is {foundAccount.accountBalance} ");
                        AccountMethods.DisplayAccountInfo(sessionUser);
                        Console.WriteLine("Press Enter key to return to main menu");
                    }
                }
                else
                {
                    Console.WriteLine("Sorry, you do not have an account with us.");
                }
            }
            else
            {
                Console.WriteLine("Invalid response entered.");
            }
            
        }

        public void DepositMoney(User sessionUser)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            //List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();
           
            Console.WriteLine("Enter amount to deposit:");
            if (int.TryParse(Console.ReadLine(), out int depositAmount))
            {

                // Find the account associated with the user
                Account foundAcount = accounts.FirstOrDefault(account => account.userId.Equals(sessionUser.Id));
                // Check if the account was found
                
                if (foundAcount != null)
                {
                    // Perform the deposit
                    //foundAccount.accountBa += depositAmount;
                    foundAcount.accountBalance += depositAmount;
                    db.UpdateEntities(accounts); // Update database
                    TransactionHistory RecordedTransaction = new TransactionHistory(foundAcount.Id, "Credit", depositAmount);
                    db.AddEntity(RecordedTransaction);
                    Console.WriteLine($"Success!! You have deposited {depositAmount} and your new balance is {foundAcount.accountBalance}.");
                    AccountMethods.DisplayAccountInfo(sessionUser);
                    Console.WriteLine("Press Enter key to return to main menu");
                }             
                else
                {
                    Console.WriteLine("Sorry, you do not have an account with us.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount entered.");
            }
            
           
        }


        //public void Transfer(float amount)
        //{
        //    BankApp_DbContext db = new BankApp_DbContext();
        //    List<User> users = db.GetAllEntities<User>();
        //    List<Account> accounts = db.GetAllEntities<Account>();
        //    Guid senderUserId = Guid.Empty;
        //    Guid receiverUserId = Guid.Empty;

        //    Console.WriteLine("Enter sender accNumber (first name):");
        //    string senderIdentifier = Console.ReadLine();
        //    Console.WriteLine("Enter receiver accNumber (first name):");
        //    string receiverIdentifier = Console.ReadLine();

        //    foreach (User user in users)
        //    {
        //        Console.WriteLine($"User: {user.FirstName}, ID: {user.Id}");
        //    }

        //    // Find sender and receiver user IDs (case-insensitive comparison)
        //    foreach (User user in users)
        //    {
        //        if (user.FirstName.Equals(senderIdentifier, StringComparison.OrdinalIgnoreCase))
        //        {
        //            senderUserId = user.Id;
        //        }
        //        if (user.FirstName.Equals(receiverIdentifier, StringComparison.OrdinalIgnoreCase))
        //        {
        //            receiverUserId = user.Id;
        //        }
        //    }

        //    // Check if both users were found
        //    if (senderUserId == Guid.Empty || receiverUserId == Guid.Empty)
        //    {
        //        Console.WriteLine($"One or both users not found. Please make sure both sender and receiver are registered. Sender ID: {senderUserId}, Receiver ID: {receiverUserId}");
        //        return;
        //    }

        //    Account senderAccount = null;
        //    Account receiverAccount = null;

        //    // Find sender and receiver accounts
        //    foreach (Account account in accounts)
        //    {
        //        if (account.userId.Equals(senderUserId))
        //        {
        //            senderAccount = account;
        //        }
        //        if (account.userId.Equals(receiverUserId))
        //        {
        //            receiverAccount = account;
        //        }
        //    }

        //    // Check if both accounts were found
        //    if (senderAccount == null || receiverAccount == null)
        //    {
        //        Console.WriteLine("One or both accounts not found. Please make sure both sender and receiver have accounts.");
        //        return;
        //    }

        //    // Check if sender has sufficient funds
        //    if (senderAccount.accountBalance < amount)
        //    {
        //        Console.WriteLine("Insufficient funds in the sender's account.");
        //        return;
        //    }

        //    // Perform the transfer
        //    senderAccount.accountBalance -= amount;
        //    receiverAccount.accountBalance += amount;

        //    // update changes to database
        //    db.UpdateEntities(accounts);

        //    TransactionHistory senderTransaction = new TransactionHistory(senderUserId, "Transfer Out", -amount);
        //    TransactionHistory receiverTransaction = new TransactionHistory(receiverUserId, "Transfer In", amount);
        //    db.AddEntity(senderTransaction);
        //    db.AddEntity(receiverTransaction);

        //    Console.WriteLine($"Transfer successful! {amount} has been transferred from {senderIdentifier} to {receiverIdentifier}.");
        //    Console.WriteLine($"Sender's new balance: {senderAccount.accountBalance}");
        //    Console.WriteLine($"Receiver's new balance: {receiverAccount.accountBalance}");
        //}

        public void Transfer(User sessionUser)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();


            Console.WriteLine("Enter amount to Transfer: ");
            string amountString = Console.ReadLine();
            float amount = float.Parse(amountString);
            //string senderIdentifier = Console.ReadLine();

            Console.WriteLine("Enter receiver accNumber:");
            string receiverAccountNumber = Console.ReadLine();

            // Find sender and receiver user IDs using LINQ
            //var account = accounts.SingleOrDefault(a => a.userId == sessionUser.Id);
            Account senderAccount = accounts.SingleOrDefault(account => account.userId.Equals(sessionUser.Id));
            Account receiverAccount = accounts.SingleOrDefault(account => account.accountNumber.Equals(receiverAccountNumber));
            User receiver = users.SingleOrDefault(user => user.Id.Equals(receiverAccount.userId));

            // Check if both users were found
            if (senderAccount == null || receiverAccount == null)
            {
                Console.WriteLine($"One or both users not found. Please make sure both sender and receiver are registered.");
                return;
            }

            // Find sender and receiver accounts using LINQ
            //var senderAccount = accounts.SingleOrDefault(account => account.userId.Equals(senderUser.Id));
            //var receiverAccount = accounts.SingleOrDefault(account => account.userId.Equals(receiverUser.Id));

            // Check if sender has sufficient funds

            
            if (senderAccount.accountBalance < amount)
            {
                Console.WriteLine("Insufficient funds in the sender's account.");
                return;
            }

            // Perform the transfer
            senderAccount.accountBalance -= amount;
            receiverAccount.accountBalance += amount;

            // Update changes to database
            db.UpdateEntities(accounts);

            TransactionHistory senderTransaction = new TransactionHistory(senderAccount.Id, "Transfer Out", -amount);
            TransactionHistory receiverTransaction = new TransactionHistory(receiverAccount.Id, "Transfer In", amount);
            db.AddEntity(senderTransaction);
            db.AddEntity(receiverTransaction);
            AccountMethods.DisplayAccountInfo(sessionUser);

            Console.WriteLine($"Transfer successful! {amount} has been transferred to {receiver.FirstName}.");
            Console.WriteLine($"Sender's new balance: {senderAccount.accountBalance}");
            Console.WriteLine($"Receiver's new balance: {receiverAccount.accountBalance}");
        }

    }
}
