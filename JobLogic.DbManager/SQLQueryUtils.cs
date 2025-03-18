using JobLogic.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JobLogic.DatabaseManager
{
    public static class SQLQueryUtils
    {
        public static void AddLineIntoQuery(StringBuilder query, string condition)
        {
            query.Append(condition);
        }

        public static void AddConditionIntoQuery(StringBuilder query, string condition, string parameterName)
        {
            query.Append(string.Format(condition, parameterName));
        }

        public static void AddParameter(string parameterName, List<IDbDataParameter> queryParameters, object value, SqlDbType? sqlType = null)
        {
            queryParameters.Add(value.AsParam(parameterName, sqlType));
        }

        public static string GetRowCountForQuery(string query)
        {
            return $"SELECT COUNT(*) FROM ({query}) count;";
        }

        public static string AddPaginationToQuery(string query, List<IDbDataParameter> queryParams, int pageSize = 10, int pageNumber = 1)
        {
            if (pageSize == int.MaxValue)
            {
                return query;
            }
            else
            {
                var currentQuery = new StringBuilder(query);
                pageSize = pageSize.ValidatePageSize();
                pageNumber = pageNumber.ValidatePageIndex();

                var parameterName = "rowOffset";
                AddConditionIntoQuery(currentQuery, $" OFFSET @{{0}} ROWS", parameterName);
                AddParameter(parameterName, queryParams, (pageNumber - 1) * pageSize);

                parameterName = "pageSize";
                AddConditionIntoQuery(currentQuery, $" FETCH NEXT @{{0}} ROWS ONLY", parameterName);
                AddParameter(parameterName, queryParams, pageSize);

                return currentQuery.ToString();
            }
        }

        /// <summary>
        /// split string from controller to SQL format
        /// </summary>
        /// <param name="fromString">1,2,3 or guid1, guid2, guid3</param>
        /// <param name="validateType"> type for validating</param>
        /// <returns></returns>
        public static string SplitByComma(this string fromString, Type validateType)
        {
            if (string.IsNullOrWhiteSpace(fromString))
            {
                return "NULL";
            }
            var listString = fromString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(it => !string.IsNullOrWhiteSpace(it))
                                .Select(x => x.Trim())
                                .ToList();

            // Only get item which can convert from the list
            if (validateType == null || validateType == typeof(string))
            {
                throw new Exception("For security reason, string type is not supported");
            }

            TypeConverter converter = TypeDescriptor.GetConverter(validateType);
            listString = listString.Where(it => converter.IsValid(it)).ToList();
           

            return string.Join(",", listString.Select(it => $"'{it}'"));
        }

        public static string FormatSplitFunction<T>(this IEnumerable<T> listObject)
        {
            // make sure T is not string
            if (typeof(T) == typeof(string))
            {
                throw new Exception("For security reason, string type is not supported");
            }
            return string.Join(",", listObject.Select(it => $"'{it}'"));
        }

        public static string FormatSplitFunction(string parameterName)
        {
            return $" Select TRIM(VALUE) from STRING_SPLIT(@{parameterName},',')";
        }

        public static void FilterBySearchTerm(this StringBuilder query, string searchTerm, List<IDbDataParameter> queryParams, params string[] fieldNames)
        {
            query.FilterBySearchTerm(searchTerm, queryParams, null, fieldNames.ToArray());
        }

        public static void FilterBySearchTerm(this StringBuilder query, string searchTerm, List<IDbDataParameter> queryParams, SqlDbType? sqlDbType, params string[] fieldNames)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm) && fieldNames != null && fieldNames.Any())
            {
                searchTerm = searchTerm.ToLower().EscapeSquareBracket();
                var filters = SearchTermUtils.Instance.Build(searchTerm);

                var nonEmptyFields = fieldNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (filters != null && filters.Any())
                {
                    for (int i = 0; i < filters.Count(); i++)
                    {
                        var filter = filters[i];
                        var parameterName = $"filter_{i}";

                        AddConditionIntoQuery(query, $" AND ({GenerateQuery(nonEmptyFields[0])}", parameterName);
                        for (int index = 1; index < nonEmptyFields.Count; index++)
                        {
                            AddConditionIntoQuery(query, $" OR {GenerateQuery(nonEmptyFields[index])}", parameterName);
                        }
                        AddLineIntoQuery(query, ")");
                        AddParameter(parameterName, queryParams, filter, sqlDbType);
                    }
                }
            }
        }

        public static string EscapeSquareBracket(this string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return searchTerm;

            var sb = new StringBuilder(searchTerm.Length);
            for (int i = 0; i < searchTerm.Length; i++)
            {
                char c = searchTerm[i];
                switch (c)
                {
                    case '[':
                    case '%':
                    case '_':
                        sb.Append("[" + c + "]");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        public static T GetItem<T>(this IDataReader dr, string columnName, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(columnName)) return GetItem(dr, 0, defaultValue);
            if (dr[columnName] != DBNull.Value)
            {
                return (T)dr[columnName];
            }
            else if (defaultValue != null && defaultValue.GetType().FullName == "System.DateTime")
            {
                defaultValue = (T)Convert.ChangeType(SqlDateTime.MinValue.Value, typeof(T));
            }

            return defaultValue;
        }

        public static T GetItem<T>(this IDataReader dr, int columnIndex, T defaultValue = default(T))
        {
            if (dr[columnIndex] != DBNull.Value) return (T)dr[columnIndex];
            return defaultValue;
        }

        public static SqlParameter AsParam<T>(this T value, string parmName, SqlDbType? sqlType = null)
        {
            if (string.IsNullOrEmpty(parmName)) parmName = GetMemberName(() => value);
            if (!parmName.Contains("@")) parmName = "@" + parmName;

            var parm = new SqlParameter();
            parm.ParameterName = parmName;
            parm.SourceColumn = parmName.Replace("@", "");
            if (typeof(T) == typeof(DateTime))
                value = (T)ConvertToSqlDateTime(value);

            if (value == null)
                parm.Value = DBNull.Value;
            else
                parm.Value = value;

            if (sqlType != null) parm.SqlDbType = sqlType.Value;

            parm.IsNullable = true;
            return parm;
        }

        private static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        private static object ConvertToSqlDateTime(object dateTime)
        {
            var pDateTime = (DateTime)dateTime;
            if (IsValidSqlDateTime(pDateTime)) return pDateTime;
            return SqlDateTime.MinValue.Value;
        }

        public static SqlParameter AsOutParam<T>(this T value, string parmName, SqlDbType? sqlType = null)
        {
            var parm = AsParam(value, parmName, sqlType);
            parm.Direction = ParameterDirection.InputOutput;
            parm.Size = 100;
            return parm;
        }

        public static string GetCheckExistQuery(this string query)
        {
            return $"SELECT ISNULL((SELECT TOP 1 1 FROM {query}), 0)";
        }

        private static string GenerateQuery(string fieldName)
        {
            return $" {fieldName} LIKE '%'+@{{0}}+'%' {(fieldName.IndexOf("exist", StringComparison.OrdinalIgnoreCase) > -1 ? ")" : "")}";
        }

        private static bool IsValidSqlDateTime(DateTime value)
        {
            if (value.Year <= SqlDateTime.MinValue.Value.Year || value.Year > SqlDateTime.MaxValue.Value.Year)
                return false;
            return true;
        }
    }
}
