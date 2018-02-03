using FoltoCrypt.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace FoltoCrypt.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateWallet.xaml
    /// </summary>
    public partial class CreateWallet : Window
    {
        public delegate void MethodContainer();
        public event MethodContainer OK;
        public event MethodContainer Cancel;
        private bool ok = false;
        public int IdWal = -1;

        public CreateWallet(ItemWallet WAL = null)
        {
            InitializeComponent();
            if (WAL != null)
            {
                IdWal = WAL.Id;
                PName.Text = WAL.Name;
                PBalance.Text = WAL.BalCur.ToString();
                PCurrencyB.Text = WAL.Currency_B;
                PInvestment.Text = WAL.InvCur.ToString();
                PCurrencyI.Text = WAL.Currency_I;
            }
        }

        public bool Check()
        {
            bool answer = true;
            double Bal;

            if (!double.TryParse(PBalance.Text, out Bal))
            {
                answer = false;
                string TRY = PBalance.Text;
                TRY = TRY.Replace(".", ",");
                if (double.TryParse(TRY, out Bal))
                {
                    answer = true;
                    PBalance.Text = TRY;
                }
            }

            if (!double.TryParse(PInvestment.Text, out Bal))
            {
                answer = false;
                string TRY = PInvestment.Text;
                TRY = TRY.Replace(".", ",");
                if (double.TryParse(TRY, out Bal))
                {
                    answer = true;
                    PInvestment.Text = TRY;
                }
            }

            if (PName.Text==""|| PBalance.Text==""|| PCurrencyB.Text==""|| PInvestment.Text==""|| PCurrencyI.Text=="")
            {
                answer = false;
            }
            
            return answer;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(!ok)Cancel?.Invoke();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                ok = true;
                OK?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show("Fill in all the fields!");
            }
        }
    }
}
