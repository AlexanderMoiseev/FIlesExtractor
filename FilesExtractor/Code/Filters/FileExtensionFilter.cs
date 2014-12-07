using System;
using System.IO.Compression;

namespace FilesExtractor.Code.Filters {
    public class FileExtensionFilter : IFilterRuleBase {
        private readonly string _extension;

        public FileExtensionFilter(string extension) {
            _extension = extension;
        }

        public virtual bool FileFitsRule(ZipArchiveEntry zippedEntry) {
            return zippedEntry.Name.EndsWith(string.Format(".{0}", _extension), 
                StringComparison.OrdinalIgnoreCase);
        }
    }
}