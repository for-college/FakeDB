using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace FakeDB.Tests
{
    [TestClass()]
    public class FakeDatabaseTests
    {
        [TestMethod]
        public void AddUser_UserAddedSuccessfully()
        {
            /** Важно отметить, что AddUser не валидирует данные, этим занимаются другие методы - ValidateAge и ValidateUserName **/

            FakeDatabase fakeDb = new FakeDatabase();

            /** Тестовые данные **/
            string username = "testUser";
            string password = "password123";
            int age = 25;
            UserRole role = UserRole.User;

            /** Пытаемся добавить юзера **/
            fakeDb.AddUser(username, password, age, role);

            var allUsers = fakeDb.GetAllUsers();

            /** Сверяем данные **/
            Assert.IsTrue(allUsers.Count == 1);
            Assert.AreEqual(username, allUsers[0].Username);
            Assert.AreEqual(password, allUsers[0].Password);
            Assert.AreEqual(age, allUsers[0].Age);
            Assert.AreEqual(role, allUsers[0].Role);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsEmptyListInitially()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            List<User> allUsers = fakeDb.GetAllUsers();

            CollectionAssert.AreEqual(new List<User>(), allUsers);
        }

        /** Авторизация **/

        [TestMethod]
        public void AuthenticateUser_ValidUser_ReturnsTrue()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            fakeDb.AddUser("admin", "admin", 25, UserRole.User);

            bool isAuthenticated = fakeDb.AuthenticateUser("admin", "admin");

            Assert.IsTrue(isAuthenticated);
        }

        [TestMethod]
        public void AuthenticateUser_ValidUser_ReturnsFalse()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            fakeDb.AddUser("admin", "admin", 25, UserRole.User);

            bool isAuthenticated = fakeDb.AuthenticateUser("admin1", "admin");

            Assert.IsFalse(isAuthenticated);
        }

        /** Валидация **/

        [TestMethod]
        public void ValidateUsername_ValidUsername_ReturnsTrue()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            bool isValidUsername = fakeDb.ValidateUsername("admin1234");

            Assert.IsTrue(isValidUsername);
        }

        [TestMethod]
        public void ValidateUsername_InvalidUsername_ReturnsFalse()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            bool isValidUsername = fakeDb.ValidateUsername("invalid!Username");

            Assert.IsFalse(isValidUsername);
        }

        public void ValidateAge_ValidAge_ReturnsTrue()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            bool isValidAge = fakeDb.ValidateAge("1", out _);

            Assert.IsTrue(isValidAge);
        }

        [TestMethod]
        public void ValidateAge_InvalidAge_ReturnsFalse()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            bool isInvalidValidAge = fakeDb.ValidateAge("1asd", out _);

            Assert.IsFalse(isInvalidValidAge);
        }

        [TestMethod]
        public void LogOut_WithMessage_PrintsCorrectMessage()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            fakeDb.LogOut("Test message");

            string expectedMessage = "Test message" + Environment.NewLine;
            Assert.AreEqual(expectedMessage, consoleOutput.ToString());
        }

        [TestMethod]
        public void LogOut_WithoutMessage_PrintsNothing()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            fakeDb.LogOut();

            Assert.AreEqual("", consoleOutput.ToString());
        }

        [TestMethod]
        public void InvalidAge_PrintsCorrectMessage()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            Action invalidAgeAction = fakeDb.InvalidAge;
            invalidAgeAction.Invoke();

            string expectedMessage = "Возвраст не прошел валидацию. Введите целое число, которое больше 0." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, consoleOutput.ToString());
        }

        [TestMethod]
        public void InvalidLogin_PrintsCorrectMessage()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            Action invalidLoginAction = fakeDb.InvalidLogin;
            invalidLoginAction.Invoke();

            string expectedMessage = "Логин не прошел валидацию. Не вводите спец символы." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, consoleOutput.ToString());
        }

        /** **/
        [TestMethod]
        public void RemoveUser_AdminRemovesUser_Successfully()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            // Add an admin user
            fakeDb.AddUser("admin", "admin", 25, UserRole.Admin);

            // Add a user to be removed
            fakeDb.AddUser("userToRemove", "password123", 30, UserRole.User);

            // Authenticate as admin and try to remove the user
            fakeDb.RemoveUser("userToRemove", fakeDb.GetAllUsers()[0]);

            var allUsers = fakeDb.GetAllUsers();

            // Ensure that the user is removed successfully
            Assert.AreEqual(1, allUsers.Count);
            Assert.AreEqual("admin", allUsers[0].Username);
        }

        [TestMethod]
        public void RemoveUser_NonAdminCannotRemoveUser()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            fakeDb.AddUser("regularUser", "password123", 30, UserRole.User);

            fakeDb.RemoveUser("regularUser", fakeDb.GetAllUsers()[0]);

            var allUsers = fakeDb.GetAllUsers();

            Assert.AreEqual(1, allUsers.Count);
            Assert.AreEqual("regularUser", allUsers[0].Username);
        }

        [TestMethod]
        public void RemoveUser_UserNotFound()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            fakeDb.AddUser("admin", "admin", 25, UserRole.Admin);

            fakeDb.RemoveUser("nonExistingUser", fakeDb.GetAllUsers()[0]);

            var allUsers = fakeDb.GetAllUsers();

            Assert.AreEqual(1, allUsers.Count);
            Assert.AreEqual("admin", allUsers[0].Username);
        }

        [TestMethod]
        public void RemoveUser_AdminRemovesUser_SuccessMessage()
        {
            FakeDatabase fakeDb = new FakeDatabase();

            fakeDb.AddUser("admin", "admin", 25, UserRole.Admin);

            fakeDb.AddUser("userToRemove", "password123", 30, UserRole.User);

            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            fakeDb.RemoveUser("userToRemove", fakeDb.GetAllUsers()[0]);

            string expectedMessage = "Удаление прошло успешно." + Environment.NewLine;
            Assert.AreEqual(expectedMessage, consoleOutput.ToString());
        }
    }
}