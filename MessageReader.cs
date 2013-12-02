
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DigiData.SVN.JiraPrecommit
{
    public class MessageReader
    {
        const string DefaultIssueRegex = @"[A-Za-z]{2,}-\d+";

        static string IssueRegex
        {
            get { return ConfigurationManager.AppSettings["IssueRegex"] ?? DefaultIssueRegex; }
        }

        public static IEnumerable<string> ParseIssues(string message)
        {
            var issues = new List<string>();
            if (message == null)
                return issues;

            var matches = Regex.Matches(message, IssueRegex);
            foreach (Match m in matches)
                issues.Add(m.Value.ToUpper());

            return issues;
        }
    }
}
