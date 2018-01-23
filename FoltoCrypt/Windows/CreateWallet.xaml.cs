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
            ok = true;
            OK?.Invoke();
            Close();
        }
    }
}
