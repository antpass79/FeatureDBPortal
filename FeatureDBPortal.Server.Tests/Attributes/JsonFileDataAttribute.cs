using FeatureDBPortal.Server.Tests.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace FeatureDBPortal.Server.Tests.Attributes
{
    public class JsonFileDataAttribute : DataAttribute
	{
        private readonly string _jsonTestFile;

		public JsonFileDataAttribute(string jsonTestFile)
        {
            _jsonTestFile = jsonTestFile;
		}

		public override IEnumerable<object[]> GetData(MethodInfo testMethod)
		{
			string testsText = File.ReadAllText(_jsonTestFile);
			IEnumerable<CombinationTest> tests = JsonConvert.DeserializeObject<IEnumerable<CombinationTest>>(testsText);
			return tests.Select(test => new object[] { test, test.Result });
		}
	}
}
