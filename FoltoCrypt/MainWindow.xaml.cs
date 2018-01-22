using FoltoCrypt.Classes;
using FoltoCrypt.Windows;
using System;
using System.Collections.Generic;
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

namespace FoltoCrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CreateWallet createWallet;
        public MainWindow()
        {
            InitializeComponent();
            NewWallets();
        }

        #region Main Functions
        private void NewWallets()
        {
            ManagerOfCurrence.Start();
        }
        #endregion

        #region Modal
        private void OpenChange(int N = -1)
        {
            if (N != -1)
            {
                createWallet = new CreateWallet();
            }
            else
            {
                createWallet = new CreateWallet();
            }
            
            createWallet.ShowDialog();
            createWallet.Activate();
        }
        #endregion
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenChange();
        }

    }
}
