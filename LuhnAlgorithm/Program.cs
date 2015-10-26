using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuhnAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            string card_num;
            Console.WriteLine("Please enter a credit card number:");
            card_num = Console.ReadLine();
            List<int> card_number_array = new List<int>();
            if (validatenumber(card_num, out card_number_array)==true)
            {
                card_number_array.Reverse();
                Console.WriteLine(luhn_checksum(card_number_array));
                string[] bankNames = GetInformation(card_num);
                Console.WriteLine("Type: {0}\nIssuing Bank: {1}",bankNames[1], bankNames[2]);
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
         
        }

        public static string[] GetInformation(string creditCardNumber)
        {
            if (creditCardNumber.Length > 6 && File.Exists("iin.csv"))
            {
                string iin = creditCardNumber.Substring(0, 6);
                using (StreamReader reader = new StreamReader("iin.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if ((values[0] == iin) && (creditCardNumber.Length == int.Parse(values[3])))
                        {
                            return values;
                        }
                    }
                }
            }
            string[] BankInfo = new string[3];
            BankInfo[1] = "Information not available";
            BankInfo[2] = "Information not available";
            return BankInfo;
        }

        public static bool validatenumber(string creditcardinput, out List<int> card_number_array)
        {
            bool validate = true;
            int digit = 0;
            card_number_array = new List<int>();
            for (int i = 0; i < creditcardinput.Length; i++)
            {
                if (Int32.TryParse(creditcardinput.Substring(i,1),out digit) == true)
                {
                    card_number_array.Add(digit);
                }
                else
                {
                    validate = false;
                    break;
                }
            }

                return validate;
        }

        public static bool luhn_checksum(List<int> card_number_array)
        {
            int checksum = 0;
            for (int i = 0; i < card_number_array.Count(); i = i + 2)
            {
                checksum += card_number_array[i];
            }
            Console.WriteLine(checksum);
            int index2 = 0;
            for (int i = 1; i < card_number_array.Count(); i = i + 2)
            {
                index2 = card_number_array[i] * 2;
                if (index2 >= 10)
                {
                    index2 = index2 - 9;
                }
                checksum += index2;
            }
            Console.WriteLine(checksum);
            if (checksum % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

       
    }
}
