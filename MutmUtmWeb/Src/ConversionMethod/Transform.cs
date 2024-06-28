using Conversion.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
using Conversion.Utility;
namespace Conversion.Transform
{
	public class TransformClass
	{
		EllipsoidalConstant Ec;
		ProjectionConstant Pc;
		public TransformClass(EllipsoidalConstant ec, ProjectionConstant pc)
		{
			Ec = ec;
			Pc = pc;
		}
		public NehParameter LatLongToNeh(LatLongParameter latlong)
		{
			double n, v, rho, eta_square, M, I, II, III, IIIA, IV, V, VI, N, E;
			n = (Ec.a - Ec.b) / (Ec.a + Ec.b);
			v = Ec.a * Pc.Fo * Pow(1 - Pow(Ec.e * Sin(latlong.Lat), 2), -0.5);
			rho = Ec.a * Pc.Fo * (1 - Pow(Ec.e, 2)) * Pow(1 - Pow(Ec.e * Sin(latlong.Lat), 2), -1.5);
			eta_square = (v / rho - 1);
			M = this.GetM(n, latlong.Lat);
			I = M + Pc.No;
			II = v / 2 * Sin(latlong.Lat) * Cos(latlong.Lat);
			III = v / 24 * Sin(latlong.Lat) * Pow(Cos(latlong.Lat), 3) * (5 - Pow(Tan(latlong.Lat), 2) + 9 * eta_square);
			IIIA = v / 720 * Sin(latlong.Lat) * Pow(Cos(latlong.Lat), 5) * (61 - 58 * Pow(Tan(latlong.Lat), 2) + Pow(Tan(latlong.Lat), 4));
			IV = v * Cos(latlong.Lat);
			V = v / 6 * Pow(Cos(latlong.Lat), 3) * (v / rho - Pow(Tan(latlong.Lat), 2));
			VI = v / 120 * Pow(Cos(latlong.Lat), 5) * (5 - 18 * Pow(Tan(latlong.Lat), 2) + Pow(Tan(latlong.Lat), 4) + 14 * eta_square - 58 * Pow(Tan(latlong.Lat), 2) * eta_square);

			E = Pc.Eo + IV * (latlong.Long - Pc.Lamdao) + V * Pow(latlong.Long - Pc.Lamdao, 3) + VI * Pow(latlong.Long - Pc.Lamdao, 5);
			N = I + II * Pow(latlong.Long - Pc.Lamdao, 2) + III * Pow(latlong.Long - Pc.Lamdao, 4) + IIIA * Pow(latlong.Long - Pc.Lamdao, 6);

			return new(N, E, latlong.Height);
		}
		public XyzParameter LatLongToXyz(LatLongParameter latlong)
		{
			double vc;
	
			vc = Ec.a / (Sqrt(1 - Ec.e2 * Pow(Sin(latlong.Lat), 2)));

			double x = (vc + latlong.Height) * Cos(latlong.Lat) * Cos(latlong.Long);
			double y = (vc + latlong.Height) * Cos(latlong.Lat) * Sin(latlong.Long);
			double z = ((1 - Ec.e2) * vc + latlong.Height) * Sin(latlong.Lat);
			return new XyzParameter(x, y, z);
		}
		double GetM(double n, double phi)
		{
			double aM = phi - Pc.Phio;
			double aP = phi + Pc.Phio;
			double Mexpression1 = (1 + n + 5 / 4 * Pow(n, 2) + 5 / 4 * Pow(n, 3)) * aM;
			double Mexpression2 = (3 * n + 3 * Pow(n, 2) + 21 / 8 * Pow(n, 3)) * Sin(aM) * Cos(aP);
			double Mexpression3 = (15 / 8 * Pow(n, 2) + 15 / 8 * Pow(n, 3)) * Sin(2 * aM) * Cos(2 * aP);
			double Mexpression4 = 35 / 24 * Pow(n, 3) * Sin(3 * aM) * Cos(3 * aP);
			return Ec.b * Pc.Fo * (Mexpression1 - Mexpression2 + Mexpression3 - Mexpression4);
		}
		public LatLongParameter NeToLatlong(NehParameter neh)
		{
			double phi, phi_final, phi_new, lamda_final, n, v, rho, eta_square, M, VII, VIII, IX, X, XI, XII, XIIA;
			n = (Ec.a - Ec.b) / (Ec.a + Ec.b);
			phi = (neh.N - Pc.No) / (Ec.a * Pc.Fo) + Pc.Phio;
			M = this.GetM(n, phi);
			while ((neh.N - Pc.No - M) > 1e-5)
			{
				phi_new = (neh.N - Pc.No - M) / (Ec.a * Pc.Fo) + phi;
				M = this.GetM(n, phi_new);
				phi = phi_new;
				// Console.WriteLine("phi:{0}    GetM:{1}   N-Mo-GetM:{2}", phi, GetM, N - No - GetM);
			}

			v = Ec.a * Pc.Fo * Pow(1 - Pow(Ec.e * Sin(phi), 2), -0.5);
			rho = Ec.a * Pc.Fo * (1 - Pow(Ec.e, 2)) * Pow(1 - Pow(Ec.e * Sin(phi), 2), -1.5);
			eta_square = (v / rho - 1);
			VII = Tan(phi) / (2 * rho * v);
			VIII = Tan(phi) / (24 * rho * Pow(v, 3)) * (5 + 3 * Pow(Tan(phi), 2) + eta_square - 9 * Pow(Tan(phi), 2) * eta_square);
			IX = Tan(phi) / (720 * rho * Pow(v, 5)) * (61 + 90 * Pow(Tan(phi), 2) + 45 * Pow(Tan(phi), 4));
			X = 1 / (Cos(phi) * v);
			XI = 1 / (Cos(phi) * 6 * Pow(v, 3)) * (v / rho + 2 * Pow(Tan(phi), 2));
			XII = 1 / (Cos(phi) * 120 * Pow(v, 5)) * (5 + 28 * Pow(Tan(phi), 2) + 24 * Pow(Tan(phi), 4));
			XIIA = 1 / (Cos(phi) * 5040 * Pow(v, 7)) * (61 + 662 * Pow(Tan(phi), 2) + 1320 * Pow(Tan(phi), 4) + 720 * Pow(Tan(phi), 6));
			double delE = neh.E - Pc.Eo;
			phi_final = phi - VII * Pow((delE), 2) + VIII * Pow(delE, 4) - IX * Pow(neh.E - Pc.Eo, 6);
			lamda_final = Pc.Lamdao + X * (delE) - XI * Pow((delE), 3) + XII * Pow((delE), 5) - XIIA * Pow((delE), 7);
			return new(phi_final, lamda_final, neh.H);
		}
		public LatLongParameter XyzToLatlong(XyzParameter xyz)
		{
			double lamda, phi, H, p, vc, phi_new;
			lamda = Atan2(xyz.Y, xyz.X);
			p = Sqrt(Pow(xyz.X, 2) + Pow(xyz.Y, 2));
			phi = Atan2(xyz.Z, (p * (1 - Ec.e2)));
			vc = Ec.a / (Sqrt(1 - Ec.e2 * Pow(Sin(phi), 2)));
			phi_new = Atan2((xyz.Z + Ec.e2 * vc * Sin(phi)), p);
			while (Abs(phi_new - phi) > 1e-100)
			{
				phi = phi_new;
				phi_new = Atan2((xyz.Z + Ec.e2 * vc * Sin(phi)), p);
			}
			vc = Ec.a / (Sqrt(1 - Ec.e2 * Pow(Sin(phi_new), 2)));
			H = p / Cos(phi) - vc;
			return new LatLongParameter(phi_new, lamda, H);
		}

		public Matrix LatLongToNeh(Matrix A)
		{
			Matrix ReturnMatrix = new Matrix(A.RowCount,A.ColumnCount);
			for( int i = 0; i < A.RowCount; i++)
			{
				var neh = LatLongToNeh(new LatLongParameter(A.Data[i, 0], A.Data[i, 1], A.Data[i,2]));
				ReturnMatrix.Data[i, 0] = neh.N;
				ReturnMatrix.Data[i, 1] = neh.E;
				ReturnMatrix.Data[i, 2] = neh.H;
			}
			return ReturnMatrix;
		}
		public Matrix LatLongToXyz(Matrix A)
		{
			Matrix ReturnMatrix = new Matrix(A.RowCount, A.ColumnCount);
			for (int i = 0; i < A.RowCount; i++)
			{
				var xyz = LatLongToXyz(new LatLongParameter(A.Data[i, 0], A.Data[i, 1], A.Data[i, 2]));
				ReturnMatrix.Data[i, 0] = xyz.X;
				ReturnMatrix.Data[i, 1] = xyz.Y;
				ReturnMatrix.Data[i, 2] = xyz.Z;
			}
			return ReturnMatrix;
		}
		public Matrix NeToLatlong(Matrix A)
		{
			Matrix ReturnMatrix = new Matrix(A.RowCount, A.ColumnCount);
			for (int i = 0; i < A.RowCount; i++)
			{
				var Latlong = NeToLatlong(new NehParameter(A.Data[i, 0], A.Data[i, 1], A.Data[i, 2]));
				ReturnMatrix.Data[i, 0] = Latlong.Lat;
				ReturnMatrix.Data[i, 1] = Latlong.Long;
				ReturnMatrix.Data[i, 2] = Latlong.Height;
			}
			return ReturnMatrix;
		}
		public Matrix XyzToLatlong(Matrix A)
		{
			Matrix ReturnMatrix = new Matrix(A.RowCount, A.ColumnCount);
			for (int i = 0; i < A.RowCount; i++)
			{
				var Latlong = XyzToLatlong(new XyzParameter(A.Data[i, 0], A.Data[i, 1], A.Data[i, 2]));
				ReturnMatrix.Data[i, 0] = Latlong.Lat;
				ReturnMatrix.Data[i, 1] = Latlong.Long;
				ReturnMatrix.Data[i, 2] = Latlong.Height;
				ReturnMatrix.Dms[i, 0] = Latlong.dLat;
				ReturnMatrix.Dms[i, 1] = Latlong.dLong;
			}
			return ReturnMatrix;
		}
	}
}
