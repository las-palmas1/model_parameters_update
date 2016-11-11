using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClassLibrary1;
using NXOpen;

namespace Model_Parameters_Update
{
	class Program
	{
		static Session theSession = Session.GetSession();
		private static Logger logger;
		
		public static void UpdateParameters(string parfiles_dirname, string filename)
		{
			string current_filename = Path.Combine(parfiles_dirname, "current", filename);
			string path_filename = Path.Combine(parfiles_dirname, "model_paths", filename);
			string old_filename = Path.Combine(parfiles_dirname, "old", filename);
			logger.Info("Comparing old and current versions");
			bool comparing_result = true;
			try 
			{ 
				comparing_result = Functions.CompareTwoParametersFiles(current_filename, old_filename);
				logger.Debug(String.Format("comparing result = {0}", comparing_result.ToString()));
			}
			catch (Exception ex) { logger.Error(ex.Message); }
			if (!comparing_result)
			{
				logger.Info("Reading parameters");
				string[][] exp_arr = Functions.ReadParameters(current_filename);
				ExpressionExporter exporter = new ExpressionExporter();
				exporter.TheSession = theSession;
				exporter.Expressions_str_arr = exp_arr;
				exporter.Part_file_name = Functions.ReadPath(path_filename);
				logger.Info("Overwriting old parameters file version");
				File.Copy(current_filename, old_filename, true);
			}

		}
		
		static void Main(string[] args)
		{
			Directory.SetCurrentDirectory(@"D:\Asus\Documents\study\5 year\9 semester\blade machines, part2\course work");
			logger = new Logger(Path.Combine(Directory.GetCurrentDirectory(), "Model Parameters Update",
			"Model Parameters Update", "LoggerSettings.txt"));
			string parfiles_dir = Path.GetFullPath("Calculation\\parts_parameters\\output");

			foreach (string i in Directory.GetFiles(Path.Combine(parfiles_dir, "current")))
			{
				UpdateParameters(parfiles_dir, Path.GetFileName(i));
			}
			
			
		}
		public static int GetUnloadOption(string arg)
		{
			//Unloads the image explicitly, via an unload dialog
			//return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

			//Unloads the image immediately after execution within NX
			return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

			//Unloads the image when the NX session terminates
			// return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
		}
	}
}
