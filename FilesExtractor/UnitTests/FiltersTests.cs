using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using FilesExtractor.Code.Filters;
using NUnit.Framework;

namespace FilesExtractor.UnitTests {
    [TestFixture]
    class FiltersTests {
        private readonly ZipArchive _archive;
        private readonly MemoryStream _memoryStream;

        public FiltersTests() {
            _memoryStream = new MemoryStream();
            _archive = new ZipArchive(_memoryStream, ZipArchiveMode.Create);
        }

        [TestFixtureTearDown]
        public void TearDown() {
            _archive.Dispose();
            _memoryStream.Dispose();
        }

        [TestCase("file.txt", "txt", true)]
        [TestCase("testFile.rb", "txt", false)]
        [TestCase("doc.ext", "", false)]
        [TestCase("file.exec", "exe", false)]
        [TestCase("file.exe.doc", "exe", false)]
        public void ExtensionFilterWorksCorrectlyTest(string fileName, string filterExtension, bool expectedResult) {
            ZipArchiveEntry entry = _archive.CreateEntry(fileName);
            var fileExtensionFilter = new FileExtensionFilter(filterExtension);
            Assert.AreEqual(fileExtensionFilter.FileFitsRule(entry), expectedResult);
        }

        [TestCase("file.py", ".+", true)]
        [TestCase("file.txt", ".+(?<!\\.exe)$", true)]
        [TestCase("file.exe", ".+(?<!\\.exe)$", false)]
        [TestCase("coolstr12.sh", "^coolstr12", true)]
        [TestCase("onecoolstr12.sh", "^coolstr12", false)]
        public void FileNamePatternFilterWorksCorrectlyTest(string fileName, string regexp, bool expectedResult) {
            ZipArchiveEntry entry = _archive.CreateEntry(fileName);
            var fileExtensionFilter = new FileNamePatternFilter(new Regex(regexp));
            Assert.AreEqual(fileExtensionFilter.FileFitsRule(entry), expectedResult);
        }
    }
}