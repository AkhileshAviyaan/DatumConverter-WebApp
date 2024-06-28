using Conversion.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MutmUtmConverstion.Data
{
	public class NehReadOnly
	{
		string Path;
		public NehReadOnly(string path)
		{
			this.Path = path;
			read();
		}
		public List<NehParameter> Neh = [];

		public void read()
		{
			using (StreamReader reader = new StreamReader(Path))
			{
				string line = reader.ReadLine();
				string[] lines;
				while (line is not null)
				{
					lines = line.Split(',');
					Neh.Add(new NehParameter(Con(lines[1]), Con(lines[0]), 0));
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
