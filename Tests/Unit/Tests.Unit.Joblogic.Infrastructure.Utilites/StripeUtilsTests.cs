using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{

    [TestClass]
    public class StripeUtilsTests : BaseUnitTest
    {
        private const string _requestJson = "{\"PrimaryUserEmailAddress\":\"testemail@joblogic.com\",\"SignUpPlanId\":\"SIMPLE_UK_MONTHLY_15\",\"Quantity\":7,\"TrialEnd\":null,\"CardNumber\":\"5200828282828210\",\"CardExpirationYear\":2021,\"CardExpirationMonth\":7,\"CardHoldersName\":\"BLOB MARLEY\",\"CardCvc\":\"765\",\"CompanyName\":\"Test Company 123\",\"PrimaryUserName\":\"blob marley\",\"CompanyId\":12345,\"VatRate\":20.0,\"CouponId\":null}";
        private const string _responseJson = "{\"ValidationMessages\":[],\"ErrorMessages\":[],\"Exception\":null,\"Status\":0,\"Data\":{\"CustomerId\":\"cus_DYozJF6XBUMDDv\",\"CurrentPeriodStart\":\"2019-05-08T23:59:59Z\",\"CurrentPeriodEnd\":\"2019-06-08T23:59:59Z\",\"SubscriptionId\":\"sub_DYozkFItKcpp7V\",\"SubscriptionStatus\":\"active\",\"CardId\":\"card_1EhDLaExkjP77ZAgLsZ3gzTU\",\"CardLast4\":\"8210\",\"CardExpirationMonth\":7,\"CardExpirationYear\":2021,\"CardExpirationDate\":\"7/2021\"},\"Message\":null,\"Success\":true,\"NotExist\":false}";

        [TestMethod]
        public void StripCardInformation_Where_stripeJson_IsNull_ShouldThrowException()
        {
            // Arrange
            string stripeJson = null;

            // Action
            Action act = () => StripeUtils.StripCardInformation(stripeJson);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StripCardInformation_Where_stripeJson_IsEmptyString_ShouldThrowException()
        {
            // Arrange
            string stripeJson = string.Empty;

            // Action
            Action act = () => StripeUtils.StripCardInformation(stripeJson);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void StripCardInformation_Where_stripeJson_IsDodgyString_ShouldThrowException()
        {
            // Arrange
            string stripeJson = "dasdasd {}dsa dasss {  dsadsabdjksa {}{}{}{{}{}{}";

            // Action
            Action act = () => StripeUtils.StripCardInformation(stripeJson);

            // Assert
            act.Should().Throw<JsonReaderException>();
        }

        [TestMethod]
        public void StripCardInformation_Where_RequestJson_Contains_CardNumber_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_requestJson);

            // Assert
            result.Should().Contain($"\"CardNumber\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_RequestJson_Contains_CardExpirationYear_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_requestJson);

            // Assert
            result.Should().Contain($"\"CardExpirationYear\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_RequestJson_Contains_CardExpirationMonth_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_requestJson);

            // Assert
            result.Should().Contain($"\"CardExpirationMonth\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_RequestJson_Contains_CardHoldersName_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_requestJson);

            // Assert
            result.Should().Contain($"\"CardHoldersName\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_RequestJson_Contains_CardCvc_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_requestJson);

            // Assert
            result.Should().Contain($"\"CardCvc\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_ResponseJson_Contains_CardLast4_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_responseJson);

            // Assert
            result.Should().Contain($"\"CardLast4\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_ResponseJson_Contains_CardExpirationMonth_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_responseJson);

            // Assert
            result.Should().Contain($"\"CardExpirationMonth\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_ResponseJson_Contains_CardExpirationYear_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_responseJson);

            // Assert
            result.Should().Contain($"\"CardExpirationYear\":\"{ StripeUtils.ReplacementCharacter }\"");
        }

        [TestMethod]
        public void StripCardInformation_Where_ResponseJson_Contains_CardExpirationDate_ShouldRemoveKey()
        {
            // Action
            var result = StripeUtils.StripCardInformation(_responseJson);

            // Assert
            result.Should().Contain($"\"CardExpirationDate\":\"{ StripeUtils.ReplacementCharacter }\"");
        }
    }
}
