using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.AgColocadores.Command.Create
{
    public class CreateAgColocadoresCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateAgColocadoresCommand>()
                .Create();
            var validator = new CreateAgColocadoresCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
