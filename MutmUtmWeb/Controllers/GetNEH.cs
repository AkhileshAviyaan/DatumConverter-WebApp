using Conversion.ConversionMethod;
using Conversion.Parameter;
using Conversion.Transform;
using Microsoft.AspNetCore.Mvc;
using MutmUtmConverstion.Data;
using MutmUtmWeb.Src;

namespace MutmUtmWeb.Controllers
{
	public class GetNEH : Controller
	{
		TransformConstant Tc = new TransformConstant();
		public IActionResult Index()
		{
			return View(Tc);
		}
		[HttpPost]
		public IActionResult UploadCSV(string Ec1, string Pc1, string Ec2, string Pc2, string filePath, string parameterPath)
		{
			if (Ec1 is not null)
			{
				foreach (var item in Tc.Ecs)
				{
					if (item.Label == Ec1)
					{
						Tc.SelectedEc1 = item;
					}
				}
			}
			if (Pc1 is not null)
			{
				foreach (var item in Tc.Pcs)
				{
					if (item.Label == Pc1)
					{
						Tc.SelectedPc1 = item;
					}
				}
			}
			if (Ec2 is not null)
			{
				foreach (var item in Tc.Ecs)
				{
					if (item.Label == Ec2)
					{
						Tc.SelectedEc2 = item;
					}
				}
			}
			if (Pc2 is not null)
			{
				foreach (var item in Tc.Pcs)
				{
					if (item.Label == Pc2)
					{
						Tc.SelectedPc2 = item;
					}
				}
			}
			if (filePath is not null)
			{
				Tc.FilePath = filePath;
				Tc.NehReadOnly = new NehReadOnly(filePath);
			}
			if (parameterPath is not null)
			{
				Tc.FilePath = parameterPath;
				SevenParameterRead sevenParameterRead = new SevenParameterRead(parameterPath);
				Tc.SevenParameters = sevenParameterRead.sevenParameters;
			}
			CalculateNeh();
			return View("Index", Tc);
		}
		public void CalculateNeh()
		{
			TransformClass tf1 = new TransformClass(this.Tc.SelectedEc1, this.Tc.SelectedPc1);
			TransformClass tf2 = new TransformClass(this.Tc.SelectedEc2, this.Tc.SelectedPc2);
			LSM lsm = new LSM(this.Tc.NehReadOnly.Neh, tf1, tf2, Tc.SevenParameters);
			Tc.NehResult = lsm.MatrixToGenericList<NehParameter>(lsm.B);
		}
	}
}