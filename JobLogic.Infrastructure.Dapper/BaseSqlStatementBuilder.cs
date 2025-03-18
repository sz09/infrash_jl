﻿using System;
using System.Collections.Generic;
using System.Data;

namespace JobLogic.Infrastructure.Dapper
{
    
    public abstract class BaseSqlStatementBuilder : ISqlStatementFactory
    {
        public delegate string GetParameterNameDelegate();
        int _parameterCount;
        Dictionary<string, object> _queryParams;
        string GenerateParameterName() => "@DoNotUseJLSQLParam" + _parameterCount++;
        
        protected void AddParameter(string nameWithoutATPrefix, object value)
        {
            _queryParams.Add(nameWithoutATPrefix, value);
        }
        protected string Sql(Func<string> sqlFactory)
        {
            return Environment.NewLine + sqlFactory();
        }
        protected string Sql<T>(Func<GetParameterNameDelegate, string> sqlFactory, T value)
        {
            var parameterName = GenerateParameterName();
            bool didGetParameterNameFuncInvoked = false;
            GetParameterNameDelegate GetParameterNameFunc = () =>
            {
                didGetParameterNameFuncInvoked = true;
                return parameterName;
            };
            var result = sqlFactory(GetParameterNameFunc);
            if (didGetParameterNameFuncInvoked)
                _queryParams.Add(parameterName, value);
            return Environment.NewLine + result;
        }

        protected string SqlIf(bool cond, string sql)
        {
            return Environment.NewLine + (cond ? sql : string.Empty);
        }
        protected string SqlIf(bool cond, Func<string> sqlFactory)
        {
            return Environment.NewLine + (cond ? sqlFactory() : string.Empty);
        }
        protected string SqlIf<T>(bool cond, Func<GetParameterNameDelegate, string> sqlFactory, T value)
        {
            if (cond)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                GetParameterNameDelegate GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                var result = sqlFactory(GetParameterNameFunc);
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(parameterName, value);
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }

        protected string SqlRepeat<T>(Func<GetParameterNameDelegate, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            string result = string.Empty;
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                GetParameterNameDelegate GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result += sqlFactory(GetParameterNameFunc, p);
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(parameterName, p);
            }
            return Environment.NewLine + result;
        }
        protected string SqlRepeatIf<T>(bool cond, Func<GetParameterNameDelegate, string> sqlFactory, IEnumerable<T> parameters)
        {
            if (cond)
            {
                string result = string.Empty;
                foreach (var p in parameters)
                {
                    var parameterName = GenerateParameterName();
                    bool didGetParameterNameFuncInvoked = false;
                    GetParameterNameDelegate GetParameterNameFunc = () =>
                    {
                        didGetParameterNameFuncInvoked = true;
                        return parameterName;
                    };
                    result += sqlFactory(GetParameterNameFunc);
                    if (didGetParameterNameFuncInvoked)
                        _queryParams.Add(parameterName, p);
                }
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }
        protected string SqlRepeatIf<T>(bool cond, Func<GetParameterNameDelegate, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            if (cond)
            {
                string result = string.Empty;
                foreach (var p in parameters)
                {
                    var parameterName = GenerateParameterName();
                    bool didGetParameterNameFuncInvoked = false;
                    GetParameterNameDelegate GetParameterNameFunc = () =>
                    {
                        didGetParameterNameFuncInvoked = true;
                        return parameterName;
                    };
                    result += sqlFactory(GetParameterNameFunc, p);
                    if (didGetParameterNameFuncInvoked)
                        _queryParams.Add(parameterName, p);
                }
                return Environment.NewLine + (cond ? result : string.Empty);
            }
            else return Environment.NewLine;
        }

        protected string SqlImplodeOr<T>(Func<GetParameterNameDelegate, string> sqlFactory, IEnumerable<T> parameters)
        {
            IList<string> result = new List<string>();
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                GetParameterNameDelegate GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result.Add(sqlFactory(GetParameterNameFunc));
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(parameterName, p);
            }
            return Environment.NewLine + string.Join(" Or ", result);
        }
        protected string SqlImplodeOr<T>(Func<GetParameterNameDelegate, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            return SqlImplode(" Or ", sqlFactory, parameters);
        }
        protected string SqlImplodeComa<T>(Func<GetParameterNameDelegate, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            return SqlImplode(" , ", sqlFactory, parameters);
        }

        private string SqlImplode<T>(string implodeVal,Func<GetParameterNameDelegate, T, string> sqlFactory, IEnumerable<T> parameters)
        {
            IList<string> result = new List<string>();
            foreach (var p in parameters)
            {
                var parameterName = GenerateParameterName();
                bool didGetParameterNameFuncInvoked = false;
                GetParameterNameDelegate GetParameterNameFunc = () =>
                {
                    didGetParameterNameFuncInvoked = true;
                    return parameterName;
                };
                result.Add(sqlFactory(GetParameterNameFunc, p));
                if (didGetParameterNameFuncInvoked)
                    _queryParams.Add(parameterName, p);
            }
            return Environment.NewLine + string.Join(implodeVal, result);
        }

        protected abstract string BuildStatement();

        public SqlStatement CreateSqlStatement(bool doShowAll = false)
        {
            _parameterCount = 0;
            _queryParams = new Dictionary<string, object>();
            var sql = BuildStatement();
            return new SqlStatement(sql, _queryParams, doShowAll);
        }
    }
}
