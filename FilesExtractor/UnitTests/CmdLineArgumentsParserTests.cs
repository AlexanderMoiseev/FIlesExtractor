using System.Collections.Generic;
using FilesExtractor.Code;
using NUnit.Framework;

namespace FilesExtractor.UnitTests {
    [TestFixture]
    class CmdLineArgumentsParserTests {
        private readonly CmdLineArgumentsParser _argsParser;

        public CmdLineArgumentsParserTests() {
            _argsParser = new CmdLineArgumentsParser();
        }

        [TestCase(new[] { "ms:2048" }, "ms", "2048")]
        [TestCase(new[] { "someParam:someValue" , "second"}, "someParam", "someValue")]
        public void ParamsParsedCorrectlyTest(string[] args, string expectedKey, string expactedValue) {
            Dictionary<string, string> argsKvPairs = _argsParser.Parse(args);
            Assert.IsTrue(argsKvPairs.ContainsKey(expectedKey) && argsKvPairs[expectedKey] == expactedValue);
        }

        [TestCase(null, 0)]
        [TestCase(new string[] { }, 0)]
        [TestCase(new[] { "simpleCheck:val" }, 1)]
        [TestCase(new[] { "someOherparamHere:val_12323232", "WrongParam", "k wrongParam", "goodkey:goodValue" }, 2)]
        [TestCase(new[] { "heresWrongParam 1", "k v", "sa-sa" }, 0)]
        [TestCase(new[] { "duplicateKey:duplicateVal", "duplicateKey:notDuplicateVal" }, 1)]
        [TestCase(new[] { "notDuplicateKey:duplicateVal", "duplicateKey:duplicateVal" }, 2)]
        [TestCase(new[] { "emptyVal:", " :emptyKey" , " : "}, 0)]
        public void HaveCorrectNumberOfParametersTest(string[] args, int expectedResult) {
            Dictionary<string, string> argsKvPairs = _argsParser.Parse(args);
            Assert.AreEqual(argsKvPairs.Count, expectedResult);
        }
    }
}