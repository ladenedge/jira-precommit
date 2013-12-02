
using System;
using System.Configuration;
using System.Linq;
using Atlassian.Jira;
using NLog;

namespace DigiData.SVN.JiraPrecommit
{
    public static class JiraExtensions
    {
        static Logger Log = LogManager.GetCurrentClassLogger();
        static string[] DefaultClosedStatuses = { "Resolved", "Closed" };

        public static string[] ClosedStatuses
        {
            get
            {
                var statuses = ConfigurationManager.AppSettings["ClosedStatuses"];
                if (statuses == null)
                    return DefaultClosedStatuses;

                var statusArray = statuses.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(s => s.Trim()).ToArray();
                Log.Trace("Looking for closed statuses: {0}", String.Join(", ", statusArray));
                
                return statusArray;
            }
        }

        public static bool IsOpen(this Issue issue)
        {
            if (issue == null || issue.Status == null)
                return false;
            return Array.IndexOf(ClosedStatuses, issue.Status.Name) == -1;
        }
    }
}
