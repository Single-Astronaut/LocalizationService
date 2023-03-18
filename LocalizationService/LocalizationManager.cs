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
        private readonly List<IResourceReader> _resourceReaders;

        public LocalizationManager()
        {
            _resourceReaders = new List<IResourceReader>();
        }

        public void RegisterSource(IResourceReader resourceReader)
        {
            _resourceReaders.Add(resourceReader);
        }

        public string GetString(string key, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            //ResourceSet resourceSet = new ResourceSet(new ResourceReader(_resourceReaders));
        }
    }
}
