using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class TopicTest
    {
        [TestMethod]
        public void ConstructorShouldInitializeMentorsCollection()
        {
            // Arrange
            Topic topic;

            // Act 
            topic = new Topic();

            // Assert
            Assert.IsNotNull(topic.Mentors);
            Assert.AreEqual(0, topic.Mentors.Count);
        }
    }
}