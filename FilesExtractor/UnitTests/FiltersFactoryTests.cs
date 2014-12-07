using System.Collections.Generic;
using FilesExtractor.Code.Extensions;
using FilesExtractor.Code.Filters;
using NUnit.Framework;

namespace FilesExtractor.UnitTests {
    [TestFixture]
    class FiltersFactoryTests {
        private const string STUB_VALUE = "123";

        [TestCase(new[] { FiltersFactory.REGEX_FILTER_KEY, FiltersFactory.MAX_FILE_SIZE_FILTER_KEY }, 2)]
        [TestCase(new[] { FiltersFactory.REGEX_FILTER_KEY, FiltersFactory.EXTENSION_FILTER_KEY, FiltersFactory.MAX_FILE_SIZE_FILTER_KEY }, 3)]
        [TestCase(new[] { FiltersFactory.EXTENSION_FILTER_KEY, "somewrongKey", "anotherWrongParamKey" }, 1)]
        public void NumberOfCreatedFiltersIsCorrectTest(string[] keys, int expactedCount) {
            Dictionary<string, string> argumentsKeysValues = CreateTestParams(keys);
            List<IFilterRuleBase> filters = FiltersFactory.CreateFiltersByArguments(argumentsKeysValues);
            Assert.AreEqual(filters.Count, expactedCount);
        }

        private Dictionary<string, string> CreateTestParams(IEnumerable<string> keys) {
            var argumentsKeysValues = new Dictionary<string, string>();
            keys.ForEach(k => argumentsKeysValues.Add(k, STUB_VALUE));
            return argumentsKeysValues;
        }
    }
}