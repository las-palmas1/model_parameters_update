using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClassLibrary1
{
    public class Functions
    {
		public static bool CompareTwoParametersFiles(string filename1, string filename2)
		{
			bool result;
			if (ReadParameters(filename1).Length > 0 && ReadParameters(filename2).Length > 0)
			{
				Dictionary<string, string[]> dict1 = GetDictFromStringArr(ReadParameters(filename1));
				Dictionary<string, string[]> dict2 = GetDictFromStringArr(ReadParameters(filename2));
				result = CompareTwoDict(dict1, dict2);
			}
			else
			{
				result = false;
			}
			return result;	
		}
		public static bool CompareTwoDict(Dictionary<string, string[]> dict1, Dictionary<string, string[]> dict2)
		{
			bool result = true;
			foreach (string i1 in dict1.Keys)
			{
				if (dict2.Keys.Count(s => s == i1) == 0)
				{
					result = false;
				}
				else
				{
					for (int i2 = 0; i2 < dict1[i1].Length; i2++ )
					{
						if (dict2[i1][i2] != dict1[i1][i2])
						{
							result = false;
						}
					}
				}
			}
			return result;
		}
		public static Dictionary<string, string[]> GetDictFromStringArr(string[][] string_arr)
		{
			Dictionary<string, string[]> result = new Dictionary<string, string[]>();
			foreach (string[] i in string_arr)
			{
				result[i[1]] = i;
			}
			return result;
		}
        public static string [][] ReadParameters(string par_file_name)
        {
            StreamReader sr = new StreamReader(par_file_name);
            string line;
            List<string> list = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
			sr.Close();
            string[][] result = new string[list.Count][];

            for (int i1 = 0; i1 < list.Count; i1++)
            {
                int word_number = 0;
                for (int i2 = 0; i2 < list[i1].Length - 1; i2++)
                {
                    if (list[i1][i2] != ' ' && list[i1][i2 + 1] == ' ')
                    {
                        word_number += 1;
                    }
                }
                if (list[i1][list[i1].Length - 1] != ' ')
                {
                    word_number += 1;
                }
                result[i1] = new string[word_number];
                int k_word = 0;
                List<StringBuilder> word_list = new List<StringBuilder>();
                word_list.Add(new StringBuilder());
                for (int i2 = 0; i2 < list[i1].Length; i2++)
                {
                    if (list[i1][i2] != ' ')
                    {
                        word_list[k_word].Append(list[i1][i2]);
                    }
                    if (i2 < list[i1].Length - 1 && list[i1][i2] != ' ' && list[i1][i2 + 1] == ' ')
                    {
                        k_word += 1;
                        word_list.Add(new StringBuilder());
                    }
                }
                for (int i2 = 0; i2 < word_number; i2++)
                {
                    result[i1][i2] = word_list[i2].ToString();
                }
            }
            return result;
        }

        public static string ReadPath(string path_file_name)
        {
            StreamReader sr = new StreamReader(path_file_name, Encoding.Default);
			string result = sr.ReadLine();
			sr.Close();
            return result;
        }
    }
}
