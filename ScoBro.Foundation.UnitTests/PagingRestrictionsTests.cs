using FluentAssertions;
using NUnit.Framework;

namespace ScoBro.Foundation.Tests {
    [TestFixture]
    public class PagingRestrictionsTests {
        [Test]
        public void Offset_Should_Return_Correct_Value_When_CurrentPage_Is_1() {
            // Arrange
            var restrictions = new PagingRestrictions(1, 10);

            // Act
            var offset = restrictions.GetOffset();

            // Assert
            offset.Should().Be(0);
        }

        [Test]
        public void Offset_Should_Return_Correct_Value_When_CurrentPage_Is_Greater_Than_1() {
            // Arrange
            var restrictions = new PagingRestrictions(2, 10);

            // Act
            var offset = restrictions.GetOffset();

            // Assert
            offset.Should().Be(10);
        }

        [Test]
        public void Offset_Should_Return_Correct_Value_When_CurrentPage_Is_Negative() {
            // Arrange
            var restrictions = new PagingRestrictions(-1, 10);

            // Act
            var offset = restrictions.GetOffset();

            // Assert
            offset.Should().Be(0);
        }

        [Test]
        public void Offset_Should_Return_Correct_Value_When_PageSize_Is_0() {
            // Arrange
            var restrictions = new PagingRestrictions(1, 0);

            // Act
            var offset = restrictions.GetOffset();

            // Assert
            offset.Should().Be(0);
        }
    }
}
