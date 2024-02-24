using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocGerentes.Command.Update
{
    public class UpdateSocGerentesCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateSocGerentesCommand>()
                .Create();
            var validator = new UpdateSocGerentesCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
