using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Fondos.Command.Update
{
    public class UpdateFondosCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateFondosCommand>()
                .Create();
            var validator = new UpdateFondosCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
