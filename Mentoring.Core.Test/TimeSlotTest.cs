using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class TimeSlotTest
    {
        [TestMethod]
        public void ConstructorShouldInitializeMentorsCollection()
        {
            // Arrange
            TimeSlot timeSlot;

            // Act 
            timeSlot = new TimeSlot();

            // Assert
            Assert.IsNotNull(timeSlot.Mentors);
            Assert.AreEqual(0, timeSlot.Mentors.Count);
        }
    }
}