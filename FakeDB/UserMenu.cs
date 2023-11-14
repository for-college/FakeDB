using System;
using System.Collections.Generic;

namespace FakeDB
{
    public class UserMenu
    {
        private readonly FakeDatabase fakeDb;
        private readonly string username;

        public UserMenu(FakeDatabase fakeDb, string username)
        {
            this.fakeDb = fakeDb;
            this.username = username;
        }

        public void Display()
        {
            Console.WriteLine($"\nПривет, {username}!");

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить пользователя");
                Console.WriteLine("2. Удалить пользователя");
                Console.WriteLine("3. Просмотреть всех пользователей");
                Console.WriteLine("4. Выйти");

                Console.Write("Введите ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddUser();
                        break;

                    case "2":
                        RemoveUser();
                        break;

                    case "3":
                        DisplayAllUsers();
                        break;

                    case "4":
                        Console.WriteLine("Выход...");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        public void AddUser()
        {
            Console.Write("Логин: ");
            string newUser = Console.ReadLine();

            if (!fakeDb.ValidateUsername(newUser)) fakeDb.InvalidLogin();

            Console.Write("Пароль: ");
            string newPassword = Console.ReadLine();

            Console.Write("Возраст: ");
            string ageString = Console.ReadLine();

            if (!fakeDb.ValidateAge(ageString, out int newAge)) fakeDb.InvalidAge();

            fakeDb.AddUser(newUser, newPassword, newAge, UserRole.User);
            Console.WriteLine("Пользователь успешно добавлен!");
        }

        public void RemoveUser()
        {
            List<User> allUsers = fakeDb.GetAllUsers();

            if (allUsers.Count == 0)
            {
                Console.WriteLine("Нет зарегистрированных пользователей.");
                return;
            }

            Console.WriteLine("Список пользователей:");
            foreach (User user in allUsers)
            {
                Console.WriteLine($"Логин: {user.Username}, Возраст: {user.Age}, Роль: {user.Role}");
            }

            Console.Write("Введите логин пользователя для удаления: ");
            string usernameToRemove = Console.ReadLine();

            User currentUser = allUsers.Find(u => u.Username == username);

            if (currentUser == null)
            {
                Console.WriteLine("Текущий пользователь не найден.");
                return;
            }

            fakeDb.RemoveUser(usernameToRemove, currentUser);
        }

        public void DisplayAllUsers()
        {
            List<User> allUsers = fakeDb.GetAllUsers();
            foreach (var user in allUsers)
            {
                Console.WriteLine($"Логин: {user.Username}, Возраст: {user.Age}, Роль: {user.Role}");
            }
        }
    }
}
