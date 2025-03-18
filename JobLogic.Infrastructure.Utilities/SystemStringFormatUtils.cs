namespace JobLogic.Infrastructure.Utilities
{
    public static class SystemStringFormatUtils
    {
        public static string GetTaxCodeDropdownItem(string description, string value)
        {
            var builtString = string.Empty;

            if (!string.IsNullOrWhiteSpace(description))
                builtString = $"{ description }";

            if (!string.IsNullOrWhiteSpace(value))
                builtString = $"{ builtString } ({ value })";

            return builtString;
        }

        public static string GetNominalCodeDropdownItem(string code, string description)
        {
            var builtString = $"{ code }";

            if (!string.IsNullOrWhiteSpace(description))
                builtString = $"{ builtString } - { description }";

            return builtString;
        }

        /// <summary>
        /// Maps values from properties of an object into a string template
        /// </summary>
        /// <param name="template">A string template to map values onto</param>
        /// <param name="obj">Object to map onto the template</param>
        /// <example>template: "Job Number = [JobNumber]", obj = Any Job object</example>
        /// <returns>Mapped string template containing the actual values from the object</returns>
        public static string ReplaceFieldTags(string template, object obj)
        {
            if (string.IsNullOrWhiteSpace(template) || obj == null)
                return string.Empty;

            foreach (var prop in obj.GetType().GetProperties())
            {
                var key = $"[{ prop.Name }]";
                if (template.Contains(key))
                {
                    template = template.Replace(key, $"{ prop.GetValue(obj, null) }");
                }
            }
            return template;
        }
    }
}
