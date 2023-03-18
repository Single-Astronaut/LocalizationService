using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationService
{
    public class CombinedResourceReader : IResourceReader
    {
        private readonly List<IResourceReader> _resourceReaders;

        public CombinedResourceReader(List<IResourceReader> resourceReaders)
        {
            _resourceReaders = resourceReaders;
        }

        public void Close()
        {
            foreach (var reader in _resourceReaders)
            {
                reader.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }

        public virtual IDictionaryEnumerator GetEnumerator()
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var reader in _resourceReaders)
            {
                foreach (DictionaryEntry entry in reader)
                {
                    string key = entry.Key.ToString();

                    if (dictionary.ContainsKey(key))
                    {
                        string value = entry.Value as string;

                        if (value != null)
                        {
                            dictionary.Add(key, value);
                        }
                    }
                }
            }
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
