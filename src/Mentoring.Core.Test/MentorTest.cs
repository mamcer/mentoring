using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class MentorTest
    {
        [TestMethod]
        public void ConstructorShouldInitializeCollections()
        {
            // Arrange
            Mentor mentor;

            // Act 
            mentor = new Mentor();

            // Assert
            Assert.IsNotNull(mentor.Topic);
            Assert.IsNotNull(mentor.Availability);
            Assert.IsNotNull(mentor.Mentees);
            Assert.IsNotNull(mentor.MenteeSeniorities);
            Assert.AreEqual(0, mentor.Topic.Count);
            Assert.AreEqual(0, mentor.Availability.Count);
            Assert.AreEqual(0, mentor.Mentees.Count);
            Assert.AreEqual(0, mentor.MenteeSeniorities.Count);
        }
    }
}