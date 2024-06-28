using Conversion.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MutmUtmConverstion.Data
{
	public class NehRead
	{
		string Path;
		public NehRead(string path)
		{
			this.Path = path;
			read();
		}
		public List<NehParameter> NehInUTM = [];
		public List<NehParameter> NehInMUTM = [];

		public void read()
		{
			using (StreamReader reader = new StreamReader(Path))
			{
				string line = reader.ReadLine();
				string[] lines;
				while (line is not null)
				{
					lines = line.Split(',');
					NehInMUTM.Add(new NehParameter(Con(lines[1]), Con(lines[0]), 0));
					NehInUTM.Add(new NehParameter(Con(lines[4]), Con(lines[3]), 0));
					line = reader.ReadLine();
				}
			}
		}
		public static double Con(string a)
		{
			return double.Parse(a);
		}
	}
}
