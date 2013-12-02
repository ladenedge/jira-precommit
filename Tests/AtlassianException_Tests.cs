
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DigiData.JiraPrecommit.Tests
{
    public class AtlassianException_Tests
    {
        [Test]
        public void Create_ThrowsWhenNull()
        {
            Assert.That(() => AtlassianException.Create(null), Throws.TypeOf<ArgumentNullException>());
        }

        Tuple<string, Type>[] Vectors = new Tuple<string, Type>[]
        {
            Tuple.Create("com.atlassian.jira.rpc.exception.RemoteValidationException: Query validation failed: An issue with key 'CFD-1' does not exist for field 'Key'.",
                         typeof(IssueNotFoundException)),
            Tuple.Create("com.atlassian.jira.rpc.exception.RemoteAuthenticationException: Invalid username or password.",
                         typeof(AuthenticationException)),
            Tuple.Create("com.atlassian.jira.rpc.exception.OtherException: This is some exception I haven't seen yet.",
                         typeof(AtlassianException)),
        };

        [Test]
        public void Create_CreatesProperExceptionType([ValueSource("Vectors")] Tuple<string, Type> vector)
        {
            var original = new FaultException(vector.Item1);
            var actual = AtlassianException.Create(original);
            Assert.That(actual, Is.TypeOf(vector.Item2));
        }

        [Test]
        public void Create_StripsExceptionTyping([ValueSource("Vectors")] Tuple<string, Type> vector)
        {
            var original = new FaultException(vector.Item1);
            var actual = AtlassianException.Create(original);
            Assert.That(actual.Message, Is.Not.StringContaining("com.atlassian.jira.rpc.exception"));
        }
    }
}
