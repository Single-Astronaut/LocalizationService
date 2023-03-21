using LocalizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace LocalizationTests
{
    [TestClass]
    public class UnitTestLocalizationManager
    {
        private LocalizationManager _manager;

        [TestInitialize]
        public void Setup()
        {
            var assemblyWrapper = new AssemblyWrapper();
            var combinedReader = new CombinedResourceReader(assemblyWrapper);
            _manager = new LocalizationManager(assemblyWrapper);
        }

        [TestMethod]
        public void GetString_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string key = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _manager.GetString(key));
        }

        [TestMethod]
        public void GetString_EmptyKey_ThrowsArgumentNullException()
        {
            // Arrange
            string key = string.Empty;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _manager.GetString(key));
        }

        [TestMethod]
        public void GetString_KeyNotFound_ReturnsNull()
        {
            // Arrange
            string key = "non-existent-key";

            // Act
            var result = _manager.GetString(key);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetString_KeyFound_ReturnsLocalizedString()
        {
            // Arrange
            string key = "hello-world";
            var expected = "Hello, world!";

            // Act
            var result = _manager.GetString(key);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RegisterSource_AddsResourceReader()
        {
            var resxReader = new ResXResourceReader("path/to/resource/file.resx");
            var assemblyWrapper = new ResXResourceSet(resxReader.ToString());
            var combinedReader = new CombinedResourceReader((IAssemblyWrapper)assemblyWrapper);
            var manager = new LocalizationManager((IAssemblyWrapper)combinedReader);

            // Assert
            Assert.AreEqual(1, manager.GetCombinedReader().GetReaders().Count);
        }
    }
}
