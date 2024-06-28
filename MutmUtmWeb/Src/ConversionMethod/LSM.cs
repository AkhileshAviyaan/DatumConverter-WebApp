using Conversion.Parameter;
using Conversion.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conversion.Utility.MathHelp;
using Conversion.Transform;

namespace Conversion.ConversionMethod
{
	public class LSM
	{
		public Matrix A;
		public Matrix B;
		public Matrix SevenParameterInMatrix = new Matrix(7, 1);
		public SevenParameter SevenParameters;

		Matrix AColumnMatrix;
		Matrix BColumnMatrix;
		Matrix DelBA;
		Matrix AArranged;
		/// <summary>
		/// Constructor for populating A matrix and B matrix for Calculating 
		/// </summary>
		/// <param name="xyzA">The Reference </param>
		/// <param name="xyzB"></param>
		public LSM(List<XyzParameter> xyzA, List<XyzParameter> xyzB)
		{
			UpdateMatrixBandBA(xyzB, xyzA);
			UpdateMatrixA(xyzA);
		}
		public LSM(List<NehParameter> NehA, TransformClass tfA, List<NehParameter> NehB, TransformClass tfB)
		{
			List<XyzParameter> xyzA = NehToXyz(NehA, tfA);
			List<XyzParameter> xyzB = NehToXyz(NehB, tfB);
			UpdateMatrixBandBA(xyzB, xyzA);
			UpdateMatrixA(xyzA);
			CalculateSevenParameter();
		}
		public List<XyzParameter> NehToXyz(List<NehParameter> Neh, TransformClass tf)
		{
			List<XyzParameter> xyz = [];
			for (int i = 0; i < Neh.Count; i++)
			{
				xyz.Add(tf.LatLongToXyz(tf.NeToLatlong(Neh[i])));
			}
			return xyz;
		}
		public List<NehParameter> XyzToNeh(List<XyzParameter> xyz, TransformClass tf)
		{
			List<NehParameter> neh = [];
			for (int i = 0; i < xyz.Count; i++)
			{
				neh.Add(tf.LatLongToNeh(tf.XyzToLatlong(xyz[i])));
			}
			return neh;
		}
		public void UpdateMatrixBandBA(List<XyzParameter> xyzB, List<XyzParameter> xyzA)
		{
			BColumnMatrix = new Matrix(xyzB.Count * 3, 1);
			DelBA = new Matrix(xyzB.Count * 3, 1);

			for (int i = 0; i < xyzB.Count; i++)
			{
				BColumnMatrix.Data[0 + i * 3, 0] = xyzB[i].X;
				BColumnMatrix.Data[1 + i * 3, 0] = xyzB[i].Y;
				BColumnMatrix.Data[2 + i * 3, 0] = xyzB[i].Z;
				DelBA.Data[0 + i * 3, 0] = xyzB[i].X - xyzA[i].X;
				DelBA.Data[1 + i * 3, 0] = xyzB[i].Y - xyzA[i].Y;
				DelBA.Data[2 + i * 3, 0] = xyzB[i].Z - xyzA[i].Z;
			}
		}
		/// <summary>
		/// Matrix B will be updated as xyz result
		/// </summary>
		/// <param name="xyzA">list of xyz parameter</param>
		/// <param name="sevenParameter"></param>
		public LSM(List<XyzParameter> xyzA, SevenParameter sevenParameter)
		{
			this.SevenParameters = sevenParameter;
			UpdateMatrixA(xyzA);
			SevenParameterMatrixUpdate();
			TranformToGetB();
		}
		/// <summary>
		/// Matrix B will be updated as Neh result
		/// </summary>
		/// <param name="neh">list of neh parameter</param>
		/// <param name="a"></param>
		/// <param name="sevenParameter"></param>
		public LSM(List<NehParameter> neh, TransformClass tf1, TransformClass tf2, SevenParameter sevenParameter)
		{
			this.SevenParameters = sevenParameter;
			List<XyzParameter> xyz = NehToXyz(neh, tf1);
			UpdateMatrixA(xyz);
			SevenParameterMatrixUpdate();
			TranformToGetB();
			List<XyzParameter> objxyz = MatrixToGenericList<XyzParameter>(this.B);
			this.B = NehToMatrix(XyzToNeh(objxyz, tf2));
		}
		public List<T> MatrixToGenericList<T>(Matrix M) where T : class, new()
		{
			List<T> list = [];
			for (int i = 0; i < M.RowCount; i++)
			{
				T obj = (T)Activator.CreateInstance(typeof(T), M.Data[i, 0], M.Data[i, 1], M.Data[i, 2]);
				list.Add(obj);
			}
			return list;
		}
		 Matrix NehToMatrix(List<NehParameter> t)
		{
			Matrix M = new Matrix(t.Count, 3);
			for (int i = 0; i < t.Count; i++)
			{
				M.Data[i, 0] = t[i].N;
				M.Data[i, 1] = t[i].E;
				M.Data[i, 2] = t[i].H;
			}
			return M;
		}
		 void UpdateMatrixA(List<XyzParameter> xyz)
		{
			Matrix TempArrange = new Matrix(xyz.Count * 3, 7);
			AColumnMatrix = new Matrix(xyz.Count * 3, 1);
			Matrix TempA = new Matrix(xyz.Count, 3);
			int count = 0;
			for (int i = 0; i < xyz.Count; i++)
			{
				TempA.Data[i, 0] = xyz[i].X;
				TempA.Data[i, 1] = xyz[i].Y;
				TempA.Data[i, 2] = xyz[i].Z;

				AColumnMatrix.Data[0 + i * 3, 0] = xyz[i].X;
				AColumnMatrix.Data[1 + i * 3, 0] = xyz[i].Y;
				AColumnMatrix.Data[2 + i * 3, 0] = xyz[i].Z;

				for (int j = 0; j < 3; j++)
				{

					for (int k = 0; k < 6; k++)
					{
						TempArrange.Data[count, k] = 0;
						if (j == 0)
						{
							TempArrange.Data[count, j] = 1;
							TempArrange.Data[count, 3] = 0;
							TempArrange.Data[count, 4] = xyz[i].Z;
							TempArrange.Data[count, 5] = -xyz[i].Y;
							TempArrange.Data[count, 6] = xyz[i].X;
						}
						else if (j == 1)
						{
							TempArrange.Data[count, j] = 1;
							TempArrange.Data[count, 3] = -xyz[i].Z;
							TempArrange.Data[count, 4] = 0;
							TempArrange.Data[count, 5] = xyz[i].X;
							TempArrange.Data[count, 6] = xyz[i].Y;
						}
						else if (j == 2)
						{
							TempArrange.Data[count, j] = 1;
							TempArrange.Data[count, 3] = xyz[i].Y;
							TempArrange.Data[count, 4] = -xyz[i].X;
							TempArrange.Data[count, 5] = 0;
							TempArrange.Data[count, 6] = xyz[i].Z;
						}
					}
					count++;
				}
			}
			this.AArranged = TempArrange;
			this.A = TempA;
		}
		/// <summary>
		/// Transform Matrix A with Seven parameter to get New Matrix B
		/// </summary>
		/// <returns></returns>
		 Matrix TranformToGetB()
		{
			Matrix Result = AArranged * SevenParameterInMatrix;
			Matrix ArrangedResult = new Matrix(Result.RowCount / 3, 3);

			for (int i = 0; i < Result.RowCount / 3; i++)
			{
				ArrangedResult.Data[i, 0] = Result.Data[0 + i * 3, 0];
				ArrangedResult.Data[i, 1] = Result.Data[1 + i * 3, 0];
				ArrangedResult.Data[i, 2] = Result.Data[2 + i * 3, 0];
			}
			this.B = this.A + ArrangedResult;
			return this.B;
		}
		 void CalculateSevenParameter()
		{
			this.SevenParameterInMatrix = (AArranged.Transpose * AArranged).Inverse * (AArranged.Transpose * DelBA);
			MatrixTo7Parameter();
		}
		 void MatrixTo7Parameter()
		{
			var M = this.SevenParameterInMatrix.Data;
			this.SevenParameters = new SevenParameter(M[0, 0], M[1, 0], M[2, 0], M[3, 0], M[4, 0], M[5, 0], M[6, 0]);
		}
		 void SevenParameterMatrixUpdate()
		{
			var M = SevenParameters;
			this.SevenParameterInMatrix = new Matrix(new double[7, 1] { { M.Tx }, { M.Ty }, { M.Tz }, { M.Rx }, { M.Ry }, { M.Rz }, { M.S } });
		}
	}
}
