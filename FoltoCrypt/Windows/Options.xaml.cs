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
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        double ptime;
        Classes.Options options;

        public Options(ref Classes.Options opti)
        {
            InitializeComponent();
            options = opti;
            PName.Text = options.MainCurrency;
            PTime.Text = Convert.ToString(options.BTime);
        }

        public bool Check()
        {
            bool answer = true;

            if (!double.TryParse(PTime.Text, out ptime))
            {
                answer = false;
                string TRY = PTime.Text;
                TRY = TRY.Replace(".", ",");
                if (double.TryParse(TRY, out ptime))
                {
                    answer = true;
                }
            }
            if (PName.Text == "" ||PTime.Text == "") answer = false;
            
            return answer;
        }

        public void Ses()
        {
            options.Set(PName.Text, ptime);
            MainFunctions.SaveOptions(options);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                Ses();
                Close();
            }
            else
            {
                MessageBox.Show("Fill in all the fields!");
            }
        }

        private void Canc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
