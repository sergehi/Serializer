//Рефлексия и её применение
//Цель:
//  Написать свой класс-сериализатор данных любого типа в формат CSV, сравнение его быстродействия с типовыми механизмами серализации.
//  Полезно для изучения возможностей Reflection, а может и для применения данного класса в будущем.

//Описание/Пошаговая инструкция выполнения домашнего задания:

//Основное задание:

//1.Написать сериализацию свойств или полей класса в строку

//2. Проверить на классе: class F { int i1, i2, i3, i4, i5; Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; }

//3.Замерить время до и после вызова функции (для большей точности можно сериализацию сделать в цикле 100-100000 раз)

//4. Вывести в консоль полученную строку и разницу времен

//5. Отправить в чат полученное время с указанием среды разработки и количества итераций

//6. Замерить время еще раз и вывести в консоль сколько потребовалось времени на вывод текста в консоль

//7. Провести сериализацию с помощью каких-нибудь стандартных механизмов (например в JSON)

//8. И тоже посчитать время и прислать результат сравнения

//9. Написать десериализацию/загрузку данных из строки (ini/csv-файла) в экземпляр любого класса

//10. Замерить время на десериализацию

//11. Общий результат прислать в чат с преподавателем в системе в таком виде:

//Сериализуемый класс: class F { int i1, i2, i3, i4, i5; }

//код сериализации-десериализации: ...

//количество замеров: 1000 итераций

//мой рефлекшен:

//Время на сериализацию = 100 мс

//Время на десериализацию = 100 мс

//стандартный механизм (NewtonsoftJson):

//Время на сериализацию = 100 мс

//Время на десериализацию = 100 мс


using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using Serializer;

internal class Program
{
    private static void Main(string[] args)
    {
        // "Самописная" сериализация в строку
        ICustomSerializator iMySerializator = new MySerializator();
        // "Самописная" сериализация в строку с кэшем для десериализации
        ICustomSerializator iMyCachedSerializator = new MyCachedSerializator();
        // сериализация/десериализация с помощью System.Text.Json.JsonSerializer
        ICustomSerializator iJSONSerializator = new JSONSerializator();
        

        int iterCount = 1000000;
        string? my_string = null;
        string? json_string = null;

        Stopwatch stopWatch = new Stopwatch();
        Console.WriteLine($"0,000 сек.\tСериализация в строку начата (количество повторений: {iterCount}) ");
        stopWatch.Start();
        for (int i = 0; i < iterCount; i++)
             my_string = iMySerializator.Serialize<SampleType>(SampleType.Get());
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds} сек.\tСериализация закончена.");
        Console.WriteLine($"Сериализованный объект: {my_string}");
        
        Console.WriteLine($"0,000 сек.\tСериализация в JSON начата (количество повторений: {iterCount}) ");
        stopWatch.Restart();
        for (int i = 0; i < iterCount; i++)
            json_string = iJSONSerializator.Serialize<SampleType>(SampleType.Get());
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds} сек.\tСериализация закончена.");
        Console.WriteLine($"Сериализованный объект: {json_string}");

        Console.WriteLine("");
        Console.WriteLine($"0,000 сек.\tДесериализация из строки начата (количество повторений: {iterCount}) ");
        stopWatch.Restart();
        SampleType? my_object = null;
        for (int i = 0; i < iterCount; i++)
            my_object = iMySerializator.Deserialize<SampleType>(my_string ?? "");
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds} сек.\tДесериализация закончена.");
        Console.WriteLine($"Десериализованный объект: {my_object?.ToString()}");

        Console.WriteLine("");
        Console.WriteLine($"0,000 сек.\tДесериализация из строки с использованием кэширования начата (количество повторений: {iterCount}) ");
        stopWatch.Restart();
        my_object = null;
        for (int i = 0; i < iterCount; i++)
            my_object = iMyCachedSerializator.Deserialize<SampleType>(my_string ?? "");
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds} сек.\tДесериализация закончена.");
        Console.WriteLine($"Десериализованный объект: {my_object?.ToString()}");

        Console.WriteLine($"0,000 сек.\tДесериализация из JSON начата (количество повторений: {iterCount}) ");
        stopWatch.Restart();

        for (int i = 0; i < iterCount; i++)
            my_object = iJSONSerializator.Deserialize<SampleType>(json_string ?? "");
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds} сек.\tДесериализация закончена.");
        Console.WriteLine($"Десериализованный объект: {my_object?.ToString()}");


        Console.WriteLine("Enter для выхода из программы...");
        Console.ReadLine();
    }
}