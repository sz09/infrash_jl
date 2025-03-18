using System;
using System.Globalization;

namespace JobLogic.Infrastructure.Utilities
{
    public class DataConverter
    {
        private static readonly Lazy<DataConverter> lazy =
        new Lazy<DataConverter>(() => new DataConverter());

        public static DataConverter Instance { get { return lazy.Value; } }

        private DataConverter()
        {
        }

        public TDataType To<TDataType>(object value)
        {
            try
            {
                if (value == null)
                {
                    return default(TDataType);
                }

                // Get new datatype if datatype input is nullable
                Type type = Nullable.GetUnderlyingType(typeof(TDataType)) ?? typeof(TDataType);

                // Convert data to new datatype
                return DoConvert<TDataType>(value, type);
            }
            catch
            {
                return default(TDataType);
            }
        }

        private TDataType DoConvert<TDataType>(object value, Type type)
        {
            if (type == typeof(Guid))
            {
                if (value is string)
                {
                    value = new Guid(value as string);
                }
                if (value is byte[])
                {
                    value = new Guid(value as byte[]);
                }
            }

            return (TDataType)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }
    }
}
