
using System;
using System.Configuration;
using System.Linq;
using Atlassian.Jira;
using NLog;

namespace DigiData.JiraPrecommit
{
    public static class JiraExtensions
    {
        static Logger Log = LogManager.GetCurrentClassLogger();
        static string[] DefaultClosedStatuses = { "Resolved", "Closed" };
        internal static string ClosedStatusString;

        static JiraExtensions()
        {
            ClosedStatusString = ConfigurationManager.AppSettings["ClosedStatuses"];
        }

        static string[] ClosedStatuses
        {
            get
            {
                if (ClosedStatusString == null)
                    return DefaultClosedStatuses;

                var statusArray = ClosedStatusString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(s => s.Trim()).ToArray();
                Log.Trace("Looking for closed statuses: {0}", String.Join(", ", statusArray));
                
                return statusArray;
            }
        }

        public static bool IsOpen(string status)
        {
            if (status == null)
                return false;
            return Array.IndexOf(ClosedStatuses, status) == -1;
        }

        public static bool IsOpen(this Issue issue)
        {
            if (issue == null || issue.Status == null)
                return false;
            return IsOpen(issue.Status.Name);
        }
    }
}
