using LocalizationService;
using System.Globalization;

////Получение текущей культуры
//CultureInfo currentCulture = CultureInfo.InvariantCulture;
//var assemblyWrapper = new AssemblyWrapper();
//var reader = new CombinedResourceReader(assemblyWrapper);
//var manager = new LocalizationManager(assemblyWrapper);
//manager.RegisterSource(reader);
//var cultureInfo = new CultureInfo("en-US");
//var str = manager.GetString("Hello World!", cultureInfo);
//Console.WriteLine(str);

CultureInfo currentCulture = CultureInfo.InvariantCulture;
var assemblyWrapper = new AssemblyWrapper();
var reader = new CombinedResourceReader(assemblyWrapper);
var manager = new LocalizationManager(assemblyWrapper);
manager.RegisterSource(reader);

// Проверка наличия ресурса в менеджере локализации
if (manager.Contains("Hello World!"))
{
    Console.WriteLine("Ресурс 'Hello World!' был успешно добавлен в менеджер локализации.");
}
else
{
    Console.WriteLine("Ресурс 'Hello World!' не был добавлен в менеджер локализации.");
}

// Получение значения ресурса для заданной культуры
var cultureInfo = new CultureInfo("en-US");
var str = manager.GetString("Hello World!", cultureInfo);
if (str == "Hello World!")
{
    Console.WriteLine("Значение ресурса для культуры 'en-US' было успешно получено.");
}
else
{
    Console.WriteLine("Значение ресурса для культуры 'en-US' не было получено.");
}