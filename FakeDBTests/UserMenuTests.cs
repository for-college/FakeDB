using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace FakeDB.Tests
{
    /**
     * Тесты для UserMenuTests местами сложные, поэтому тут много комментов
    **/

    [TestClass()]
    public class UserMenuTests
    {
        [TestMethod]
        public void AddUser_ValidInput_UserAddedSuccessfully()
        {
            // Создаем фейковую базу данных для тестирования
            FakeDatabase fakeDb = new FakeDatabase();
            string username = "admin";
            UserMenu userMenu = new UserMenu(fakeDb, username);

            // Используем StringWriter для перехвата вывода в консоль
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Используем StringReader для предоставления ввода в консоль
                using (StringReader sr = new StringReader("TestUser\nTestPassword\n25\n"))
                {
                    Console.SetIn(sr);

                    // Вызываем метод AddUser(), который читает ввод из консоли
                    userMenu.AddUser();

                    // Получаем все пользователи из фейковой базы данных
                    List<User> allUsers = fakeDb.GetAllUsers();

                    // Проверяем, что был добавлен один пользователь с правильными данными
                    Assert.AreEqual(1, allUsers.Count);
                    Assert.AreEqual("TestUser", allUsers[0].Username);
                    Assert.AreEqual("TestPassword", allUsers[0].Password);
                    Assert.AreEqual(25, allUsers[0].Age);
                    Assert.AreEqual(UserRole.User, allUsers[0].Role);

                    // Проверяем, что вывод в консоль совпадает с ожидаемым
                    string expectedOutput = "Логин: Пароль: Возраст: Пользователь успешно добавлен!\n";
                    Assert.AreEqual(expectedOutput.Trim(), sw.ToString().Trim());
                }
            }
        }

        [TestMethod]
        public void RemoveUser_UserExists_UserRemovedSuccessfully()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            string username = "admin";
            UserMenu userMenu = new UserMenu(fakeDb, username);

            // Добавляем пользователя для тестирования
            fakeDb.AddUser("TestUser", "TestPassword", 25, UserRole.User);

            // Используем StringReader для предоставления ввода в консоль
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Предоставляем ввод для теста, указывая пользователя для удаления
                using (StringReader sr = new StringReader("TestUser"))
                {
                    Console.SetIn(sr);

                    userMenu.RemoveUser();

                    // Получаем всех пользователей из фейковой базы данных
                    var allUsers = fakeDb.GetAllUsers();

                    // Проверяем, что пользователь был успешно удален
                    Assert.AreEqual(1, allUsers.Count);

                }
            }
        }

        [TestMethod]
        public void RemoveUser_NoUsers_NothingRemoved()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            string username = "admin";
            UserMenu userMenu = new UserMenu(fakeDb, username);

            // Используем StringReader для предоставления ввода в консоль
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Предоставляем ввод для теста, но не добавляем пользователей

                userMenu.RemoveUser();

                // Получаем всех пользователей из фейковой базы данных
                var allUsers = fakeDb.GetAllUsers();

                // Проверяем, что ни один пользователь не был удален
                Assert.AreEqual(0, allUsers.Count);

                // Проверяем, что вывод в консоль соответствует ожидаемому
                string expectedOutput = "Нет зарегистрированных пользователей.";
                Assert.AreEqual(expectedOutput.Trim(), sw.ToString().Trim());
            }
        }

        [TestMethod]
        public void DisplayAllUsers_UsersExist_DisplayedSuccessfully()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            string username = "admin";
            UserMenu userMenu = new UserMenu(fakeDb, username);

            // Добавляем пользователей для тестирования
            fakeDb.AddUser("TestUser1", "TestPassword1", 25, UserRole.User);
            fakeDb.AddUser("TestUser2", "TestPassword2", 30, UserRole.Admin);

            // Используем StringWriter для перехвата вывода в консоль
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                userMenu.DisplayAllUsers();

                // Проверяем, что вывод в консоль соответствует ожидаемому
                string expectedOutput = $"Логин: TestUser1, Возраст: 25, Роль: User{Environment.NewLine}Логин: TestUser2, Возраст: 30, Роль: Admin";
                Assert.AreEqual(expectedOutput.Trim(), sw.ToString().Trim());
            }
        }
    }
}
