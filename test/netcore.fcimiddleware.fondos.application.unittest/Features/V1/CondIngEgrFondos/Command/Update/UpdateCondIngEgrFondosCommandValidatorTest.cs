using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.CondIngEgrFondos.Command.Update
{
    public class UpdateCondIngEgrFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateCondIngEgrFondosCommand>()
                .Create();
            var validator = new UpdateCondIngEgrFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
