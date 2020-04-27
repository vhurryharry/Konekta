using Xunit;

namespace WCA.UnitTests
{
    public class ABNValidationAttributeTests
    {
        [Theory]
        [InlineData("33 102 417 032", false, true)]
        [InlineData("29002589460", false, true)]
        [InlineData("33 102 417 032asdfsf", false, false)]
        [InlineData("444", false, false)]
        [InlineData(null, false, false)]
        [InlineData(null, true, true)]
        [InlineData("", false, false)]
        [InlineData("", true, true)]
        public void IsValidAbn(string abn, bool allowNullOrEmpty, bool expectedValidity)
        {
            Domain.Validators.ABNAttribute aBNValidationAttribute = new Domain.Validators.ABNAttribute(allowNullOrEmpty);
            Assert.True(aBNValidationAttribute.IsValid(abn) == expectedValidity);
        }
    }
}