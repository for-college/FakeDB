using System;

namespace FakeDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FakeDatabase fakeDb = new FakeDatabase();

            fakeDb.AddUser("admin", "admin", 30, UserRole.Admin);
            fakeDb.AddUser("test1", "test2", 30, UserRole.User);
            fakeDb.AddUser("test3", "test2", 30, UserRole.User);
            fakeDb.AddUser("test4", "admin", 30, UserRole.User);
            fakeDb.AddUser("test5", "admin", 30, UserRole.User);

            Console.WriteLine("Добро пожаловать!");

            Console.Write("Ваш логин: ");
            string username = Console.ReadLine();

            if (!fakeDb.ValidateUsername(username)) fakeDb.InvalidLogin();

            Console.Write("Ваш пароль: ");
            string password = Console.ReadLine();

            if (!fakeDb.AuthenticateUser(username, password)) fakeDb.LogOut("Что-то пошло не так...");

            Console.WriteLine("Вы в системе!");

            UserMenu userMenu = new UserMenu(fakeDb, username);
            userMenu.Display();


            Console.ReadLine();
        }
    }
}
