using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Command.Update
{
    public class UpdateTpValorCptFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateTpValorCptFondosCommand>()
                .Create();
            var validator = new UpdateTpValorCptFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
