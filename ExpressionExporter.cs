using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using System.IO;

namespace ClassLibrary1
{
    public class ExpressionExporter
    {
		private Logger logger = new Logger(Path.Combine(Directory.GetCurrentDirectory(), "Model Parameters Update", 
			"Model Parameters Update", "LoggerSettings.txt"));
        private Session theSession = null;
        private string[][] expressions_str_arr = null;
        private string part_file_name = null;
        private string output_file_name = null;
        private Part workPart = null;
        private SaveMode save_mode = SaveMode.SaveAndClose;

		private bool NotNullFieldsForExport()
		{
			if (this.theSession == null || this.expressions_str_arr == null || this.part_file_name == null)
			{
				return false;
			}
			else { return true; }
		}

        public Session TheSession
        {
            set 
			{ 
				this.theSession = value;
				if (this.NotNullFieldsForExport())
				{
					this.ExportExpressions();
					this.SavePart();
				}
			}
            get { return this.theSession; }
        }
		public string[][] Expressions_str_arr
		{
			set 
			{ 
				this.expressions_str_arr = value;
				if (this.NotNullFieldsForExport())
				{
					this.ExportExpressions();
					this.SavePart();
				}
			}
			get { return this.expressions_str_arr; }
		}
		public string Part_file_name
		{
			set 
			{ 
				this.part_file_name = value;
				logger.Debug(String.Format("Model name = {0}", Path.GetFileName(value)));
				if (this.NotNullFieldsForExport())
				{
					this.ExportExpressions();
					this.SavePart();
				}
			}
			get { return this.part_file_name; }
		}
		public string Output_file_name
		{
			set { this.output_file_name = value; }
			get { return this.output_file_name; }
		}
		public SaveMode Save_mode
		{
			set { this.save_mode = value; }
			get { return this.save_mode; }
		}
		public void ExportExpressions()
		{
			this.logger.Info("Exporting parameters");
			try
			{
				try
				{
					string[] part_files = Directory.GetFiles(Path.GetDirectoryName(this.part_file_name));
					if (part_files.Count(s => s == this.part_file_name) != 0)
					{
						PartLoadStatus partLoadStatus;
						this.workPart = this.theSession.Parts.OpenDisplay(this.part_file_name, out partLoadStatus);
						NXAPIFunctions.ExportExpressions(this.expressions_str_arr, this.workPart);
					}
					else
					{
						this.workPart = this.theSession.Parts.NewDisplay(this.part_file_name, Part.Units.Millimeters);
						NXAPIFunctions.ExportExpressions(this.expressions_str_arr, this.workPart);
					}
				}
				catch (IOException ex1) { logger.Error(ex1.Message); }
			}
			catch (NXException ex)
			{
				this.logger.Error(ex.Message);
			}
		}
		public void Step203Export()
		{
			try
			{
				PartLoadStatus partLoadStatus;
				this.workPart = this.theSession.Parts.OpenDisplay(this.part_file_name, out partLoadStatus);
				NXAPIFunctions.Step203Export(this.theSession, this.part_file_name, this.output_file_name);
			}
			catch (NXException ex)
			{
				NXAPIFunctions.Step203Export(this.theSession, this.part_file_name, this.output_file_name);
			}
		}
		public void SavePart()
		{
			this.logger.Info("Saving model file");
			try
			{
				try
				{
					NXAPIFunctions.SavePart(this.theSession, this.workPart, this.save_mode);
				}
				catch (Exception ex1) { this.logger.Error(ex1.Message); }
			}
			catch (NXException ex)
			{
				this.logger.Error(ex.Message);
			}
		}
		  
    }
}
