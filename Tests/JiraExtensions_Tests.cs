
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DigiData.JiraPrecommit.Tests
{
    public class JiraExtensions_Tests
    {
        [TearDown]
        public void Cleanup()
        {
            JiraExtensions.ClosedStatusString = null;
        }

        [Test]
        public void IsOpen_FalseWhenNull()
        {
            Assert.That(JiraExtensions.IsOpen((string)null), Is.False);
        }

        string[] DefaultClosedStatuses = { "Resolved", "Closed" };

        [Test]
        public void IsOpen_TrueForDefaultStatuses([ValueSource("DefaultClosedStatuses")] string status)
        {
            Assert.That(JiraExtensions.IsOpen(status), Is.False);
        }

        [Test]
        public void IsOpen_FalseForOtherStatus()
        {
            Assert.That(JiraExtensions.IsOpen("Half-empty"), Is.True);
        }

        [Test]
        public void IsOpen_TrueForCustomStatus()
        {
            JiraExtensions.ClosedStatusString = "Full";

            Assert.That(JiraExtensions.IsOpen("Full"), Is.False);
        }

        [Test]
        public void IsOpen_TrueForMultipleStatuses()
        {
            JiraExtensions.ClosedStatusString = "Full,Done";

            Assert.That(JiraExtensions.IsOpen("Full"), Is.False);
            Assert.That(JiraExtensions.IsOpen("Done"), Is.False);
        }

        [Test]
        public void IsOpen_TrimsStatuses()
        {
            JiraExtensions.ClosedStatusString = "Full\t, Done ";

            Assert.That(JiraExtensions.IsOpen("Full"), Is.False);
            Assert.That(JiraExtensions.IsOpen("Done"), Is.False);
        }

        [Test]
        public void IsOpen_RemovesEmptyStatuses()
        {
            JiraExtensions.ClosedStatusString = "Full\t, Done ,,Finished ";

            Assert.That(JiraExtensions.IsOpen("Full"), Is.False);
            Assert.That(JiraExtensions.IsOpen("Done"), Is.False);
        }
    }
}
