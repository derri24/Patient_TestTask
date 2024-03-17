using System;
using System.Collections.Generic;
using Patient.TestTask.ConsoleApp.Models;

namespace Patient.TestTask.ConsoleApp;

public static class PatientGenerator
{
    private const int PatientCount = 100;

    private static Random _random;

    static PatientGenerator()
    {
        _random = new Random();
    }

    private static List<string> _names = new()
    {
        "Александр", "Михаил", "Иван", "Дмитрий", "Артём", "Максим", "Андрей",
        "Кирилл", "Сергей", "Николай", "Алексей", "Егор", "Владимир", "Илья",
        "Павел", "Станислав", "Глеб", "Тимофей", "Даниил", "Владислав", "Роман", "Олег",
        "Ярослав", "Василий", "Виктор"
    };

    private static List<string> _surnames = new()
    {
        "Иванов", "Смирнов", "Кузнецов", "Попов", "Соколов", "Лебедев", "Козлов",
        "Новиков", "Морозов", "Петров", "Волков", "Соловьёв", "Васильев", "Зайцев",
        "Павлов", "Семёнов", "Голубев", "Виноградов", "Богданов", "Воробьёв",
        "Фёдоров", "Михайлов", "Беляев", "Тарасов", "Белов"
    };

    private static List<string> _patronymics = new()
    {
        "Александрович", "Иванович", "Дмитриевич", "Артёмович", "Михайлович", "Кириллович",
        "Сергеевич", "Николаевич", "Алексеевич", "Егорович", "Владимирович", "Ильич", "Павлович",
        "Станиславович", "Глебович", "Тимофеевич", "Даниилович", "Владиславович", "Романович",
        "Олегович", "Ярославович", "Васильевич", "Викторович"
    };

    private static List<string> _use = new()
    {
        "official", "not-official",
    };

    private static List<bool> _activate = new()
    {
        true, false
    };

    private static List<string> _genders = new()
    {
        "male", "female", "other", "unknown"
    };

    public static List<PatientModel> Generate()
    {
        var patients = new List<PatientModel>();

        for (int i = 0; i < PatientCount; i++)
        {
            var patient = new PatientModel()
            {
                Gender = _genders[_random.Next(0, 4)],
                Active = _activate[_random.Next(0, 2)],
                BirthDate = new DateTime(_random.Next(2000, 2024), _random.Next(1, 13), _random.Next(1, 28)),
                Name = new NameModel()
                {
                    Family = _surnames[_random.Next(0, _surnames.Count)],
                    Use = _use[_random.Next(0, 2)],
                    Given = new List<string>()
                    {
                        _names[_random.Next(0, _names.Count)],
                        _patronymics[_random.Next(0, _patronymics.Count)],
                    },
                }
            };

            patients.Add(patient);
        }

        return patients;
    }
}