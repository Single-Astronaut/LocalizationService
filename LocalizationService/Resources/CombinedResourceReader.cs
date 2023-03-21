using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationService
{
    public interface IAssemblyWrapper
    {
        Assembly GetExecutingAssembly();
    }

    public class AssemblyWrapper : IAssemblyWrapper
    {
        public Assembly GetExecutingAssembly() => Assembly.GetExecutingAssembly();
    }

    public class CombinedResourceReader : IResourceReader
    {
        private readonly List<IResourceReader> _readers;
        private readonly IAssemblyWrapper _assemblyWrapper;

        public CombinedResourceReader(IAssemblyWrapper assemblyWrapper)
        {
            _readers = new List<IResourceReader>();
            _assemblyWrapper = assemblyWrapper;
        }

        public List<IResourceReader> GetReaders()
        {
            return _readers;
        }

        public void AddReader(IResourceReader reader)
        {
            _readers.Add(reader);
        }

        public void Close()
        {
            foreach (var reader in _readers)
            {
                reader.Close();
            }
        }

        public void Dispose()
        {
            foreach (var reader in _readers)
            {
                reader.Dispose();
            }
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var combinedDictionary = new Dictionary<object, object>();

            foreach (var reader in _readers)
            {
                IDictionaryEnumerator enumerator = reader.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    combinedDictionary[enumerator.Key] = enumerator.Value;
                }

                enumerator.Reset();
            }

            return new DictionaryEnumerator(combinedDictionary);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Stream GetResourceStream(CultureInfo cultureInfo)
        {
            foreach (var reader in _readers)
            {
                if (reader is IResourceReader resourceReader)
                {
                    var resourceSet = new ResourceSet(resourceReader);

                    var enumerator = resourceSet.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        var resourceKey = enumerator.Key.ToString();

                        var assembly = _assemblyWrapper.GetExecutingAssembly(); 

                        var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceKey}");

                        if (resourceStream != null)
                        {
                            return resourceStream;
                        }
                    }
                }
            }

            return null;
        }
    }

    public class DictionaryEnumerator : IDictionaryEnumerator
    {
        private readonly IEnumerator<KeyValuePair<object, object>> _enumerator;

        public DictionaryEnumerator(Dictionary<object, object> dictionary)
        {
            _enumerator = dictionary.GetEnumerator();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public object Current => Entry;

        public DictionaryEntry Entry => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);

        public object Key => _enumerator.Current.Key;

        public object Value => _enumerator.Current.Value;
    }
}
