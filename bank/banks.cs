using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    internal class Banks
    {
       public double same_bank_rtgs = 0, same_bank_imps = 0.05, otherbank_rtgs = 0.02, otherbank_imps = 0.06;
       public string accepted_currency = "INR";
       public string bankID;
       public Dictionary<string, string> AccountCredentials = new Dictionary<string, string>();
       public Dictionary<string, double> AccountBalances = new Dictionary<string, double>();
       public Dictionary<string, List<string>> Transactions = new Dictionary<string, List<string>>();
      
        static string TxnIdGenerator(string b_id, string acc_id)
        {
            DateTime date = DateTime.Today;
            string currentDate = date.ToString();
            string tempId;
            tempId = "TXN" + b_id + acc_id + currentDate.Substring(0, 10);
            return tempId;
        }
        public void ChangeCharges(string bankType)
        {
            if (bankType == "samebank")
            {
                Console.WriteLine("Enter imps charges for same bank");
                same_bank_imps = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter rtgs charges for same bank");
                same_bank_rtgs = Convert.ToDouble(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Enter imps charges for other bank");
                otherbank_rtgs = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter rtgs charges for other bank");
                otherbank_rtgs = Convert.ToDouble(Console.ReadLine());
            }
            Console.WriteLine("\t\tChanges Done\n");
        }
        public static void InfoCreator(string txnid,string send_bankid,string re_bankid,string send_acid,string re_acid,double amnt,double charges)
        {
            ArrayList details = new ArrayList();
            details.Add(send_bankid);
            details.Add(re_bankid);
            details.Add(send_acid);
            details.Add(re_acid);
            details.Add(amnt);
            details.Add(charges);
            Program.SpecialInfo.Add(txnid, details);
        }
       public void CreateAccount()
        {
            Console.WriteLine("Please enter your name");
            string name = Console.ReadLine();
            string user_id = Program.IdGenerator(name);
            Random num = new Random();
            string assets_of_password = "abcdefghijklmnopqrstuvwxyz0123456789";
            string password = "";
            for (int i = 0; i < 7; i++)
            {
                int random_num = num.Next(assets_of_password.Length);
                password = password + assets_of_password[random_num];
            }
            if (AccountCredentials.ContainsKey(user_id))
            {
                Console.WriteLine("user already exists");
            }
            else
            {
                AccountCredentials.Add(user_id, password);
                AccountBalances.Add(user_id, 0);
                Transactions.Add(user_id, new List<string>());
                Console.WriteLine($"your account credentials \n\t\t ID:{user_id}\n\t\t password:{password}\n");
            }

        }
        public void DeleteAccount()
        {
            Console.WriteLine("please enter account id");
            string id = Console.ReadLine();
            if (AccountCredentials.ContainsKey(id))
            {
                AccountBalances.Remove(id);
                AccountCredentials.Remove(id);
                Transactions.Remove(id);
                Console.WriteLine("successfully removed specified account\n");
            }
            else
            {
                Console.WriteLine("Sorry,this account is not available in the bank\n");
            }

        }
        public void DepositAmount(string bnk_id,string userid)
        {
            string bankid = bnk_id;
            string ac_id = userid;
            if (AccountCredentials.ContainsKey(ac_id))
            {
                string txn_id = TxnIdGenerator(bankid, ac_id);
                Console.WriteLine("Please enter 1 if you are trying to deposit amount with other currencies\n\tenter 2 if you are trying to deposit with Indian currency");
                int option =Convert.ToInt32(Console.ReadLine());
                if (option == 1)
                {
                    double exchange_rate=CurrencyConverter.ShowCurrencies();
                    Console.WriteLine("Please enter the amount in selected currency only");
                    double amount = Convert.ToDouble(Console.ReadLine());
                    double new_amnt = amount * exchange_rate;
                    AccountBalances[ac_id] += new_amnt;
                    Transactions[ac_id].Add(txn_id + $"\t: added {new_amnt} & final balance : {AccountBalances[ac_id]}\n");
                    Console.WriteLine("\t\t transaction successful\n");

                }
                else if (option == 2)
                {
                    Console.WriteLine("Please enter the amount in indian rupees only");
                    double amount = Convert.ToDouble(Console.ReadLine());
                    AccountBalances[ac_id] += amount;
                    Transactions[ac_id].Add(txn_id + $"\t: added {amount} & final balance : {AccountBalances[ac_id]}\n");
                    Console.WriteLine("\t\t transaction successful\n");
                }
                else
                {
                    Console.WriteLine("Please enter valid option\n");
                }
            }
            else
            {
                Console.WriteLine("Account not found\n");
            }
        }
        public void TransferAmount(string which_bank,string sender,string bnk_id)
        {
            if (which_bank == "samebank")
            {
                Console.WriteLine("You have chosen to transfer amount within the bank \n");
                string bank_id = bnk_id;
                string sender_ac_id = sender;
                if (AccountCredentials.ContainsKey(sender_ac_id))
                {
                    Console.WriteLine("Enter reciever account id");
                    string reciever_ac_id = Console.ReadLine();
                    if (AccountCredentials.ContainsKey(reciever_ac_id))
                    {
                        Console.WriteLine("Please enter the amount to be transferred");
                        double amnt = Convert.ToDouble(Console.ReadLine());
                        if (amnt <= AccountBalances[sender_ac_id]) 
                        { 
                        if (amnt >= 200000)
                        {
                            Console.WriteLine("Amount exceeds 2 Lakhs. Please select the transfer type\n\t a.imps \n\t b.rtgs");
                            char option = Convert.ToChar(Console.ReadLine());
                            if (option == 'a')
                            {
                                double charges = amnt * same_bank_imps;
                                double newamnt = amnt + (amnt * same_bank_imps);
                                AccountBalances[sender_ac_id] -= newamnt;
                                string txn_id = TxnIdGenerator(bank_id, sender_ac_id);
                                Transactions[sender_ac_id].Add(txn_id + $"\t: debited {newamnt} & final balance : {AccountBalances[sender_ac_id]}");
                                Program.SpecialTransactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with  special charges {charges} & final balance : {AccountBalances[sender_ac_id]}");
                                AccountBalances[reciever_ac_id] += amnt;
                                string rtxn_id = TxnIdGenerator(bank_id, reciever_ac_id);
                                Transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {AccountBalances[reciever_ac_id]}");
                                Console.WriteLine("\t\t transaction successful\n");
                                InfoCreator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
                            }
                            else
                            {
                                double charges = amnt * same_bank_rtgs;
                                double newamnt = amnt + (amnt * same_bank_rtgs);
                                AccountBalances[sender_ac_id] -= newamnt;
                                string txn_id = TxnIdGenerator(bank_id, sender_ac_id);
                                Transactions[sender_ac_id].Add(txn_id + $"\t: debited {newamnt} & final balance : {AccountBalances[sender_ac_id]}");
                                Program.SpecialTransactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with  special charges {charges} & final balance : {AccountBalances[sender_ac_id]}");
                                AccountBalances[reciever_ac_id] += amnt;
                                string rtxn_id = TxnIdGenerator(bank_id, reciever_ac_id);
                                Transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {AccountBalances[reciever_ac_id]}");
                                Console.WriteLine("\t\t transaction successful\n");
                                InfoCreator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
                            }
                        }
                        else
                        {
                            double charges = 0;
                            AccountBalances[sender_ac_id] -= amnt;
                            string txn_id = TxnIdGenerator(bank_id, sender_ac_id);
                            Transactions[sender_ac_id].Add(txn_id + $"\t: transferred {amnt} to {reciever_ac_id} & final balance : {AccountBalances[sender_ac_id]}");
                            Program.SpecialTransactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {AccountBalances[sender_ac_id]}");
                            AccountBalances[reciever_ac_id] += amnt;
                            string rtxn_id = TxnIdGenerator(bank_id, reciever_ac_id);
                            Transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {AccountBalances[reciever_ac_id]}");
                            Console.WriteLine("\t\t transaction successful\n");
                            InfoCreator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
                        }
                    }
                        else
                        {
                           Console.WriteLine("You have insufficient balance\n");
                        }
                }
                else
                {
                    Console.WriteLine("Reciever account not found\n");
                }
                }
                else
                {
                    Console.WriteLine("Sender account not found\n");
                }
            }
            else
            {
                Console.WriteLine("You have chosen to transfer amount from one bank to other bank");
                string sender_bankid=bnk_id;
                if(Program.AllBanks.ContainsKey(sender_bankid))
                {
                    Console.WriteLine("Please enter reciever bankid");
                    string reciever_bankid = Console.ReadLine();
                    if(Program.AllBanks.ContainsKey(reciever_bankid))
                    {
                        string sender_ac_id = sender;
                        Console.WriteLine("Please enter reciever ac_id");
                        string reciever_ac_id = Console.ReadLine();
                        Console.WriteLine("Please enter the amount in indian rupees only");
                        double amnt= Convert.ToDouble(Console.ReadLine());
                        var current_sender=Program.AllBanks[sender_bankid];
                        var current_reciever = Program.AllBanks[reciever_bankid];
                        if (amnt <= current_sender.AccountBalances[sender_ac_id])
                        {
                            if (amnt < 200000)
                            {
                                double charges = 0;
                                current_sender.AccountBalances[sender_ac_id] -= amnt;
                                string txnid = TxnIdGenerator(sender_bankid, sender_ac_id);
                                current_sender.Transactions[sender_ac_id].Add(txnid + $"\t: transferred {amnt} to {reciever_ac_id} & final balance : {current_sender.AccountBalances[sender_ac_id]}");
                                string rtxn_id = TxnIdGenerator(reciever_bankid, reciever_ac_id);
                                current_reciever.AccountBalances[reciever_ac_id] += amnt;
                                current_reciever.Transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {current_reciever.AccountBalances[reciever_ac_id]}");
                                Program.SpecialTransactions.Add(txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {AccountBalances[sender_ac_id]}");
                                InfoCreator(txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
                                Console.WriteLine("\t\t transaction successful\n");

                            }
                            else
                            {
                                Console.WriteLine("Amount exceeds 2 Lakhs. Please select the transfer type\n\t a.imps \n\t b.rtgs");
                                char option = Convert.ToChar(Console.ReadLine());
                                if (option == 'a')
                                {
                                    double charges = amnt * otherbank_imps;
                                    double new_amnt = amnt + charges;
                                    current_sender.AccountBalances[sender_ac_id] -= new_amnt;
                                    string sent_txnid = TxnIdGenerator(sender_bankid, sender_ac_id);
                                    current_sender.Transactions[sender_ac_id].Add(sent_txnid + $"\t: debited {new_amnt} & final balance : {current_sender.AccountBalances[sender_ac_id]}");
                                    current_reciever.AccountBalances[reciever_ac_id] += amnt;
                                    string recieved_txnid = TxnIdGenerator(reciever_bankid, reciever_ac_id);
                                    current_reciever.Transactions[reciever_ac_id].Add(recieved_txnid + $"\t: credited {amnt} & final balance : {current_reciever.AccountBalances[reciever_ac_id]}");
                                    Program.SpecialTransactions.Add(sent_txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {AccountBalances[sender_ac_id]}");
                                    InfoCreator(sent_txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
                                    Console.WriteLine("\t\t transaction successful\n");
                                }
                                else
                                {
                                    double charges = amnt * otherbank_imps;
                                    double new_amnt = amnt + charges;
                                    current_sender.AccountBalances[sender_ac_id] -= new_amnt;
                                    string sent_txnid = TxnIdGenerator(sender_bankid, sender_ac_id);
                                    current_sender.Transactions[sender_ac_id].Add(sent_txnid + $"\t: debited {amnt} & final balance : {current_sender.AccountBalances[sender_ac_id]}");
                                    current_reciever.AccountBalances[reciever_ac_id] += amnt;
                                    string recieved_txnid = TxnIdGenerator(reciever_bankid, reciever_ac_id);
                                    current_reciever.Transactions[reciever_ac_id].Add(recieved_txnid + $"\t: credited {amnt} & fina balance : {current_reciever.AccountBalances[reciever_ac_id]}");
                                    Program.SpecialTransactions.Add(sent_txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {AccountBalances[sender_ac_id]}");
                                    InfoCreator(sent_txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
                                    Console.WriteLine("\t\t transaction successful\n");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{reciever_bankid} does not exist");
                    }

                }
                else
                {
                    Console.WriteLine($"{sender_bankid} does not exist");
                }
                
            }
        }
       public void TransactionHistory()
        {
            Console.WriteLine("Enter the account id");
            string ac_id = Console.ReadLine();
            foreach (string i in Transactions[ac_id])
            {
                Console.WriteLine(i);

            }
            Console.WriteLine();
        }
        public void WithdrawAmount(string bnkid,string a_id)
        {
            string bankid = bnkid;
            string ac_id = a_id;
            var current_bank = Program.AllBanks[bankid];
            Console.WriteLine("Please enter the amount");
            double amnt=Convert.ToDouble(Console.ReadLine());
            if (amnt <= current_bank.AccountBalances[ac_id])
            {
                current_bank.AccountBalances[ac_id] -= amnt;
                string txnid = TxnIdGenerator(bankid, ac_id);
                current_bank.Transactions[ac_id].Add(txnid + $"\t: debited {amnt} & fina balance : {current_bank.AccountBalances[ac_id]}");
                Console.WriteLine("\t\t transaction successful\n");
            }
            else
            {
                Console.WriteLine("Insufficient Balance\n");
            }

        }
        public void ShowAccounts()
        {
            foreach(string i in AccountCredentials.Keys)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
        }
        public static void NowRevertTransaction(string sender_bank, string re_bankid, string send_acid, string re_acid, double amnt, double charges)
        {
            var current_send_bank = Program.AllBanks[re_bankid];
            var current_receive_bank = Program.AllBanks[sender_bank];
            current_send_bank.AccountBalances[re_acid] -= amnt;
            string sent_txnid = TxnIdGenerator(re_bankid,re_acid);
            current_send_bank.Transactions[re_acid].Add(sent_txnid + $"\t: debited {amnt} & fina balance : {current_send_bank.AccountBalances[re_acid]}");
            current_receive_bank.AccountBalances[send_acid] += (charges + amnt);
            string rtxn_id = TxnIdGenerator(sender_bank, send_acid);
            current_receive_bank.Transactions[send_acid].Add(rtxn_id + $"\t: credited {amnt} & final balance : {current_receive_bank.AccountBalances[send_acid]}");
            Program.SpecialTransactions.Add(sent_txnid, $"\t: transferred {amnt} from {re_acid} to {send_acid} with  special charges {charges} & final balance : {current_send_bank.AccountBalances[re_acid]}");
            InfoCreator(sent_txnid, re_bankid, sender_bank,re_acid,send_acid, amnt, charges);
            Console.WriteLine("\t\tTransaction Successful");
        }
    }
}
