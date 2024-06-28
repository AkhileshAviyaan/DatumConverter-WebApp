using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
namespace Conversion.Parameter
{
	public class ProjectionConstant
	{
		public double No;
		public double Eo;
		public double Fo;
		public double Phio;
		public double Lamdao;
		public string Label;
		public ProjectionConstant(double no,double eo,double fo, double phio,double lamdao,string label)
		{
			No= no; Eo = eo; Fo = fo; Phio = phio*PI/180; Lamdao = lamdao*PI/180;
			Label = label;
		}
		public ProjectionConstant()
		{
		}
	}
}
