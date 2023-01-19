using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    internal class Program
    {
       public static string IdGenerator(string name)
        {
            DateTime date = DateTime.Today;
            string currentDate = date.ToString();
            string tempId;
            tempId = name.Substring(0, 3) + currentDate.Substring(0, 10);
            return tempId;
        }
       public static Dictionary<string, Banks> AllBanks = new Dictionary<string, Banks>();
       public static Dictionary<string, ArrayList> SpecialInfo = new Dictionary<string,ArrayList>();
       public static Dictionary<string, string> SpecialTransactions = new Dictionary<string, string>();
        static void Main(string[] args)
        {

            string bankName, bankId;
           
            SetupBank();
            while (true)
            {

                Console.WriteLine("Please enter the bank id you wish to work on");
                bankId = Console.ReadLine();
                if (AllBanks.ContainsKey(bankId))
                {
                    Console.WriteLine($"\t\t\tWelcome to {bankId}");
                    Console.WriteLine("Please select wether you are a account holder or bank staff \n enter 1 if you are staff \n enter 2 if you are account holder");
                    Console.WriteLine("\t\t1.Bank Staff");
                    Console.WriteLine("\t\t2.Account holder");
                    int option = (int)Convert.ToInt64((Console.ReadLine()));
                    if (option == 1)
                    {
                        ShowStaffMenu();
                    }
                    else
                    {
                        Console.WriteLine("Enter user-id");
                        string user_id = Console.ReadLine();
                        var current_bank = AllBanks[bankId];
                        if (current_bank.AccountCredentials.ContainsKey(user_id))
                        {
                            Console.WriteLine("Enter password");
                            string password = Console.ReadLine();
                            if (current_bank.AccountCredentials[user_id] == password)
                            {
                                showUserMenu(user_id);
                            }
                            else
                            {
                                Console.WriteLine("Wrong Password");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{user_id} does not exist");
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Bank does not exist\n if you would like to setup a new bank please enter yes");
                    string acknowledgement = Console.ReadLine();
                    if (acknowledgement == "yes")
                    {
                        SetupBank();
                    }
                    else
                    {
                        Console.WriteLine("Thank you\n");
                    }
                }
            }

            void SetupBank()
            {
                Console.WriteLine("Please enter the bank name");
                bankName = Console.ReadLine();
                Banks obj = new Banks();
                string temp_bankId = IdGenerator(bankName);
                if (AllBanks.ContainsKey(temp_bankId))
                { Console.WriteLine($"{bankName} already exists"); }
                else
                {
                    obj.bankID = temp_bankId;
                    AllBanks[temp_bankId] = obj;
                    Console.WriteLine("your bankID is\t:\t" + temp_bankId + "\n");
                }
            }  
            void RevertTransaction()
            {
                Console.WriteLine("Please enter transaction id\n Please enter help if you can't remember transaction id");
                string txn_id=Console.ReadLine();
                if(txn_id== "help")
                {
                    foreach(string i in SpecialTransactions.Keys)
                    {
                        Console.WriteLine(i);
                    }
                    Console.WriteLine("Please enter the transaction id from above mentioned");
                    string new_txn_id = Console.ReadLine();
                    Console.WriteLine("You have chosen to revert the following transaction");
                    Console.WriteLine(SpecialTransactions[new_txn_id]);
                    string send_bankid = Convert.ToString(SpecialInfo[new_txn_id][0]);
                    string re_bankid =  Convert.ToString(SpecialInfo[new_txn_id][1]);
                    string send_acid = Convert.ToString(SpecialInfo[new_txn_id][2]);
                    string re_acid = Convert.ToString(SpecialInfo[new_txn_id][3]);
                    double amnt = Convert.ToDouble(SpecialInfo[new_txn_id][4]);
                    double charges = Convert.ToDouble(SpecialInfo[new_txn_id][5]);
                    Banks.NowRevertTransaction(send_bankid,re_bankid,send_acid,re_acid,amnt,charges);
                }
                else
                {
                    string new_txn_id = Console.ReadLine();
                    Console.WriteLine("You have chosen to revert the following transaction");
                    Console.WriteLine(SpecialTransactions[new_txn_id]);
                    string send_bankid = Convert.ToString(SpecialInfo[new_txn_id][0]);
                    string re_bankid = Convert.ToString(SpecialInfo[new_txn_id][1]);
                    string send_acid = Convert.ToString(SpecialInfo[new_txn_id][2]);
                    string re_acid = Convert.ToString(SpecialInfo[new_txn_id][3]);
                    double amnt = Convert.ToDouble(SpecialInfo[new_txn_id][4]);
                    double charges = Convert.ToDouble(SpecialInfo[new_txn_id][5]);
                    Banks.NowRevertTransaction(send_bankid, re_bankid, send_acid, re_acid, amnt, charges);
                }

            }
            void ShowStaffMenu()
            {
                
                Console.WriteLine("Please select the provided optons");
                Console.WriteLine("\t\ta.create a new user account");
                Console.WriteLine("\t\tb.delete a user account");
                Console.WriteLine("\t\td.Add service charge for same bank account");
                Console.WriteLine("\t\te.Add service cahrge for different bank account");
                Console.WriteLine("\t\tf.view transaction history of a user");
                Console.WriteLine("\t\tg.revert a transaction");
                Console.WriteLine("\t\th.show all accounts");
                char option = Convert.ToChar(Console.ReadLine());
                var current_bank = AllBanks[bankId];
                if (option == 'a')
                {
                    current_bank.CreateAccount();
                }
                else if (option == 'b')
                {
                    current_bank.DeleteAccount();
                }
                else if (option == 'd')
                {
                    current_bank.ChangeCharges("samebank");
                }
                else if (option == 'e')
                {
                    current_bank.ChangeCharges("otherbank");
                }
                else if (option == 'f')
                {
                    current_bank.TransactionHistory();
                }
                else if (option == 'g')
                {
                    RevertTransaction();
                }
                else if (option == 'h')
                {
                    current_bank.ShowAccounts();
                }
                else
                {
                    Console.WriteLine("Please enter valid option\n");
                }   
            }
            void showUserMenu(string userid)
            {
                Console.WriteLine("Please select the provided optons");
                Console.WriteLine("\t\ta.deposit amount");
                Console.WriteLine("\t\tb.Withdraw amount");
                Console.WriteLine("\t\tc.transfer amount to a account in same bank");
                Console.WriteLine("\t\td.transfer amount to a account in other bank");
                Console.WriteLine("\t\te.view transaction history");
                char option = Convert.ToChar(Console.ReadLine());
                var current_bank = AllBanks[bankId];
                if (option == 'a')
                {
                    current_bank.DepositAmount(bankId,userid);
                }
                else if (option == 'b')
                {
                    current_bank.WithdrawAmount(bankId,userid);
                }
                else if (option == 'c')
                {
                    current_bank.TransferAmount("samebank",userid,bankId);
                }
                else if (option == 'd')
                {
                    current_bank.TransferAmount("otherbank",userid,bankId);
                }
                else if (option == 'e')
                {
                    current_bank.TransactionHistory();
                }
                else
                {
                    Console.WriteLine("Enter valid option");
                }

            }
            
        }
    }
}
