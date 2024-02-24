using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Fondos.Command.Create
{
    public class CreateFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateFondosCommand>()
                .Create();
            var validator = new CreateFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
