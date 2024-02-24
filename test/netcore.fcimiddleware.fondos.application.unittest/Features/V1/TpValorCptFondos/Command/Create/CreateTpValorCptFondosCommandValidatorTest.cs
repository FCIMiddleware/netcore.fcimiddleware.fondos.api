using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Command.Create
{
    public class CreateTpValorCptFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateTpValorCptFondosCommand>()
                .Create();
            var validator = new CreateTpValorCptFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
