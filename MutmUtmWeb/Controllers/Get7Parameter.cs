using Conversion.Parameter;
using Microsoft.AspNetCore.Mvc;
using MutmUtmWeb.Src;
using MutmUtmConverstion.Data;
using Conversion.ConversionMethod;
using Conversion.Transform;

namespace MutmUtmWeb.Controllers
{
	public class Get7Parameter : Controller
	{
		TransformConstant Tc = new TransformConstant();
		public IActionResult Index()
		{
			return View(Tc);
		}
		[HttpPost]
		public IActionResult UploadCSV(string Ec1, string Pc1, string Ec2, string Pc2, string filePath)
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
				Tc.NehRead = new NehRead(filePath);
			}
			CalculateSevenParameter();
			return View("Index", Tc);
		}
		public void CalculateSevenParameter() 
		{
			TransformClass tf1 = new TransformClass(this.Tc.SelectedEc1,this.Tc.SelectedPc1);
			TransformClass tf2 = new TransformClass(this.Tc.SelectedEc2,this.Tc.SelectedPc2);
			LSM lsm = new LSM(this.Tc.NehRead.NehInMUTM,tf1,this.Tc.NehRead.NehInUTM,tf2);
			Tc.SevenParameters=lsm.SevenParameters;
		}

	}
}