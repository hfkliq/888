using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FinancialSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Добро пожаловать в систему управления финансовыми потоками предприятия!");

            // Проверяем наличие файлов пользователей
            if (!File.Exists("Users.json"))
            {
                Console.WriteLine("Файл пользователей не найден. Создаётся файл администратора...");
                CreateAdminAccount();
            }

            // Запуск авторизации
            AuthorizeUser();
        }

        static void CreateAdminAccount()
        {
            var admin = new User
            {
                Role = "Администратор",
                FullName = "Системный Администратор",
                Login = "admin",
                Password = "Admin123!@#",
                BirthDate = new DateTime(1990, 1, 1)
            };

            SaveUser(admin);
            Console.WriteLine("Аккаунт администратора успешно создан. Логин: admin, Пароль: Admin123!@#");
        }

        static void SaveUser(User user)
        {
            var users = LoadUsers();
            users.Add(user);
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("Users.json", json);
        }

        static List<User> LoadUsers()
        {
            if (!File.Exists("Users.json")) return new List<User>();

            string json = File.ReadAllText("Users.json");
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        static void AuthorizeUser()
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();

            Console.WriteLine("Введите пароль:");
            string password = ReadPassword();

            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                Console.WriteLine($"Добро пожаловать, {user.FullName}! Ваша роль: {user.Role}");
                LaunchModule(user);
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
                AuthorizeUser();
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        static void LaunchModule(User user)
        {
            switch (user.Role)
            {
                case "Администратор":
                    AdminModule();
                    break;
                // Добавить вызовы других модулей для остальных ролей
                default:
                    Console.WriteLine("Модуль для вашей роли находится в разработке.");
                    break;
            }
        }

        static void AdminModule()
        {
            Console.WriteLine("Вы вошли в модуль Администратора.");
            Console.WriteLine("1. Просмотр пользователей\n2. Добавить пользователя\n3. Удалить пользователя\n4. Выйти");
            // Реализация функций модуля администратора будет здесь
        }
    }

    [Serializable]
    class User
    {
        public string Role { get; set; }  // Роль пользователя
        public string FullName { get; set; }  // ФИО
        public string Login { get; set; }  // Логин
        public string Password { get; set; }  // Пароль
        public DateTime BirthDate { get; set; }  // Дата рождения
    }
}
