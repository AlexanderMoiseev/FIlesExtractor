using System.Collections.Generic;
using System.Linq;
using FilesExtractor.Code.Extensions;
using JetBrains.Annotations;

namespace FilesExtractor.Code {
    /// <summary>
    /// Parses command line arguments. Expects the key value pairs that look like  key:value
    /// </summary>
    public class CmdLineArgumentsParser {
        private const int ARGUMENT_PARTS_COUNT = 2;
        public const char ARG_KEY_VALUE_SEPARATOR = ':';

        [NotNull]
        public Dictionary<string, string> Parse(string[] args) {
            return args.IsNullOrEmpty() ? new Dictionary<string, string>() :
                args.Select(arg => arg.Split(new[] { ARG_KEY_VALUE_SEPARATOR }, ARGUMENT_PARTS_COUNT))
                   .Where(arg => arg.Length == ARGUMENT_PARTS_COUNT && !string.IsNullOrWhiteSpace(arg[0]) && !string.IsNullOrWhiteSpace(arg[1]))
                        .GroupBy(arg => arg[0], arg => arg[1])
                        .ToDictionary(g => g.Key, g => g.First());
        }
    }
}