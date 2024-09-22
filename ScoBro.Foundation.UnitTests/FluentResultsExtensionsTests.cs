using FluentResults;

namespace ScoBro.Foundation.Tests {
    public class FluentResultsExtensionsTests {
        [Test]
        public void ToSimpleResult_ShouldConvertResultToSimpleResult() {
            // Arrange
            var result = Result.Ok("Success");

            // Act
            var simpleResult = result.ToSimpleResult();

            Assert.Multiple(() => {
                // Assert
                Assert.That(simpleResult.Value, Is.EqualTo("Success"));
                Assert.That(simpleResult.WasSuccessful, Is.True);
                Assert.That(simpleResult.Errors, Is.Empty);
            });
        }

        [Test]
        public void ToSimpleResult_ShouldConvertFailedResultToSimpleResultWithErrors() {
            // Arrange
            var result = Result.Fail("Error");

            // Act
            var simpleResult = result.ToSimpleResult();

            Assert.Multiple(() => {
                // Assert
                Assert.That(simpleResult.WasSuccessful, Is.False);
                Assert.That(simpleResult.Errors.Count(), Is.EqualTo(1));
                Assert.That(simpleResult.Errors.First(), Is.EqualTo("Error"));
            });
        }
    }
}
