using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocGerentes.Command.Create
{
    public class CreateSocGerentesCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateSocGerentesCommand>()
                .Create();
            var validator = new CreateSocGerentesCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
