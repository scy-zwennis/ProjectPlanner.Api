using System.Text.RegularExpressions;

namespace ProjectPlanner.Api.Common.Helpers
{
    public static class ExceptionHelpers
    {
        public static BasicException HandleUniqueIndex(string message, string table)
        {
            var rxColumn = new Regex(string.Format(@"'{0}_(.*)'.*The duplicate key value is \((.*)\)", table));

            var clmMatches = rxColumn.Matches(message);
            var clmGroups = clmMatches[0].Groups;

            return new BasicException
            {
                Type = "uniqueIndex",
                Field = clmGroups[1].ToString(),
                Message = clmGroups[2].ToString() + " already exists"
            };
        }
    }

    public class BasicException
    {
        public string Type { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
