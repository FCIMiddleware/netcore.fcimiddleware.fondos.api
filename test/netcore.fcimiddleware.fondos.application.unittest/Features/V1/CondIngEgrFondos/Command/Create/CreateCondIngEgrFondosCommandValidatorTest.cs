using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.CondIngEgrFondos.Command.Create
{
    public class CreateCondIngEgrFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateCondIngEgrFondosCommand>()
                .Create();
            var validator = new CreateCondIngEgrFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
