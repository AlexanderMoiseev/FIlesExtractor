using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace FilesExtractor.Code.Filters {
    public static class FiltersFactory {
        public const string EXTENSION_FILTER_KEY = "ext";
        public const string MAX_FILE_SIZE_FILTER_KEY = "ms";
        public const string REGEX_FILTER_KEY = "rg";

        [NotNull]
        public static List<IFilterRuleBase> CreateFiltersByArguments(Dictionary<string, string> argumentKeyValuePairs) {
            var filters = new List<IFilterRuleBase>();

            foreach(KeyValuePair<string, string> arg in argumentKeyValuePairs) {
                switch(arg.Key) {
                    case EXTENSION_FILTER_KEY:
                        filters.Add(new FileExtensionFilter(arg.Value));
                        break;
                    case MAX_FILE_SIZE_FILTER_KEY:
                        int maxSize;
                        if(int.TryParse(arg.Value, out maxSize)) {
                            filters.Add(new FileMaxSizeFilter(maxSize));
                        }
                        break;
                    case REGEX_FILTER_KEY:
                        try {
                            filters.Add(new FileNamePatternFilter(new Regex(arg.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase)));
                        } catch(ArgumentException) {
                            Console.WriteLine("Regexp {0} is not valid", arg.Value);
                        }
                        break;
                }
            }
            return filters;
        }
    }
}