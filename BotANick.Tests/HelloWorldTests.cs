using FluentAssertions;
using Xunit;

namespace BotANick.Tests
{
    public class HelloWorldTests
    {
        [Fact]
        public void SayHelloWorld_ShouldReturnHelloWorld()
        {
            // Arrange
            var sut = "HelloWorld";

            // Act
            var actual = sut;

            // Assert
            actual.Should().Be("HelloWorld");
        }
    }
}
