using LocalizationService;
using System.Globalization;

//Получение текущей культуры
CultureInfo currentCulture = CultureInfo.InvariantCulture;
var assemblyWrapper = new AssemblyWrapper();
var reader = new CombinedResourceReader(assemblyWrapper);
var manager = new LocalizationManager(assemblyWrapper);
manager.RegisterSource(reader);
var cultureInfo = new CultureInfo("en-US");
var str = manager.GetString("Hello World!", cultureInfo);
Console.WriteLine(str);