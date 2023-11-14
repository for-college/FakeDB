using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        [TestMethod]
        public void AuthenticateUser_ValidUser_ReturnsTrue()
        {
            FakeDatabase fakeDb = new FakeDatabase();
            fakeDb.AddUser("admin", "admin", 25, UserRole.User);

            bool isAuthenticated = fakeDb.AuthenticateUser("admin", "admin");

            Assert.IsTrue(isAuthenticated);
        }

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

    }
}