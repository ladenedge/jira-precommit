
= jira-precommit

This application parses source control commit messages for JIRA
issue keys (eg. 'ABC-123') and validates the state of those issues
using [Atlassian's SDK](http://www.nuget.org/packages/Atlassian.SDK).

== Configuration

jira-precommit requires some brief configuration in its
**jira-precommit.config** file.  Here are the options:

* `IssueRegex`: the regular expression used to find issue keys.
* `ClosedStatuses`: a comma-separated list of statuses that issues
  may *not* have if they are mentioned in commit messages.  If you
  don't want to require that mentioned issues be open, you may
  leave this setting blank.

== Command Line

jira-precommit uses several command line options to connect to JIRA.

    -b, --baseuri=VALUE        Base URI of the JIRA service (required)
    -m, --message=VALUE        The commit message to validate ('-' or leave
                                   blank for stdin)
    -p, --password=VALUE       The JIRA password to use (required)
    -u, --username=VALUE       The JIRA username to use (required)
    -v, --verbose              Enable verbose logging
    -?, -h, --help             Show this help message.

== As SVN Precommit Hook

To use jira-precommit as an SVN precommit hook, pipe the output of
svnlook to the application.

    @echo off  
    set REPOS=%1  
    set TXN=%2           

    svnlook log %REPOS% -t %TXN% | jira-precommit -b <uri> -u username -p password
    if %errorlevel% gtr 0 (goto err) else exit 0  

    :err
    exit 1
