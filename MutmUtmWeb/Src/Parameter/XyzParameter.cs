using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversion.Parameter
{
	public class XyzParameter
	{
		public double X;
		public double Y;
		public double Z;
		public XyzParameter() { }
		public XyzParameter(double x, double y, double z) { this.X = x; this.Y = y; this.Z = z; }
	}
}
