using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Paises.Command.Create
{
    public class CreatePaisesCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreatePaisesCommand>()
                .Create();
            var validator = new CreatePaisesCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
