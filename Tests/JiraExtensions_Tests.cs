
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DigiData.SVN.JiraPrecommit.Tests
{
    public class JiraExtensions_Tests
    {
        [Test]
        public void Create_ThrowsWhenNull()
        {
            Assert.That(() => AtlassianException.Create(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
