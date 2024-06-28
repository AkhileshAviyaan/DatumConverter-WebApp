using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
namespace Conversion.Parameter
{
	public class DMS
	{
		public double degree;
		public double min;
		public double sec;
		public double radian;
		public DMS(double degree, double min, double sec)
		{
			this.degree = degree;
			this.min = min;
			this.sec = sec;
			this.radian= (this.degree + this.min / 60 + this.sec / 3600) * PI / 180;
		}
		public DMS(double radian)
		{
			this.radian= radian;
			double deg = radian * 180 / PI;
			this.degree = (int)deg;
			this.min = (int)((deg - degree) * 60) ;
			this.sec = (deg -degree-min/60)*3600;
		}
	}

}
