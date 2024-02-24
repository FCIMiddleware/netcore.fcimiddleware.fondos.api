using AutoFixture;
using FluentAssertions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Update;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Command.Update
{
    public class UpdateSocDepositariasCommandValidatorTest
    {
        [Fact]
        public async Task Validation_WithPropertyCorrect_IsValidTrue()
        {
            // Arrange
            var request = new Fixture().Build<UpdateSocDepositariasCommand>()
                .Create();
            var validator = new UpdateSocDepositariasCommandValidator();
            // Act
            var result = await validator.ValidateAsync(request);
            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
