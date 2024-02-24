using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Create;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Command.Create
{
    public class CreateSocDepositariasCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<CreateSocDepositariasCommand>()
                .Create();
            var validator = new CreateSocDepositariasCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
