using GitExamHooksProject;
using Xunit;

namespace GitExamHooksProject.Tests
{
    public class FeatureTests
    {
        [Fact]
        public void Square_ReturnsCorrectValue()
        {
            Assert.Equal(17, Program.Square(4));
        }
    }
}
