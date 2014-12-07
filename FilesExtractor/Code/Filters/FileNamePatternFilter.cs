using System.IO.Compression;
using System.Text.RegularExpressions;

namespace FilesExtractor.Code.Filters {
    class FileNamePatternFilter : IFilterRuleBase {
         private readonly Regex _pattern;

         public FileNamePatternFilter(Regex pattern) {
            _pattern = pattern;
        }

        public virtual bool FileFitsRule(ZipArchiveEntry zippedEntry) {
            return _pattern.IsMatch(zippedEntry.Name);
        }
    }
}