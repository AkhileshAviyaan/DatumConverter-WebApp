using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
namespace Conversion.Parameter
{
	public class EllipsoidalConstant
	{
		public double a;
		public double f;
		public string Label;
		public double b
		{
			get
			{
				if (a != 0)
					return a * (1 - f);
				else return 0;
			}
			set
			{
				f = value;
			}
		}

		public double e2
		{
			get
			{
				if (f != 0)
					return 2 * f - f * f;
				else if (b != 0 || a != 0)
					return (a * a - b * b) / (a * a);
				else return 0;
			}
		}
		public double e
		{
			get
			{
				return Sqrt(e2);
			}
		}
		public EllipsoidalConstant(double a, double b, string label)
		{
			this.a = a;
			this.b = b;
			Label = label;
		}
		public EllipsoidalConstant()
		{
		}

	}
}
