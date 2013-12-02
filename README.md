
jira-precommit
==

*jira-precommit* is a .NET 4.0 application parses source control
commit messages for JIRA issue keys (eg. 'ABC-123') and validates
the state of those issues using
[Atlassian's .NET SDK](http://www.nuget.org/packages/Atlassian.SDK).

A return value of 0 indicates the issue was found in JIRA, and that
it was in an 'open' state (see below).  Otherwise the application
will have a return value greater than 0.

Configuration
--

jira-precommit will require some brief configuration in its
**jira-precommit.config** file for most users.  Here are the options:

* `IssueRegex`: the regular expression used to find issue keys.
  This will probably work as-is for most installations.
* `ClosedStatuses`: a comma-separated list of statuses that issues
  may *not* have if they are mentioned in commit messages.  If you
  don't want to require that mentioned issues be "open," you may
  leave this setting blank.

Error, status and verbose logging messages all appear on STDERR to
be compatible with SVN commit reports, but this may be changed in
the config file according to the [NLog](http://nlog-project.org/)
[Console Target](https://github.com/nlog/NLog/wiki/Console%20Target)
rules.

Command Line
--

jira-precommit uses several command line options to connect to JIRA.

    -b, --baseuri=VALUE        Base URI of the JIRA service (required)
    -m, --message=VALUE        The commit message to validate ('-' or leave
                                   blank for stdin)
    -p, --password=VALUE       The JIRA password to use (required)
    -u, --username=VALUE       The JIRA username to use (required)
    -v, --verbose              Enable verbose output
    -?, -h, --help             Show this help message.

As SVN Precommit Hook
--

To use jira-precommit as an SVN precommit hook, pipe the output of
svnlook to the application.

    @echo off
    setlocal
    set repo=%1
    set txn=%2

    svnlook log %repo% -t %txn% | jira-precommit -b <uri> -u username -p password
    if %errorlevel% gtr 0 (goto err) else exit 0

    :err
    exit 1
