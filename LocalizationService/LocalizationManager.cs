using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;

namespace LocalizationService
{
    public class LocalizationManager
    {
        private readonly CombinedResourceReader _combinedReader;
        private readonly ResourceSet _resourceSet;

        public LocalizationManager()
        {
            _combinedReader = new CombinedResourceReader();
            _resourceSet = new ResourceSet(_combinedReader);

        }

        public void RegisterSource(CombinedResourceReader resourceReader)
        {
            _combinedReader.AddReader(resourceReader);
        }

        public string GetString(string key, CultureInfo? cultureInfo = null)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Код не должен быть пустым или равным null");
            }

            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentCulture;
            }

            ResourceSet resourceSet = new ResourceSet(_combinedReader.GetResourceStream(cultureInfo));

            string localizedString = resourceSet.GetString(key);
            return localizedString;
        }
    }
}
