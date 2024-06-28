using Conversion.Parameter;
using MutmUtmConverstion.Data;

namespace MutmUtmWeb.Src
{
	public class TransformConstant
	{
		public List<EllipsoidalConstant> Ecs { get; set; }
		public List<ProjectionConstant> Pcs { get; set; }
		public EllipsoidalConstant SelectedEc1 { get; set; }
		public ProjectionConstant SelectedPc1 { get; set; }
        public EllipsoidalConstant SelectedEc2 { get; set; }
        public ProjectionConstant SelectedPc2 { get; set; }
        public NehRead NehRead { get; set; }
        public NehReadOnly NehReadOnly { get; set; }
		public List<NehParameter> NehResult { get; set; }
		public SevenParameter SevenParameters { get; set; }
        public string ParameterPath { get; set; }
		public string FilePath { get; set; }
		public TransformConstant()
		{
			EllipsoidalConstant Airy1830 = new(6377563.396, 6356256.909, "Airy1830");
			EllipsoidalConstant Grs80 = new(6378137.00, 6356752.3141, "Grs80");
			EllipsoidalConstant Everest1830 = new() { a = 6377276.345, f = 1 / 300.8017, Label = "Everest1830" };
			EllipsoidalConstant Wgs84 = new() { a = 6378137, f = 1 / 298.257223563, Label = "Wgs84" };

			ProjectionConstant NationalGrid = new(-100000, 400000, 0.9996012717, 49, -2, "NationalGrid");
			ProjectionConstant Utm44 = new(0, 500000, 0.9996, 0, 81, "Utm44");
			ProjectionConstant Utm45 = new(0, 500000, 0.9996, 0, 87, "Utm45");
			ProjectionConstant Mutm81 = new(0, 500000, 0.9999, 0, 81, "Mutm81");
			ProjectionConstant Mutm84 = new(0, 500000, 0.9999, 0, 84, "Mutm84");
			ProjectionConstant Mutm87 = new(0, 500000, 0.9999, 0, 87, "Mutm87");
			Ecs = [];
			Pcs = [];
			Ecs.AddRange([Airy1830,Grs80,Everest1830,Wgs84]);
			Pcs.AddRange([NationalGrid,Utm44,Utm45,Mutm81,Mutm84,Mutm87]);
		}
	}
}
