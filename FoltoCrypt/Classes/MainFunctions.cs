﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace FoltoCrypt.Classes
{
    static class MainFunctions
    {
        public static string NameOptions = "options";

        static public void SaveWallets(List<ItemWallet> ItemList, string FileName)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ItemWallet[]));

            var json = JsonConvert.SerializeObject(ItemList, Formatting.Indented);

            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                // запись массива байтов в файл
                fs.Write(array, 0, array.Length);
            }

            /*using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                //jsonFormatter.WriteObject(fs, ItemList);
                fs.Write(json, 0, json.Length);
            }*/
        }

        static public List<ItemWallet> LoadWallets(string FileName)
        {
            List<ItemWallet> ItemList;
            using (FileStream fstream = File.OpenRead(FileName))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string json = System.Text.Encoding.Default.GetString(array);

                ItemList = JsonConvert.DeserializeObject<List<ItemWallet>>(json);
            }
            /*List<ItemWallet> ItemList;
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ItemWallet[]));

            using (FileStream fs = new FileStream(FileName + ".json", FileMode.OpenOrCreate))
            {
                ItemList = (List<ItemWallet>)jsonFormatter.ReadObject(fs);
            }*/
            return ItemList;
        }

        static public void SaveOptions(Options ItemList)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Options));

            using (FileStream fs = new FileStream(NameOptions, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, ItemList);
            }
        }

        static public bool LoadOptions(out Options opt)
        {
            Options ItemList = new Options();
            var ans = File.Exists(NameOptions);
            if (ans)
            {
                try
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Options));
                    using (FileStream fs = new FileStream(NameOptions, FileMode.OpenOrCreate))
                    {
                        ItemList = (Options)jsonFormatter.ReadObject(fs);
                    }
                }
                catch
                {
                    ans = false;
                }
            }
            opt = ItemList;
            return ans;
        }
    }

    public class ManagerOfCurrence
    {
        private static List<Currency> ListOfCurrency;
        public static string MainCurrency;

        public static void Start()
        {
            ListOfCurrency = new List<Currency>();
        }
        public static void SetCur(string mai)
        {
            MainCurrency = mai;
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
        public string MainCurrency;
        public double BTime;

        public Options()
        {
            MainCurrency = "USD";
            BTime = 0.1;
        }
        
        public void Set(string a, double b)
        {
            MainCurrency = a;
            BTime = b;
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
        public string Cost { get { return Math.Round((BalCur / Price_B - InvCur / Price_I)* Price_I, 8) + " " + Currency_I; } }
        public string Balance { get { return BalCur + " " + Currency_B; } }
        public string TrueBalance { get { return Math.Round((BalCur / Price_B) * Price_I, 8) + " " + Currency_I; } }
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