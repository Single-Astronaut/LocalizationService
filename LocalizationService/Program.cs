using LocalizationService;
using System.Globalization;

//Получение текущей культуры
CultureInfo currentCulture = CultureInfo.InvariantCulture;

var reader = new CombinedResourceReader();
var manager = new LocalizationManager();
manager.RegisterSource(reader);
var cultureInfo = new CultureInfo("en-US");
var str = manager.GetString("Hello World!", cultureInfo);
Console.WriteLine(str);