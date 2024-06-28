using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversion.Parameter
{
	public class NehParameter
	{
		public double N;
		public double E;
		public double H;
		public NehParameter() { }
		public NehParameter(double n, double e, double h) { this.N = n; this.E = e; this.H = h; }
	}
}
