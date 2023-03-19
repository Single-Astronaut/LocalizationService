using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationService
{
    //public class CombinedResourceReader : IResourceReader
    //{
    //    private List<IResourceReader> _readers = new List<IResourceReader>();

    //    public CombinedResourceReader(params IResourceReader[] readers)
    //    {
    //        _readers.AddRange(readers);
    //    }

    //    public void AddReader(IResourceReader reader)
    //    {
    //        _readers.Add(reader);
    //    }

    //    public void Close()
    //    {
    //        foreach (var reader in _readers)
    //        {
    //            reader.Close();
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        Close();
    //    }

    //    //public IDictionaryEnumerator GetEnumerator()
    //    //{
    //    //    Dictionary<object, object> combinedDictionary = new Dictionary<object, object>();

    //    //    foreach (IResourceReader reader in _readers)
    //    //    {
    //    //        IDictionaryEnumerator enumerator = reader.GetEnumerator();

    //    //        while (enumerator.MoveNext())
    //    //        {
    //    //            if (!combinedDictionary.Contains(enumerator.Key))
    //    //            {
    //    //                combinedDictionary.Add(enumerator.Key, enumerator.Value);
    //    //            }
    //    //        }

    //    //        enumerator.Reset();
    //    //    }

    //    //    return combinedDictionary.GetEnumerator();
    //    //}

    //    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    //    {
    //        var combinedDictionary = new Dictionary<string, object>();
    //        foreach (var reader in _readers)
    //        {
    //            var enumerator = reader.GetEnumerator();
    //            while (enumerator.MoveNext())
    //            {
    //                combinedDictionary[(string)enumerator.Key] = enumerator.Value;
    //            }
    //        }
    //        return combinedDictionary.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}

    public class CombinedResourceReader : IResourceReader
    {
        private readonly List<IResourceReader> _readers;

        public CombinedResourceReader() 
        {
            _readers = new List<IResourceReader>();
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

        public virtual IDictionaryEnumerator GetEnumerator()
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
