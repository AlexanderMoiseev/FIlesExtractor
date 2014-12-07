using System.IO.Compression;

namespace FilesExtractor.Code.Filters {
    class FileMaxSizeFilter : IFilterRuleBase {
        private readonly int _maxSize;

        public FileMaxSizeFilter(int maxSize) {
            _maxSize = maxSize;
        }

        public virtual bool FileFitsRule(ZipArchiveEntry zippedEntry) {
            return zippedEntry.Length <= _maxSize;
        }
    }
}