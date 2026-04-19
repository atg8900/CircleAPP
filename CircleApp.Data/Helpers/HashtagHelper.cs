using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CircleApp.Data.Helpers
{
    public static class HashtagHelper
    {
        public static List<string> GetHashtags(string postText)
        {
            var hashtagPattern = new Regex(@"#\w+");
            var matches = hashtagPattern.Matches(postText)
                .Select(match => match.Value.TrimEnd('.', ',', '!', '?').ToLower())
                .Distinct()
                .ToList();

            return matches;
        }
    }
}
