using System;
using System.Web;
using System.Text;
namespace Selenium
{
	/// <summary>
	/// A representation of a single Selenese Command
	/// </summary>
	public class DefaultSeleneseCommand : ISeleneseCommand
	{
		private static readonly string PARSE_ERROR_MESSAGE = "Command string must contain 4 pipe characters and should start with a '|'. Unable to parse command string";
		private readonly string[] args;
		private readonly string command;

		/// <summary>
		/// Creates a command with the specified arguments
		/// </summary>
		/// <param name="command">the name of the command to run</param>
		/// <param name="args">its arguments (convert non-string arguments to strings)</param>
		public DefaultSeleneseCommand(string command, string[] args)
		{
			this.command = command;
			this.args = args;
		}

		/// <summary>
		/// The string token that we'll send to the server
		/// </summary>
		public string CommandString
		{
			get
			{
				StringBuilder sb = new StringBuilder("cmd=");
				sb.Append(HttpUtility.UrlPathEncode(command));
				if (args == null) return sb.ToString();
				for (int i = 0; i < args.Length; i++)
				{
					sb.Append('&').Append((i+1).ToString()).Append('=').Append(HttpUtility.UrlPathEncode(args[i]));
				}
				return sb.ToString();
			}
		}
		
		/// <summary>
		/// The name of the Selenium command verb
		/// </summary>
		public string Command
		{
			get { return command; }
		}

		/// <summary>
		/// The array of arguments for this command
		/// </summary>
		public string[] Args
		{
			get { return args; }
		}

		/// <summary>
		/// Parses a "wiki-style" command string, like this: |type|q|Hello World|
		/// </summary>
		/// <param name="commandString">a wiki-style command string to parse</param>
		/// <returns>a Selenese Command object that implements the command string</returns>
		public static DefaultSeleneseCommand Parse(string commandString)
		{
			if (commandString == null || commandString.Trim().Length == 0 || !commandString.StartsWith("|"))
			{
				throw new ArgumentException(PARSE_ERROR_MESSAGE + "'" + commandString + "'.");
			}

			string[] commandArray = commandString.Split(new char[] { '|' });
			
			if (commandArray.Length != 5)
			{
				throw new ArgumentException(PARSE_ERROR_MESSAGE + "'" + commandString + "'.");
			}
			
			return new DefaultSeleneseCommand(commandArray[1], new String[] {commandArray[2], commandArray[3]});
		}
	}
}
