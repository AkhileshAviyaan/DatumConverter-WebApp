using Conversion.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;

namespace Conversion.Parameter
{
	public class SevenParameter
	{
		#region Original Transfomation Paramater
		public double Tx;
		public double Ty;
		public double Tz;
		public double Rx;
		public double Ry;
		public double Rz;
		public double S;
		#endregion

		#region In Rotation in sec, scaling in PPM                                             
		public double SInPpm;
		public double RxInSec;
		public double RyInSec;
		public double RzInSec;
		#endregion
		public SevenParameter(double tx, double ty, double tz, double rx, double ry, double rz, double s)
		{
			Tx = tx;
			Ty = ty;
			Tz = tz; 
			Rx = rx;
			Ry = ry;
			Rz = rz;
			S = s;
			this.RxInSec = rx * 3600/ PI * 180;
			this.RyInSec = ry * 3600/ PI * 180;
			this.RzInSec = rz * 3600/ PI * 180;
			this.SInPpm = s * 1e6;
		}
		public SevenParameter() { }
	}
}
