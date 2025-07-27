using Xunit;
using GitExamHooksProject;

namespace GitExamHooksProject.Tests
{
    public class FeatureTests
    {
        [Fact]
        public void Square_ReturnsCorrectValue()
        {
            Assert.Equal(16, Program.Square(4));
        }
    }
}
