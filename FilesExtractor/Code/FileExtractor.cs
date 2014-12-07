using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FilesExtractor.Code.Extensions;
using FilesExtractor.Code.Filters;

namespace FilesExtractor.Code {
    class FileExtractor {
        public const string ZIP_FILES_EXTENSION = ".zip";
        private readonly List<IFilterRuleBase> _extractionRules;

        public FileExtractor(List<IFilterRuleBase> extractionRules) {
            _extractionRules = extractionRules;
        }

        public void Extract(string source, string destination) {
            Directory.CreateDirectory(destination);
            try {
                using(ZipArchive archive = ZipFile.OpenRead(source)) {
                    archive.Entries.ForEach(entry => ExtractEntry(Path.Combine(destination, entry.FullName), entry));
                }
            } catch(IOException e) {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private void ExtractEntry(string fullPath, ZipArchiveEntry entry) {
            if(IsFolder(entry)) {
                Directory.CreateDirectory(fullPath);
                return;
            }
            if(entry.Name.EndsWith(ZIP_FILES_EXTENSION)) { //Note: Archive within an archive. 
                ExtractInnerArchive(entry, fullPath);
            } else if(_extractionRules.All(r => r.FileFitsRule(entry))) {
                entry.ExtractToFile(fullPath, true);
            }
        }

        private void ExtractInnerArchive(ZipArchiveEntry entry, string fullPath) {
            entry.ExtractToFile(fullPath, true);
            string newUnZipFolder =
                fullPath.Substring(0, fullPath.LastIndexOf(ZIP_FILES_EXTENSION, StringComparison.OrdinalIgnoreCase));
            Directory.CreateDirectory(newUnZipFolder);
            Extract(fullPath, newUnZipFolder);
            File.Delete(fullPath);
        }

        private bool IsFolder(ZipArchiveEntry entry) {
            return string.IsNullOrEmpty(entry.Name);
        }
    }
}