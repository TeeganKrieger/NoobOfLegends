using System.Text;

namespace NoobOfLegends_BackEnd.Models.HTTP
{
    /// <summary>
    /// Helper class that builds query strings.
    /// </summary>
    public class QueryBuilder
    {
        private StringBuilder stringBuilder;
        private bool concatWithAnd;

        public QueryBuilder()
        {
            concatWithAnd = false;
            stringBuilder = new StringBuilder();
            stringBuilder.Append("?");
        }

        /// <summary>
        /// Adds a query to the query string.
        /// </summary>
        /// <param name="key">The key of the query.</param>
        /// <param name="value">The value of the query.</param>
        public void AddQuery(string key, object value)
        {
            if (concatWithAnd)
                stringBuilder.Append("&");
            stringBuilder.Append(key);
            stringBuilder.Append("=");
            stringBuilder.Append(value);
            concatWithAnd = true;
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}
