using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ClassLibrary1
{
	public enum LogMinLevel { Debug, Info, Error };
	public enum LogTarget { Console, File, Both };

	public class Logger
	{
		private string target_filename;
		private LogMinLevel minLevel;
		private LogTarget target;

		public Logger(LogTarget target, string target_filename, LogMinLevel minLevel)
		{
			this.target_filename = target_filename;
			this.minLevel = minLevel;
			this.target = target;

		}
		public Logger(string settings_filename)
		{
			StreamReader sr = new StreamReader(settings_filename);
			Dictionary<string, string> settings = new Dictionary<string, string>();
			settings["TargetFileName"] = sr.ReadLine().Split('=')[1];
			settings["MinLevel"] = sr.ReadLine().Split('=')[1];
			settings["Target"] = sr.ReadLine().Split('=')[1];
			this.target_filename = settings["TargetFileName"];
			if (settings["MinLevel"] == "Debug") { this.minLevel = LogMinLevel.Debug; }
			if (settings["MinLevel"] == "Info") { this.minLevel = LogMinLevel.Info; }
			if (settings["MinLevel"] == "Error") { this.minLevel = LogMinLevel.Error; }
			if (settings["Target"] == "file") { this.target = LogTarget.File; }
			if (settings["Target"] == "console") { this.target = LogTarget.Console; }
			if (settings["Target"] == "both") { this.target = LogTarget.Both; }
		}
		public void Debug(string message)
		{
			if ((int)this.minLevel == 0)
			{
				string line = String.Format("{0} {1}  Debug  {2}", DateTime.Now.ToShortDateString(),
					DateTime.Now.ToLongTimeString(), message);
				if (this.target == LogTarget.File || this.target == LogTarget.Both)
				{
					StreamWriter sw = new StreamWriter(this.target_filename, true);
					sw.WriteLine(line);
					sw.Close();
				}
				if (this.target == LogTarget.Console || this.target == LogTarget.Both) { Console.WriteLine(line); }
			}
		}
		public void Info(string message)
		{
			if ((int)this.minLevel <= 1)
			{
				string line = String.Format("{0} {1}  Info  {2}", DateTime.Now.ToShortDateString(),
					DateTime.Now.ToLongTimeString(), message);
				if (this.target == LogTarget.File || this.target == LogTarget.Both)
				{
					StreamWriter sw = new StreamWriter(this.target_filename, true);
					sw.WriteLine(line);
					sw.Close();
				}
				if (this.target == LogTarget.Console || this.target == LogTarget.Both) { Console.WriteLine(line); }
			}
		}
		public void Error(string message)
		{
			if ((int)this.minLevel <= 2)
			{
				string line = String.Format("{0} {1}  Error  {2}", DateTime.Now.ToShortDateString(),
					DateTime.Now.ToLongTimeString(), message);
				if (this.target == LogTarget.File || this.target == LogTarget.Both)
				{
					StreamWriter sw = new StreamWriter(this.target_filename, true);
					sw.WriteLine(line);
					sw.Close();
				}
				if (this.target == LogTarget.Console || this.target == LogTarget.Both) { Console.WriteLine(line); }
			}
		}
	}
}
