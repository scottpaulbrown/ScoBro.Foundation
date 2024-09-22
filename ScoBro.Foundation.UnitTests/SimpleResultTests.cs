
namespace ScoBro.Foundation.Tests {
    public class SimpleResultTests {
        [Test]
        public void SimpleResult_Constructor_WasSuccessful_True() {
            // Arrange
            bool wasSuccessful = true;
            IEnumerable<string> errors = new List<string> { "Error 1", "Error 2" };

            // Act
            SimpleResult result = new SimpleResult(wasSuccessful, errors);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result.WasSuccessful, Is.True);
                Assert.That(result.Errors, Is.EqualTo(errors));
            });
        }

        [Test]
        public void SimpleResult_Constructor_WasSuccessful_False() {
            // Arrange
            bool wasSuccessful = false;
            IEnumerable<string> errors = new List<string> { "Error 1", "Error 2" };

            // Act
            SimpleResult result = new SimpleResult(wasSuccessful, errors);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result.WasSuccessful, Is.False);
                Assert.That(result.Errors, Is.EqualTo(errors));
            });
        }

        [Test]
        public void SimpleResult_Constructor_Errors_Null() {
            // Arrange
            bool wasSuccessful = true;

            // Act
            SimpleResult result = new SimpleResult(wasSuccessful);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result.WasSuccessful, Is.True);
                Assert.That(result.Errors, Is.Empty);
            });
        }

        [Test]
        public void SimpleResult_Ok() {
            // Arrange
            int value = 42;

            // Act
            SimpleResult<int> result = SimpleResult<int>.Ok(value);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result.WasSuccessful, Is.True);
                Assert.That(result.Value, Is.EqualTo(value));
                Assert.That(result.Errors, Is.Empty);
            });
        }

        [Test]
        public void SimpleResult_Fail() {
            // Act
            SimpleResult<int> result = SimpleResult.Fail<int>("It Failed");

            Assert.Multiple(() => {
                // Assert
                Assert.That(result.WasSuccessful, Is.False);
                Assert.That(result.Value, Is.EqualTo(default(int)));
                Assert.That(result.Errors.Count(), Is.EqualTo(1));
                Assert.That(result.Errors.First(), Is.EqualTo("It Failed"));
            });
        }
    }
}
