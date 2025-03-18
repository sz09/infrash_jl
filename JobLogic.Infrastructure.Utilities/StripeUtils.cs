using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace JobLogic.Infrastructure.Utilities
{
    public static class StripeUtils
    {
        /// <summary>
        /// Current value = "x"
        /// </summary>
        public static readonly string ReplacementCharacter = "x";

        private static string[] _keysToStrip = new string[]
            { "CardNumber", "CardExpirationYear", "CardExpirationMonth", "CardHoldersName", "CardCvc", "CardLast4", "CardExpirationDate" };

        /// <summary>
        /// Strip sensitive information (such as credit card numbers) from Stripe Request and Response JSON objects, ready to be audited
        /// </summary>
        /// <param name="stripeJson">JSON string of Request or Response to strip</param>
        /// <returns>Stripped JSON string</returns>
        public static string StripCardInformation(string stripeJson)
        {
            if (string.IsNullOrWhiteSpace(stripeJson))
                throw new ArgumentNullException("StripeUtils.StripCardInformation() - stripeJson cannot be null or empty");

            var stripeObject = JObject.Parse(stripeJson);

            // This is called recursively until it finds a leaf node (no children)
            // Once leaf node is found, it is compared to _keysToStrip and blanked out if it matches
            stripeObject.stripKeysFromToken();

            return stripeObject.ToString(Formatting.None);
        }

        private static JToken stripKeysFromToken(this JToken token)
        {
            foreach (var child in token.Children())
            {
                child.stripKeysFromToken();

                if (child.Type == JTokenType.Property)
                {
                    var property = child as JProperty;
                    property.stripSensitiveKeys();
                }
            }

            return token;
        }

        private static void stripSensitiveKeys(this JProperty property)
        {
            if (_keysToStrip.Contains(property.Name))
            {
                property.Value = ReplacementCharacter;
            }
        }
    }
}
