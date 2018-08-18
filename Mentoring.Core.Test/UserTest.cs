using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void ConstructorShouldInitializeRolesCollection()
        {
            // Arrange
            User user;

            // Act 
            user = new User();

            // Assert
            Assert.IsNotNull(user.Roles);
            Assert.AreEqual(0, user.Roles.Count);
        }
    }
}