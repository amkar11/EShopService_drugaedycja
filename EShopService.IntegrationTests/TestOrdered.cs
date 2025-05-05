using Xunit.Abstractions;
using Xunit.Sdk;
using System.Collections.Generic;
using System.Linq;

namespace EShopService.IntegrationTests
{
    public class TestOrdered : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(tc =>
            tc.TestMethod.Method
             .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
             .FirstOrDefault()?.GetNamedArgument<int>("Priority") ?? 0);
        }
    }
}
