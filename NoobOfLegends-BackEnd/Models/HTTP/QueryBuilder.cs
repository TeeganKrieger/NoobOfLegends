using System.Text;

namespace NoobOfLegends_BackEnd.Models.HTTP
{
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
