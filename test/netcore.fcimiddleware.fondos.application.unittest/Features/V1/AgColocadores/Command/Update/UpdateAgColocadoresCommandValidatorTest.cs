using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.AgColocadores.Command.Update
{
    public class UpdateAgColocadoresCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateAgColocadoresCommand>()
                .Create();
            var validator = new UpdateAgColocadoresCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
