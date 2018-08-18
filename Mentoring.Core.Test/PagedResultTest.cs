using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class PagedResultTest
    {
        [TestMethod]
        public void ConstructorShouldSetProperties()
        {
            // Arrange
            var list = new List<string> { "how", "are", "you" };
            var totalCount = 20;
            PagedResult<string> pagedResult;

            // Act
            pagedResult = new PagedResult<string>(list, totalCount);

            // Assert
            Assert.IsNotNull(pagedResult.Items);
            Assert.AreEqual(3, pagedResult.Items.Count());
            Assert.AreEqual(totalCount, pagedResult.TotalCount);
        }
    }
}
