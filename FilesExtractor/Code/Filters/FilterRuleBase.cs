using System.IO.Compression;

namespace FilesExtractor.Code.Filters {
    public interface IFilterRuleBase {
        bool FileFitsRule(ZipArchiveEntry zippedEntry);
    }
}