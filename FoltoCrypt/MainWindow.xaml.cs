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
using System.Threading;

namespace FoltoCrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ItemWallet> walletArray;
        public string Path;
        CreateWallet createWallet;
        public Classes.Options mainOptions = new Classes.Options();
        public System.Windows.Threading.DispatcherTimer Time;

        Windows.Options options;
        public MainWindow()
        {
            InitializeComponent();
            NewWallets();
            StartOptions();

            Time = new System.Windows.Threading.DispatcherTimer();
            Time.Tick += new EventHandler(timerTick);
        }

        #region Main Functions

        async void REFRESHALL()
        {
            if(dataGrid.Items.Count!=0)
            {
                await ManagerOfCurrence.Refresh();
                Refresh();
            }
        }

        private void StartOptions()
        {
            //открывает
            if (!MainFunctions.LoadOptions(out mainOptions))
            {
                mainOptions = new Classes.Options();
                OpenOptions();
            }
            else
            {
                ManagerOfCurrence.SetCur(mainOptions.MainCurrency);
            }
            //если не открылось открывет опции.
        }

        private void NewWallets()
        {
            LabelTotIn.Content = "";
            LabelTotCo.Content = "";
            LabelTotBa.Content = "";
            ManagerOfCurrence.Start();
            walletArray = new List<ItemWallet>();
            Path = "";
            dataGrid.Items.Clear();
            CreateColumns();
        }

        private void CreateColumns()
        {
            // Добавляем колонки
            string[] columns = { "Id", "Name", "Investment", "Cost", "Balance" };
            //AddColumns(columns);
        }

        private void AddColumns(string[] str)
        {
            foreach (var index in str)
            {
                if (index == "Id")
                {
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = index,
                        Binding = new Binding(index),
                        Visibility = Visibility.Hidden,
                        
                    });
                }
                else if(index == "Cost")
                {
                    /*DataGridTextColumn column = new DataGridTextColumn();
                    column.ElementStyle = this.FindResource("MyStyle") as Style;*/
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = index,
                        Binding = new Binding(index),
                        CellStyle = this.FindResource("IdStyle") as Style
                    });
                }
                else
                {
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = index,
                        Binding = new Binding(index)
                    });
                }
            }
        }

        private  void Refresh()
        {
            dataGrid.Items.Clear();
            double totIn = 0;
            double totCo = 0;
            double totBa = 0;
            foreach (var p in walletArray)
            {
                p.Price_I = ManagerOfCurrence.Get(p.Currency_I);
                p.Price_B = ManagerOfCurrence.Get(p.Currency_B);
                //p.Price_I = MainWindow.managerOfCurrence.Get(p.Currency_I);
                //p.Price_B = MainWindow.managerOfCurrence.Get(p.Currency_B);
                totIn += p.InvCur / p.Price_I;
                totCo += p.BalCur / p.Price_B - p.InvCur / p.Price_I;
                totBa += p.BalCur / p.Price_B;
                dataGrid.Items.Add(p);
            }
            LabelTotIn.Content = "Total investment: " + Math.Round(totIn,8) + " " + ManagerOfCurrence.MainCurrency;
            LabelTotCo.Content = "Cost: " + Math.Round(totCo, 8) + " " + ManagerOfCurrence.MainCurrency;
            LabelTotBa.Content = "Balance: " + Math.Round(totBa, 8) + " " + ManagerOfCurrence.MainCurrency;
        }

        private async void EditStroke()
        {
            var Ed = walletArray.FirstOrDefault(x => x.Id == (dataGrid.SelectedItem as ItemWallet).Id);
            Ed.Id = createWallet.IdWal;
            Ed.Name = createWallet.PName.Text;
            Ed.BalCur = double.Parse(createWallet.PBalance.Text);
            Ed.Currency_B = createWallet.PCurrencyB.Text;
            Ed.InvCur = double.Parse(createWallet.PInvestment.Text);
            Ed.Currency_I = createWallet.PCurrencyI.Text;

            CloseChange();

            await ManagerOfCurrence.Refresh();
            Refresh();
        }

        private async void AddStroke()
        {
            var cur1 = createWallet.PCurrencyB.Text;
            var cur2 = createWallet.PCurrencyI.Text;
            double Cur1, Cur2;
            try
            {
                Cur1 = double.Parse(createWallet.PBalance.Text);
                Cur2 = double.Parse(createWallet.PInvestment.Text);
            }
            catch
            {
                Cur1 = 0;
                Cur2 = 0;
            }
            var name = createWallet.PName.Text;

            CloseChange();

            ManagerOfCurrence.New(cur1);
            ManagerOfCurrence.New(cur2);
            //MainWindow.managerOfCurrence.New(cur1);
            //MainWindow.managerOfCurrence.New(cur2);

            walletArray.Add(new ItemWallet
            {
                Name = name,
                InvCur = Cur2,
                Currency_I = cur2,
                BalCur = Cur1,
                Currency_B = cur1,
                Price_I = 0,
                Price_B = 0,
                Id = walletArray.Count
            });
            dataGrid.Items.Add(walletArray[walletArray.Count - 1]);
            await ManagerOfCurrence.Refresh();
            //MainWindow.managerOfCurrence.Refresh();
            Refresh();
        }

        private void DeleteStroke(int Index)
        {
            if (Index != -1)
            {

                walletArray.Remove(walletArray.FirstOrDefault(x => x.Id == (dataGrid.SelectedItem as ItemWallet).Id));
                dataGrid.Items.RemoveAt(Index);
                Refresh();
            }
        }

        private void OpenOptions()
        {
            options = new Windows.Options(ref mainOptions);
            options.ShowDialog();
            options.Activate();
            ManagerOfCurrence.SetCur(mainOptions.MainCurrency);
        }

        private void OpenChange(int N = -1)
        {
            if (N != -1)
            {
                createWallet = new CreateWallet(walletArray.FirstOrDefault(x => x.Id == (dataGrid.SelectedItem as ItemWallet).Id));
                createWallet.OK += EditStroke;
            }
            else
            {
                createWallet = new CreateWallet();
                createWallet.OK += AddStroke;
            }
            createWallet.Cancel += CloseChange;
            createWallet.ShowDialog();
            createWallet.Activate();
        }

        private void CloseChange()
        {
            if (createWallet.IdWal == -1)
            {
                createWallet.OK -= AddStroke;
            }
            else
            {
                createWallet.OK -= EditStroke;
            }
            createWallet.Cancel -= CloseChange;
        }

        private void SaveAs()
        {
            if (Path != "") SaveJust();
            else
            {
                SaveChooseDir();
            }
        }

        private async void OpenChooseDir()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "My Wallets"; // Default file name
            dlg.DefaultExt = ".jw"; // Default file extension
            dlg.Filter = "JSON wallets|*.jw"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                NewWallets();
                walletArray = MainFunctions.LoadWallets(dlg.FileName);

                foreach (var p in walletArray)
                {
                    ManagerOfCurrence.New(p.Currency_B);
                    ManagerOfCurrence.New(p.Currency_I);
                }

                await ManagerOfCurrence.Refresh();
                Refresh();
                Path = dlg.FileName;
            }
        }

        private void SaveChooseDir()
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "My Wallets"; // Default file name
            dlg.DefaultExt = ".jw"; // Default file extension
            dlg.Filter = "JSON wallets|*.jw"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                Path = dlg.FileName;
                SaveJust();
                //MessageBox.Show(Path, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveJust()
        {
            MainFunctions.SaveWallets(walletArray,Path);
        }
        #endregion

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenChange();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 1)
            {
                if (AutoUp.IsChecked ?? false)
                {
                    AutoUp.IsChecked = false;
                    Time.Stop();
                }
                OpenChange(dataGrid.SelectedIndex);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 1)
            {
                DeleteStroke(dataGrid.SelectedIndex);
            }
        }

        private void New_Click(object sender, RoutedEventArgs e) => NewWallets();

        private void Refre_Click(object sender, RoutedEventArgs e) => REFRESHALL();

        private void Open_Click(object sender, RoutedEventArgs e) => OpenChooseDir();

        private void Save_Click(object sender, RoutedEventArgs e) => SaveAs();

        private void SaveAs_Click(object sender, RoutedEventArgs e) => SaveChooseDir();

        private void Exit_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenOptions();
        }

        private void timerTick(object sender, EventArgs e)
        {
            REFRESHALL();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (AutoUp.IsChecked ?? false)
            {
                var a = mainOptions.BTime;
                int m, s;
                m = Convert.ToInt32(Math.Truncate(a));
                s = Convert.ToInt32((a - m) * 10);

                Time.Interval = new TimeSpan(0, m, s);

                Time.Start();
            }
            else
            {
                Time.Stop();
            }
        }
    }
}
