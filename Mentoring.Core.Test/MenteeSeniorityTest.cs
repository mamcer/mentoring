using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class MenteeSeniorityTest
    {
        [TestMethod]
        public void ConstructorShouldInitializeMentorsCollection()
        {
            // Arrange
            MenteeSeniority menteeSeniority;

            // Act 
            menteeSeniority = new MenteeSeniority();

            // Assert
            Assert.IsNotNull(menteeSeniority.Mentors);
            Assert.AreEqual(0, menteeSeniority.Mentors.Count);
        }
    }
}