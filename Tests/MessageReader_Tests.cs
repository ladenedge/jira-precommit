
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DigiData.SVN.JiraPrecommit.Tests
{
    public class MessageReader_Tests
    {
        Tuple<string, string[]>[] Vectors = new Tuple<string, string[]>[]
        {
            Tuple.Create((string)null, new string[] { }),
            Tuple.Create("", new string[] { }),
            Tuple.Create("C-1", new string[] { }),
            Tuple.Create("CF-1", new string[] { "CF-1", }),
            Tuple.Create("CF-10", new string[] { "CF-10", }),
            Tuple.Create("cfd-10", new string[] { "CFD-10", }),
            Tuple.Create("CFD-10", new string[] { "CFD-10", }),
            Tuple.Create("CFD-10, CFD-11", new string[] { "CFD-10", "CFD-11", }),
            Tuple.Create("CFD-10 and CFD-11", new string[] { "CFD-10", "CFD-11", }),
            Tuple.Create("Closes CFD-10 and CFD-11", new string[] { "CFD-10", "CFD-11", }),
        };

        [Test]
        public void ParseIssues_FindsIssues([ValueSource("Vectors")] Tuple<string, string[]> test)
        {
            var issues = MessageReader.ParseIssues(test.Item1);
            Assert.That(issues, Is.EquivalentTo(test.Item2));
        }
    }
}
