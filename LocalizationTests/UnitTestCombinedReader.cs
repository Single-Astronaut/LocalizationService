using LocalizationService;
using System.Resources;
using Moq;
using System.Globalization;
using System.Reflection;
using System.Text;
using static LocalizationService.UnitTestCombinedReader;

namespace LocalizationService
{
    [TestClass]
    public class UnitTestCombinedReader
    {
        [TestMethod]
        public void TestAddReader()
        {
            var assemblyWrapper = new AssemblyWrapper();
            var mockReader1 = new Mock<IResourceReader>();
            var mockReader2 = new Mock<IResourceReader>();
            var combinedResourceReader = new CombinedResourceReader((LocalizationService.IAssemblyWrapper)assemblyWrapper);

            combinedResourceReader.AddReader(mockReader1.Object);
            combinedResourceReader.AddReader(mockReader2.Object);

            Assert.AreEqual(2, combinedResourceReader.GetReaders().Count);
        }

        [TestMethod]
        public void TestClose()
        {
            var assemblyWrapper = new AssemblyWrapper();
            var mockResourceReader1 = new Mock<IResourceReader>();
            var mockResourceReader2 = new Mock<IResourceReader>();
            var combinedResourceReader = new CombinedResourceReader((LocalizationService.IAssemblyWrapper)assemblyWrapper);

            combinedResourceReader.AddReader(mockResourceReader1.Object);
            combinedResourceReader.AddReader(mockResourceReader2.Object);

            combinedResourceReader.Close();

            mockResourceReader1.Verify(x => x.Close(), Times.Once);
            mockResourceReader2.Verify(x => x.Close(), Times.Once);
        }

        [TestMethod]
        public void TestDispose()
        {
            var assemblyWrapper = new AssemblyWrapper();
            var mockResourceReader1 = new Mock<IResourceReader>();
            var mockResourceReader2 = new Mock<IResourceReader>();
            var combinedResourceReader = new CombinedResourceReader((LocalizationService.IAssemblyWrapper)assemblyWrapper);

            combinedResourceReader.AddReader(mockResourceReader1.Object);
            combinedResourceReader.AddReader(mockResourceReader2.Object);

            combinedResourceReader.Dispose();

            mockResourceReader1.Verify(x => x.Dispose(), Times.Once);
            mockResourceReader2.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            var assemblyWrapper = new AssemblyWrapper();
            var mockResourceReader1 = new Mock<IResourceReader>();
            var mockResourceReader2 = new Mock<IResourceReader>();
            var combinedResourceReader = new CombinedResourceReader((LocalizationService.IAssemblyWrapper)assemblyWrapper);

            var mockDictionary1 = new Dictionary<object, object>()
    {
        { "key1", "value1" },
        { "key2", "value2" }
    };
            mockResourceReader1.Setup(x => x.GetEnumerator()).Returns(mockDictionary1.GetEnumerator());

            var mockDictionary2 = new Dictionary<object, object>()
    {
        { "key3", "value3" },
        { "key4", "value4" }
    };
            mockResourceReader2.Setup(x => x.GetEnumerator()).Returns(mockDictionary2.GetEnumerator());

            combinedResourceReader.AddReader(mockResourceReader1.Object);
            combinedResourceReader.AddReader(mockResourceReader2.Object);

            var enumerator = combinedResourceReader.GetEnumerator();

            var expectedDictionary = new Dictionary<object, object>()
    {
        { "key1", "value1" },
        { "key2", "value2" },
        { "key3", "value3" },
        { "key4", "value4" }
    };

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(expectedDictionary[enumerator.Key], enumerator.Value);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(expectedDictionary[enumerator.Key], enumerator.Value);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(expectedDictionary[enumerator.Key], enumerator.Value);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(expectedDictionary[enumerator.Key], enumerator.Value);

            Assert.IsFalse(enumerator.MoveNext());
        }

        public interface IAssemblyWrapper
        {
            Assembly GetExecutingAssembly();
        }

        public class AssemblyWrapper : IAssemblyWrapper
        {
            public Assembly GetExecutingAssembly() => Assembly.GetExecutingAssembly();
        }

        [TestMethod]
        public void TestGetResourceStream()
        {
            var mockResourceReader = new Mock<IResourceReader>();
            var mockAssemblyWrapper = new Mock<IAssemblyWrapper>();
            var combinedResourceReader = new CombinedResourceReader((LocalizationService.IAssemblyWrapper)mockAssemblyWrapper.Object);
            combinedResourceReader.AddReader(mockResourceReader.Object);
            mockResourceReader.Setup(x => x.GetEnumerator())
                              .Returns(new Dictionary<object, object> { { "TestResource", new object() } }.GetEnumerator());
            mockAssemblyWrapper.Setup(x => x.GetExecutingAssembly())
                               .Returns(typeof(CombinedResourceReader).Assembly);

            var result = combinedResourceReader.GetResourceStream(CultureInfo.InvariantCulture);

            Assert.IsNotNull(result);
        }
    }
}