using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FakeDB
{
    public class FakeDatabase
    {
        private readonly List<User> users = new List<User>();

        public void AddUser(string username, string password, int age, UserRole role)
        {
            users.Add(new User { Username = username, Password = password, Age = age, Role = role });
        }

        public void RemoveUser(string usernameToRemove, User currentUser)
        {
            if (currentUser.Role != UserRole.Admin)
            {
                Console.WriteLine("Недостаточно прав для удаления пользователя.");
            }

            User userToRemove = users.Find(u => u.Username == usernameToRemove);

            if (userToRemove == null)
            {
                Console.WriteLine("Пользователь не найден.");
            }

            users.Remove(userToRemove);
            Console.WriteLine("Удаление прошло успешно.");
        }

        public List<User> GetAllUsers() => users;

        public bool AuthenticateUser(string username, string password) => users.Exists(u => u.Username == username && u.Password == password);

        public bool ValidateUsername(string username) => Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");

        public bool ValidateAge(string ageString, out int age) => int.TryParse(ageString, out age) && age >= 1;
        public void InvalidAge()
        {
            Console.WriteLine("Возвраст не прошел валидацию. Введите целое число, которое больше 0.");

            LogOut();
        }
        public void InvalidLogin()
        {
            Console.WriteLine("Логин не прошел валидацию. Не вводите спец символы.");

            LogOut();
        }

        public void LogOut(string message = null)
        {
            if (message != null) Console.WriteLine(message);

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
