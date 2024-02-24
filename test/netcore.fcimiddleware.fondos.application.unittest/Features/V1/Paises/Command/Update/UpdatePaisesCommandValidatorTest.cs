using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Paises.Command.Update
{
    public class UpdatePaisesCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdatePaisesCommand>()
                .Create();
            var validator = new UpdatePaisesCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
