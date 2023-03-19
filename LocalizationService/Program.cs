using LocalizationService;
using System.Globalization;

//Получение текущей культуры
CultureInfo currentCulture = CultureInfo.InvariantCulture;

var reader = new XmlResourceReader(@"C:\Resources");
var manager = new LocalizationManager();
manager.RegisterSource(reader);
var cultureInfo = new CultureInfo("en-US");
var str = manager.GetString("Greeting", cultureInfo);
Console.WriteLine(str);