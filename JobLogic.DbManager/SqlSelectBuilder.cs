using System;

namespace JobLogic.DatabaseManager
{
    public class SqlSelectBuilder
    {
        private string with;
        private string selectcolumns;
        private string where;
        private string from;
        private string ordercolumn;
        private string groupbycolumn;
        private string havingcolumn;
        private string orderDirection;


        public SqlSelectBuilder()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public void AddSelect(string selectcolumns)
        {
            this.selectcolumns = selectcolumns;
        }

        public void AddFrom(string from)
        {
            this.from = from;
        }

        public void AddWhere(string where)
        {
            this.where = where;
        }

        public void AddOrderBy(string ordercolumn, string orderDirection = "ASC")
        {
            this.ordercolumn = ordercolumn;
            this.orderDirection = orderDirection;
        }

        public void AddHaving(string havingcolumn)
        {
            this.havingcolumn = havingcolumn;
        }

        public void AddGroupBy(string groupbycolumn)
        {
            this.groupbycolumn = groupbycolumn;
        }

        public void AddWith(string with)
        {
            this.with = with;
        }

        public string GetSqlCount()
        {
            var sql = "WITH ";

            if (!string.IsNullOrWhiteSpace(with))
                sql += with + "," + Environment.NewLine;

            sql += "countResult (TotalCount) AS (";

            sql += "SELECT 0 " + Environment.NewLine;
            sql += "FROM " + from + Environment.NewLine;
            if (!string.IsNullOrEmpty(where))
                sql += "WHERE " + where + Environment.NewLine;

            if (!string.IsNullOrEmpty(groupbycolumn))
                sql += "GROUP BY " + groupbycolumn + Environment.NewLine;

            if (!string.IsNullOrEmpty(havingcolumn))
                sql += "HAVING " + havingcolumn + Environment.NewLine;

            sql += ") SELECT count(*) from countResult";
            return sql;
        }

        public string GetPagedSql()
        {
            var sql = "";

            if (!string.IsNullOrWhiteSpace(with))
                sql += "WITH " + with + Environment.NewLine;

            sql +=
            @"SELECT TOP " + PageSize + " * FROM (" + Environment.NewLine;
            sql += "SELECT " + Environment.NewLine;
            sql += "ROW_NUMBER() over (order by " + ordercolumn + " " + orderDirection + ") as rn,";
            sql += selectcolumns + Environment.NewLine;
            sql += "FROM " + from + Environment.NewLine;

            if (!string.IsNullOrEmpty(where))
                sql += "WHERE (" + where + ")" + Environment.NewLine;
            if (!string.IsNullOrEmpty(groupbycolumn))
                sql += "GROUP BY " + groupbycolumn + Environment.NewLine;

            if (!string.IsNullOrEmpty(havingcolumn))
                sql += "HAVING " + havingcolumn + Environment.NewLine;

            sql += ") [list]" + Environment.NewLine;
            sql += @"WHERE rn>=" + (((PageIndex - 1) * PageSize) + 1).ToString() + Environment.NewLine;

            return sql;
        }

        public string GetSql()
        {
            var sql = "";

            if (!string.IsNullOrWhiteSpace(with))
                sql += "WITH " + with + Environment.NewLine;

            sql +=
            "SELECT " + Environment.NewLine;
            sql += selectcolumns + Environment.NewLine;
            sql += "FROM " + from + Environment.NewLine;

            if (!string.IsNullOrEmpty(where))
                sql += "WHERE " + where + Environment.NewLine;
            if (!string.IsNullOrEmpty(groupbycolumn))
                sql += "GROUP BY " + groupbycolumn + Environment.NewLine;

            if (!string.IsNullOrEmpty(havingcolumn))
                sql += "HAVING " + havingcolumn + Environment.NewLine;

            sql += "ORDER BY COALESCE(" + ordercolumn + ",'') " + Environment.NewLine;

            if (!string.IsNullOrEmpty(orderDirection))
                sql += " " + orderDirection + Environment.NewLine;

            return sql;
        }
    }
}
