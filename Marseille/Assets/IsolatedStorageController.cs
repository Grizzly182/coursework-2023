using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Input;

namespace Marseille.Assets
{
    internal class IsolatedStorageController
    {
        private const string _defaultIsolatedStorageFilePath = "storage";

        public static bool IsolatedStorageEnabled { get; set; }

        public static void Save(Dictionary<string, string> values)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();

            try
            {
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(_defaultIsolatedStorageFilePath, FileMode.OpenOrCreate, storage);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (var item in values)
                    {
                        writer.WriteLineAsync($"{item.Key}:{item.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Возникла непредвиденная ошибка: " + ex.Message);
            }
        }

        // TODO: Comment this later
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Read(string key)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();

            try
            {
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(_defaultIsolatedStorageFilePath, FileMode.OpenOrCreate, storage);
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] keyAndValue = reader.ReadLine().Split(':');

                        if (keyAndValue[0].Contains(key)) // keyAndValue[0] is key and [1] is a value of the key
                        {
                            return keyAndValue[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Возникла непредвиденная ошибка: " + ex.Message);
            }
            return null;
        }

        public static void Remove()
        {
            if (IsolatedStorageEnabled)
            {
            }
        }

        public static void Clear()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            try
            {
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(_defaultIsolatedStorageFilePath, FileMode.Truncate, storage);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLineAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Возникла непредвиденная ошибка: " + ex.Message);
            }
        }
    }
}