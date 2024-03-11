#nullable enable
namespace System
{
	public static class MathExtensions
	{
		public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo) =>
			(value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
		
		public static double Map(this double value, double inputFrom, double inputTo, double outputFrom, double outputTo) =>
			(value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
		
		public static float Lerp(this float value, float outputFrom, float outputTo) =>
			(1 - value) * outputFrom + value * outputTo;
		
		public static double Lerp(this double value, double outputFrom, double outputTo) =>
			(1 - value) * outputFrom + value * outputTo;
		
		/// <summary>
		/// Returns positive mod of value
		/// </summary>
		public static float Mod(this float value, float mod)
		{
			float res = value % mod;
			if (res < 0) res += mod;
			return res;
		}
		
		/// <summary>
		/// Returns positive mod of value
		/// </summary>
		public static double Mod(this double value, double mod)
		{
			double res = value % mod;
			if (res < 0) res += mod;
			return res;
		}
		
		/// <summary>
		/// Returns positive mod of value
		/// </summary>
		public static int Mod(this int value, int mod)
		{
			int res = value % mod;
			if (res < 0) res += mod;
			return res;
		}
		
		/// <summary>
		/// Returns mod of value between -mod/2 and mod/2
		/// </summary>
		public static float ModAround(this float value, float mod)
		{
			float res = value.Mod(mod);
			if (res > mod / 2)
				res -= mod;
			return res;
		}
		
		/// <summary>
		/// Returns mod of value between -mod/2 and mod/2
		/// </summary>
		public static double ModAround(this double value, double mod)
		{
			double res = value.Mod(mod);
			if (res > mod / 2)
				res -= mod;
			return res;
		}
		
		public static int Add(int a, int b) => a + b;
		public static float Add(float a, float b) => a + b;
		public static double Add(double a, double b) => a + b;
	}
}