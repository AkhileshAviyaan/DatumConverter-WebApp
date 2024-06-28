using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Conversion.Parameter
{
	public class LatLongParameter
	{
		//Radian
		public double Lat;
		public double Long;
		public double Height;

		//degree, minute, second
		public DMS dLat;
		public DMS dLong;
		public LatLongParameter()
		{

		}
		public LatLongParameter(double Lat, double Long, double height)
		{
			this.Lat = Lat;
			this.Long = Long;
			this.Height = height;
			dLat = new DMS(Lat);
			dLong = new DMS(Long);
		}
		public LatLongParameter(DMS Lat, DMS Long, double height)
		{
			this.dLat = Lat;
			this.dLong = Long;
			this.Height = height;

			this.Lat = Lat.radian;
			this.Long = Long.radian;
		}
	}
}
