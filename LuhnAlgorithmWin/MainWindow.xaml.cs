using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuhnAlgorithmWin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreditCardNumber_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //Hide validation messages on startup and changing input number
            ValidationTest.Visibility = Visibility.Hidden;
            Type.Visibility = Visibility.Hidden;
            Bank.Visibility = Visibility.Hidden;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> card_number_array = new List<int>();
            if (validatenumber(CreditCardNumber.Text, out card_number_array) == true) //Checks to see if actual number
            {
                card_number_array.Reverse();
                if (luhn_checksum(card_number_array)) //Checks input number
                {
                    ValidationTest.Foreground = Brushes.Green;
                    ValidationTest.Content = "Number passes the Luhn Algorithm";
                    string[] bankNames = GetInformation(CreditCardNumber.Text);  //Get bank & card info
                    Type.Content = "Type: " + bankNames[1];                      //Displays info
                    Bank.Content = "Issuing Bank: " + bankNames[2];
                    ValidationTest.Visibility = Visibility.Visible;
                    Type.Visibility = Visibility.Visible;
                    Bank.Visibility = Visibility.Visible;
                }
                else
                {
                    ValidationTest.Foreground = Brushes.Red;                    //Displays algorithm failed messages
                    ValidationTest.Content = "Number fails the Luhn Algorithm";
                    ValidationTest.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                ValidationTest.Content = "Invalid input";
            }
        }

        //Method to read CSV and searches for information 
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

        //Check to see if each character entered is actually a digit
        public static bool validatenumber(string creditcardinput, out List<int> card_number_array)
        {
            bool validate = true;
            int digit = 0;
            card_number_array = new List<int>();
            for (int i = 0; i < creditcardinput.Length; i++)
            {
                if (Int32.TryParse(creditcardinput.Substring(i, 1), out digit) == true)
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
            //adds up all the even placed digits
            for (int i = 0; i < card_number_array.Count(); i = i + 2)
            {
                checksum += card_number_array[i];
            }

            int index2 = 0;
            //now adds up (2 x odd placed digits) 
            for (int i = 1; i < card_number_array.Count(); i = i + 2)
            {
                index2 = card_number_array[i] * 2;
                if (index2 >= 10)
                {
                    index2 = index2 - 9; // if 2 x digit > 10
                }
                checksum += index2;
            }

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
