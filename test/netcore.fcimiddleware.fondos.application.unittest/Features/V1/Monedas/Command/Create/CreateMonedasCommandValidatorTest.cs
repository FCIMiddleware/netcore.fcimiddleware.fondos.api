using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Command.Create
{
    public class CreateMonedasCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateMonedasCommand>()
                .Create();
            var validator = new CreateMonedasCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
