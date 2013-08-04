using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Highway.Data.Rest.Configuration
{
    public static class Formatting
    {
        private static readonly IList<string> Unpluralizables = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };
        private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
        {
            // Start with the rarest cases, and move to the most common
            { "person", "people" },
            { "ox", "oxen" },
            { "child", "children" },
            { "foot", "feet" },
            { "tooth", "teeth" },
            { "goose", "geese" },
            // And now the more standard rules.
            { "(.*)fe?", "$1ves" },         // ie, wolf, wife
            { "(.*)man$", "$1men" },
            { "(.+[aeiou]y)$", "$1s" },
            { "(.+[^aeiou])y$", "$1ies" },
            { "(.+z)$", "$1zes" },
            { "([m|l])ouse$", "$1ice" },
            { "(.+)(e|i)x$", @"$1ices"},    // ie, Matrix, Index
            { "(octop|vir)us$", "$1i"},
            { "(.+(s|x|sh|ch))$", @"$1es"},
            { "(.+)", @"$1s" }
        };

        public static string Pluralize(this string singular)
        {
            if (Unpluralizables.Contains(singular))
                return singular;

            var plural = "";

            foreach (var pluralization in Pluralizations)
            {
                if (Regex.IsMatch(singular, pluralization.Key))
                {
                    plural = Regex.Replace(singular, pluralization.Key, pluralization.Value);
                    break;
                }
            }

            return plural;
        }
    }
}