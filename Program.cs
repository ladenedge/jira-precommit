
using System;
using System.Linq;
using System.ServiceModel;
using Atlassian.Jira;
using NLog;

namespace DigiData.SVN.JiraPrecommit
{
    class Program
    {
        static Logger Log = LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        {
            try
            {
                try
                {
                    var opts = new Options();
                    opts.Parse(args);
                    if (opts.Verbose)
                    {
                        foreach (var rule in LogManager.Configuration.LoggingRules)
                            rule.EnableLoggingForLevel(LogLevel.Trace);
                        LogManager.ReconfigExistingLoggers();
                        Log.Trace("Verbose logging enabled.");
                    }
                    Log.Trace("Contents of stdin:");
                    Log.Trace(opts.Message);

                    var service = new Jira(opts.JiraUri, opts.Username, opts.Password);

                    var issueKeys = MessageReader.ParseIssues(opts.Message);
                    if (!issueKeys.Any())
                    {
                        Log.Error("Commit message must contain at least one open JIRA issue.");
                        return 2;
                    }

                    foreach (var key in issueKeys)
                    {
                        Log.Trace("Validating issue '{0}'.", key);
                        var issue = service.GetIssue(key);

                        if (!issue.IsOpen())
                        {
                            Log.Error("Referenced issue {0} must be open - current status is '{1}'.",
                                    key, issue.Status.Name);
                            return 3;
                        }

                        Log.Trace("Issue {0} is open ('{1}').", key, issue.Status.Name);
                    }

                    Log.Trace("Success.");
                    return 0;
                }
                catch (FaultException e)
                {
                    throw AtlassianException.Create(e);
                }
            }
            catch (HelpOnlyException)
            {
                return 1;
            }
            catch (AtlassianException e)
            {
                Log.Fatal(e.Message);
                return 9;
            }
            catch (Exception e)
            {
                Log.FatalException("Unexpected error:", e);
                return 10;
            }
        }
    }
}
