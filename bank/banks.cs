using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    internal class banks
    {
       public double same_bank_rtgs = 0, same_bank_imps = 0.05, otherbank_rtgs = 0.02, otherbank_imps = 0.06;
       public string accepted_currency = "INR";
       public string bankID;
       public Dictionary<string, string> account_credentials = new Dictionary<string, string>();
       public Dictionary<string, double> account_balances = new Dictionary<string, double>();
       public Dictionary<string, List<string>> transactions = new Dictionary<string, List<string>>();
      
        static string txn_id_Generator(string b_id, string acc_id)
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
        public static void info_creator(string txnid,string send_bankid,string re_bankid,string send_acid,string re_acid,double amnt,double charges)
        {
            ArrayList details = new ArrayList();
            details.Add(send_bankid);
            details.Add(re_bankid);
            details.Add(send_acid);
            details.Add(re_acid);
            details.Add(amnt);
            details.Add(charges);
            Program.special_info.Add(txnid, details);
        }
       public void create_account()
        {
            Console.WriteLine("Please enter your name");
            string name = Console.ReadLine();
            string user_id = Program.id_Generator(name);
            Random num = new Random();
            string assets_of_password = "abcdefghijklmnopqrstuvwxyz0123456789";
            string password = "";
            for (int i = 0; i < 7; i++)
            {
                int random_num = num.Next(assets_of_password.Length);
                password = password + assets_of_password[random_num];
            }
            if (account_credentials.ContainsKey(user_id))
            {
                Console.WriteLine("user already exists");
            }
            else
            {
                account_credentials.Add(user_id, password);
                account_balances.Add(user_id, 0);
                transactions.Add(user_id, new List<string>());
                Console.WriteLine($"your account credentials \n\t\t ID:{user_id}\n\t\t password:{password}\n");
            }

        }
        public void delete_account()
        {
            Console.WriteLine("please enter account id");
            string id = Console.ReadLine();
            if (account_credentials.ContainsKey(id))
            {
                account_balances.Remove(id);
                account_credentials.Remove(id);
                transactions.Remove(id);
                Console.WriteLine("successfully removed specified account\n");
            }
            else
            {
                Console.WriteLine("Sorry,this account is not available in the bank\n");
            }

        }
        public void deposit_amount(string bnk_id,string userid)
        {
            string bankid = bnk_id;
            string ac_id = userid;
            if (account_credentials.ContainsKey(ac_id))
            {
                string txn_id = txn_id_Generator(bankid, ac_id);
                Console.WriteLine("Please enter 1 if you are trying to deposit amount with other currencies\n\tenter 2 if you are trying to deposit with Indian currency");
                int option =Convert.ToInt32(Console.ReadLine());
                if (option == 1)
                {
                    double exchange_rate=CurrencyConverter.ShowCurrencies();
                    Console.WriteLine("Please enter the amount in selected currency only");
                    double amount = Convert.ToDouble(Console.ReadLine());
                    double new_amnt = amount * exchange_rate;
                    account_balances[ac_id] += new_amnt;
                    transactions[ac_id].Add(txn_id + $"\t: added {new_amnt} & final balance : {account_balances[ac_id]}\n");
                    Console.WriteLine("\t\t transaction successful\n");

                }
                else if (option == 2)
                {
                    Console.WriteLine("Please enter the amount in indian rupees only");
                    double amount = Convert.ToDouble(Console.ReadLine());
                    account_balances[ac_id] += amount;
                    transactions[ac_id].Add(txn_id + $"\t: added {amount} & final balance : {account_balances[ac_id]}\n");
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
        public void transfer_amount(string which_bank,string sender,string bnk_id)
        {
            if (which_bank == "samebank")
            {
                Console.WriteLine("You have chosen to transfer amount within the bank \n");
                string bank_id = bnk_id;
                string sender_ac_id = sender;
                if (account_credentials.ContainsKey(sender_ac_id))
                {
                    Console.WriteLine("Enter reciever account id");
                    string reciever_ac_id = Console.ReadLine();
                    if (account_credentials.ContainsKey(reciever_ac_id))
                    {
                        Console.WriteLine("Please enter the amount to be transferred");
                        double amnt = Convert.ToDouble(Console.ReadLine());
                        if (amnt <= account_balances[sender_ac_id]) 
                        { 
                        if (amnt >= 200000)
                        {
                            Console.WriteLine("Amount exceeds 2 Lakhs. Please select the transfer type\n\t a.imps \n\t b.rtgs");
                            char option = Convert.ToChar(Console.ReadLine());
                            if (option == 'a')
                            {
                                double charges = amnt * same_bank_imps;
                                double newamnt = amnt + (amnt * same_bank_imps);
                                account_balances[sender_ac_id] -= newamnt;
                                string txn_id = txn_id_Generator(bank_id, sender_ac_id);
                                transactions[sender_ac_id].Add(txn_id + $"\t: debited {newamnt} & final balance : {account_balances[sender_ac_id]}");
                                Program.special_transactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with  special charges {charges} & final balance : {account_balances[sender_ac_id]}");
                                account_balances[reciever_ac_id] += amnt;
                                string rtxn_id = txn_id_Generator(bank_id, reciever_ac_id);
                                transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {account_balances[reciever_ac_id]}");
                                Console.WriteLine("\t\t transaction successful\n");
                                info_creator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
                            }
                            else
                            {
                                double charges = amnt * same_bank_rtgs;
                                double newamnt = amnt + (amnt * same_bank_rtgs);
                                account_balances[sender_ac_id] -= newamnt;
                                string txn_id = txn_id_Generator(bank_id, sender_ac_id);
                                transactions[sender_ac_id].Add(txn_id + $"\t: debited {newamnt} & final balance : {account_balances[sender_ac_id]}");
                                Program.special_transactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with  special charges {charges} & final balance : {account_balances[sender_ac_id]}");
                                account_balances[reciever_ac_id] += amnt;
                                string rtxn_id = txn_id_Generator(bank_id, reciever_ac_id);
                                transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {account_balances[reciever_ac_id]}");
                                Console.WriteLine("\t\t transaction successful\n");
                                info_creator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
                            }
                        }
                        else
                        {
                            double charges = 0;
                            account_balances[sender_ac_id] -= amnt;
                            string txn_id = txn_id_Generator(bank_id, sender_ac_id);
                            transactions[sender_ac_id].Add(txn_id + $"\t: transferred {amnt} to {reciever_ac_id} & final balance : {account_balances[sender_ac_id]}");
                            Program.special_transactions.Add(txn_id, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {account_balances[sender_ac_id]}");
                            account_balances[reciever_ac_id] += amnt;
                            string rtxn_id = txn_id_Generator(bank_id, reciever_ac_id);
                            transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {account_balances[reciever_ac_id]}");
                            Console.WriteLine("\t\t transaction successful\n");
                            info_creator(txn_id, bank_id, bank_id, sender_ac_id, reciever_ac_id, amnt, charges);
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
                if(Program.all_banks.ContainsKey(sender_bankid))
                {
                    Console.WriteLine("Please enter reciever bankid");
                    string reciever_bankid = Console.ReadLine();
                    if(Program.all_banks.ContainsKey(reciever_bankid))
                    {
                        string sender_ac_id = sender;
                        Console.WriteLine("Please enter reciever ac_id");
                        string reciever_ac_id = Console.ReadLine();
                        Console.WriteLine("Please enter the amount in indian rupees only");
                        double amnt= Convert.ToDouble(Console.ReadLine());
                        var current_sender=Program.all_banks[sender_bankid];
                        var current_reciever = Program.all_banks[reciever_bankid];
                        if (amnt <= current_sender.account_balances[sender_ac_id])
                        {
                            if (amnt < 200000)
                            {
                                double charges = 0;
                                current_sender.account_balances[sender_ac_id] -= amnt;
                                string txnid = txn_id_Generator(sender_bankid, sender_ac_id);
                                current_sender.transactions[sender_ac_id].Add(txnid + $"\t: transferred {amnt} to {reciever_ac_id} & final balance : {current_sender.account_balances[sender_ac_id]}");
                                string rtxn_id = txn_id_Generator(reciever_bankid, reciever_ac_id);
                                current_reciever.account_balances[reciever_ac_id] += amnt;
                                current_reciever.transactions[reciever_ac_id].Add(rtxn_id + $"\t: credited {amnt} & final balance : {current_reciever.account_balances[reciever_ac_id]}");
                                Program.special_transactions.Add(txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {account_balances[sender_ac_id]}");
                                info_creator(txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
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
                                    current_sender.account_balances[sender_ac_id] -= new_amnt;
                                    string sent_txnid = txn_id_Generator(sender_bankid, sender_ac_id);
                                    current_sender.transactions[sender_ac_id].Add(sent_txnid + $"\t: debited {new_amnt} & final balance : {current_sender.account_balances[sender_ac_id]}");
                                    current_reciever.account_balances[reciever_ac_id] += amnt;
                                    string recieved_txnid = txn_id_Generator(reciever_bankid, reciever_ac_id);
                                    current_reciever.transactions[reciever_ac_id].Add(recieved_txnid + $"\t: credited {amnt} & final balance : {current_reciever.account_balances[reciever_ac_id]}");
                                    Program.special_transactions.Add(sent_txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {account_balances[sender_ac_id]}");
                                    info_creator(sent_txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
                                    Console.WriteLine("\t\t transaction successful\n");
                                }
                                else
                                {
                                    double charges = amnt * otherbank_imps;
                                    double new_amnt = amnt + charges;
                                    current_sender.account_balances[sender_ac_id] -= new_amnt;
                                    string sent_txnid = txn_id_Generator(sender_bankid, sender_ac_id);
                                    current_sender.transactions[sender_ac_id].Add(sent_txnid + $"\t: debited {amnt} & final balance : {current_sender.account_balances[sender_ac_id]}");
                                    current_reciever.account_balances[reciever_ac_id] += amnt;
                                    string recieved_txnid = txn_id_Generator(reciever_bankid, reciever_ac_id);
                                    current_reciever.transactions[reciever_ac_id].Add(recieved_txnid + $"\t: credited {amnt} & fina balance : {current_reciever.account_balances[reciever_ac_id]}");
                                    Program.special_transactions.Add(sent_txnid, $"\t: transferred {amnt} from {sender_ac_id} to {reciever_ac_id} with 0 special charges & final balance : {account_balances[sender_ac_id]}");
                                    info_creator(sent_txnid, sender_bankid, reciever_bankid, sender_ac_id, reciever_ac_id, amnt, charges);
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
       public void transaction_history()
        {
            Console.WriteLine("Enter the account id");
            string ac_id = Console.ReadLine();
            foreach (string i in transactions[ac_id])
            {
                Console.WriteLine(i);

            }
            Console.WriteLine();
        }
        public void Withdraw_amount(string bnkid,string a_id)
        {
            string bankid = bnkid;
            string ac_id = a_id;
            var current_bank = Program.all_banks[bankid];
            Console.WriteLine("Please enter the amount");
            double amnt=Convert.ToDouble(Console.ReadLine());
            if (amnt <= current_bank.account_balances[ac_id])
            {
                current_bank.account_balances[ac_id] -= amnt;
                string txnid = txn_id_Generator(bankid, ac_id);
                current_bank.transactions[ac_id].Add(txnid + $"\t: debited {amnt} & fina balance : {current_bank.account_balances[ac_id]}");
                Console.WriteLine("\t\t transaction successful\n");
            }
            else
            {
                Console.WriteLine("Insufficient Balance\n");
            }

        }
        public void show_accounts()
        {
            foreach(string i in account_credentials.Keys)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
        }
        public static void now_revert_transaction(string sender_bank, string re_bankid, string send_acid, string re_acid, double amnt, double charges)
        {
            var current_send_bank = Program.all_banks[re_bankid];
            var current_receive_bank = Program.all_banks[sender_bank];
            current_send_bank.account_balances[re_acid] -= amnt;
            string sent_txnid = txn_id_Generator(re_bankid,re_acid);
            current_send_bank.transactions[re_acid].Add(sent_txnid + $"\t: debited {amnt} & fina balance : {current_send_bank.account_balances[re_acid]}");
            current_receive_bank.account_balances[send_acid] += (charges + amnt);
            string rtxn_id = txn_id_Generator(sender_bank, send_acid);
            current_receive_bank.transactions[send_acid].Add(rtxn_id + $"\t: credited {amnt} & final balance : {current_receive_bank.account_balances[send_acid]}");
            Program.special_transactions.Add(sent_txnid, $"\t: transferred {amnt} from {re_acid} to {send_acid} with  special charges {charges} & final balance : {current_send_bank.account_balances[re_acid]}");
            info_creator(sent_txnid, re_bankid, sender_bank,re_acid,send_acid, amnt, charges);
            Console.WriteLine("\t\tTransaction Successful");

        }


    }
}
