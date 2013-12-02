
using System;
using System.IO;
using System.Reflection;
using NDesk.Options;

namespace DigiData.SVN.JiraPrecommit
{
    class Options
    {
        public Options()
        {
        }

        public string Message { get; private set; }
        public string JiraUri { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool Verbose { get; private set; }

        public void Parse(string[] args)
        {
            bool show_help = false;

            var opts = new OptionSet()
            {
   	            { "b|baseuri=", "Base URI of the JIRA service (required)", v => JiraUri = v },
   	            { "m|message=", "The commit message to validate ('-' or leave blank for stdin)", v => Message = v },
   	            { "p|password=", "The JIRA password to use (required)", v => Password = v },
   	            { "u|username=", "The JIRA username to use (required)", v => Username = v },
   	            { "v|verbose", "Enable verbose logging", v => Verbose = true },
                { "?|h|help", "Show this help message.", v => show_help = true },
            };

            opts.Parse(args);
            if (show_help)
            {
                Console.WriteLine("jira-precommit v{0}",
                        Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine();

                opts.WriteOptionDescriptions(Console.Out);
                throw new HelpOnlyException();
            }

            if (String.IsNullOrWhiteSpace(JiraUri))
                throw new OptionException("The base URI to JIRA is required.", "baseuri");
            if (String.IsNullOrWhiteSpace(Username))
                throw new OptionException("A username is required to access JIRA.", "username");
            if (String.IsNullOrWhiteSpace(Password))
                throw new OptionException("A password is required to access JIRA.", "password");

            ReadMessage();

            if (String.IsNullOrWhiteSpace(Message))
                throw new OptionException("A commit message must be present on the command line or via stdin.", "message");
        }

        void ReadMessage()
        {
            if (!String.IsNullOrWhiteSpace(Message) && Message != "-")
                return;

            Message = String.Empty;
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (Message != String.Empty)
                    Message += "; ";
                Message += line;
            }
        }
    }
}
