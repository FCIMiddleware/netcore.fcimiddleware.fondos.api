using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Command.Update
{
    public class UpdateMonedasCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateMonedasCommand>()
                .Create();
            var validator = new UpdateMonedasCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
