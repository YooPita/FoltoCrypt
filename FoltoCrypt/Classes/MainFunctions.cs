using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace FoltoCrypt.Classes
{
    static class MainFunctions
    {
        public static string NameOptions = "options";

        static public void SaveWallets(List<ItemWallet> ItemList, string FileName)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ItemWallet[]));

            using (FileStream fs = new FileStream(FileName + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, ItemList);
            }
        }

        static public List<ItemWallet> LoadWallets(string FileName)
        {
            List<ItemWallet> ItemList;
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ItemWallet[]));

            using (FileStream fs = new FileStream(FileName + ".json", FileMode.OpenOrCreate))
            {
                ItemList = (List<ItemWallet>)jsonFormatter.ReadObject(fs);
            }
            return ItemList;
        }

        static public void SaveOptions(Options ItemList)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Options[]));

            using (FileStream fs = new FileStream("options", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, ItemList);
            }
        }

        static public Options LoadOptions()
        {
            Options ItemList;
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Options[]));

            using (FileStream fs = new FileStream("options", FileMode.OpenOrCreate))
            {
                ItemList = (Options)jsonFormatter.ReadObject(fs);
            }
            return ItemList;
        }
    }

    class ManagerOfCurrence
    {
        private static List<Currency> ListOfCurrency;
        private static Options OPTIONS = new Options();
        public static string MainCurrency;

        public static void Start()
        {
            LoadOptions();
            MainCurrency = OPTIONS.MainCurrency;
            ListOfCurrency = new List<Currency>();
        }

        public static bool SaveOptions()
        {
            try
            {
                MainFunctions.SaveOptions(OPTIONS);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LoadOptions()
        {
            try
            {
                OPTIONS = MainFunctions.LoadOptions();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region Manager of currency
        public static void New(string Name)//Создаёт новую валюту, если таковой не найдено
        {
            if (!Find(Name) && Name != MainCurrency)
            {
                ListOfCurrency.Add(new Currency(Name));
            }
        }

        public static double Get(string Name)//Возвращает значение валюты
        {
            if (ListOfCurrency.Count > 0)
            {
                int index = ListOfCurrency.FindIndex(x => x.Name == Name);
                if (index != -1) return ListOfCurrency[index].Value;
                else if (Name == MainCurrency) return 1;
                else return 0;
            }
            else return 0;
        }

        public static bool Find(string name)//Ищет нужную валюту
        {
            if (ListOfCurrency.Count > 0)
            {
                var found = ListOfCurrency.FindAll(p => p.Name == name);
                return found.Count > 0;
            }
            else
            {
                return false;
            }
        }

        public static async Task Refresh()//Обновляем значения всееех валют
        {
            if (ListOfCurrency.Count > 0)
            {
                string line = "";
                foreach (var p in ListOfCurrency)
                {
                    line += p.Name + ",";
                }
                string request = "https://min-api.cryptocompare.com/data/price?fsym=" + MainCurrency + "&tsyms=" + line;
                try
                {
                    string answer = await GetCurrencyValue(request);
                    JObject jObject = JObject.Parse(answer);
                    foreach (var p in ListOfCurrency)
                    {
                        if ((string)jObject[p.Name] != null)
                            p.Value = (double)jObject[p.Name];
                        else p.Value = 0;
                    }
                }
                catch
                {
                    foreach (var p in ListOfCurrency)
                    {
                        p.Value = 0;
                    }
                }
            }
        }

        public static async Task<string> GetCurrencyValue(string uri)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri(uri));
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
    public class Currency
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public Currency(string name)
        {
            Name = name.ToUpperInvariant();
            Value = 0;
        }
    }

    public class Options
    {
        public string MainCurrency { get; set; }
        public List<string> LastDocuments { get; set; }

        public Options()
        {
            LastDocuments = new List<string>();
            MainCurrency = "RUB";
        }

        public void Add(string a)
        {
            LastDocuments.Add(a);
            while (LastDocuments.Count > 5) LastDocuments.RemoveAt(0);
        }
    }

    public class ItemWallet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double BalCur { get; set; }
        public string Currency_B { get; set; }
        public double InvCur { get; set; }
        public string Currency_I { get; set; }
        public string Investment { get { return InvCur + " " + Currency_I; } }
        public string Cost { get { return BalCur / Price_B - InvCur / Price_I + " " + Currency_I; } }
        public string Balance { get { return BalCur + " " + Currency_B; } }
        public string TrueBalance { get { return BalCur / Price_B + " " + Currency_I; } }
        public double Price_I { get; set; }
        public double Price_B { get; set; }

        public void Edit(string name, string cur2, string cur1, double Cur2, double Cur1)
        {
            Name = name;
            InvCur = Cur2;
            Currency_I = cur2;
            BalCur = Cur1;
            Currency_B = cur1;
        }
    }
}