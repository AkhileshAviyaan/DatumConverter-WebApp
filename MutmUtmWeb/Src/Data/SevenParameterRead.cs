using Conversion.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
namespace MutmUtmConverstion.Data
{
	public class SevenParameterRead
	{
		string Path;
		public SevenParameterRead(string path)
		{
			this.Path = path;
			read();
		}
		public SevenParameter sevenParameters = new SevenParameter();
		
		public void read()
		{
			using (StreamReader reader = new StreamReader(Path))
			{
				string line = reader.ReadLine();
				string[] lines;
				while (line is not null)
				{
					lines = line.Split(',');
					double Tx = Con(lines[0]);
					double Ty = Con(lines[1]);
					double Tz = Con(lines[2]);
					double RxInSec = Con(lines[3]);
					double RyInSec = Con(lines[4]);
					double RzInSec = Con(lines[5]);
					double SInPpm = Con(lines[6]);
					double Rx = RxInSec / 3600 * PI / 180;
					double Ry = RyInSec / 3600 * PI / 180;
					double Rz = RzInSec / 3600 * PI / 180;
					double S = SInPpm / 1e6;
					sevenParameters=new SevenParameter(Tx,Ty,Tz,Rx,Ry,Rz,S);
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
