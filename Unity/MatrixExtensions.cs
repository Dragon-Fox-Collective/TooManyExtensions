#if UNITY_5_3_OR_NEWER
namespace UnityEngine
{
	public static class MatrixExtensions
	{
		// From PhysX
		// indexed rotation around axis, with sine and cosine of half-angle
		private static Quaternion IndexedRotation(int axis, float s, float c)
		{
			Vector3 v = Vector3.zero;
			v[axis] = s;
			return new Quaternion(v[0], v[1], v[2], c);
		}
		
		// From PhysX. I *sure hope* this just works with 4x4 matrices
		public static Vector3 Diagonalize(this Matrix4x4 m, out Quaternion massFrame)
		{
			// jacobi rotation using quaternions (from an idea of Stan Melax, with fix for precision issues)
			
			const int maxIters = 24;
			
			Quaternion q = Quaternion.identity;
			Matrix4x4 d = Matrix4x4.identity;
			
			for (int i = 0; i < maxIters; i++)
			{
				Matrix4x4 axes = Matrix4x4.Rotate(q);
				d = axes.transpose * m * axes;
				
				float d0 = Mathf.Abs(d[1, 2]), d1 = Mathf.Abs(d[0, 2]), d2 = Mathf.Abs(d[0, 1]);
				int a = d0 > d1 && d0 > d2 ? 0 : d1 > d2 ? 1 : 2; // rotation axis index, from largest off-diagonal element
				
				int a1 = (a + 1) % 3, a2 = (a1 + 1) % 3;
				if (d[a1, a2] == 0.0f || Mathf.Abs(d[a1, a1] - d[a2, a2]) > 2e6f * Mathf.Abs(2.0f * d[a1, a2]))
					break;
				
				float w = (d[a1, a1] - d[a2, a2]) / (2.0f * d[a1, a2]); // cot(2 * phi), where phi is the rotation angle
				float absw = Mathf.Abs(w);
				
				Quaternion r;
				if (absw > 1000)
					r = IndexedRotation(a, 1 / (4 * w), 1); // h will be very close to 1, so use small angle approx instead
				else
				{
					float t = 1 / (absw + Mathf.Sqrt(w * w + 1)); // absolute value of tan phi
					float h = 1 / Mathf.Sqrt(t * t + 1);          // absolute value of cos phi
					
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					Debug.Assert(h != 1); // |w|<1000 guarantees this with typical IEEE754 machine eps (approx 6e-8)
					r = IndexedRotation(a, Mathf.Sqrt((1 - h) / 2) * Mathf.Sign(w), Mathf.Sqrt((1 + h) / 2));
				}
				
				q = (q * r).normalized;
			}
			
			massFrame = q;
			return new Vector3(d[0, 0], d[1, 1], d[2, 2]);
		}
		
		public static Matrix4x4 Plus(this Matrix4x4 a, Matrix4x4 b)
		{
			Matrix4x4 rtn = Matrix4x4.zero;
			for (int i = 0; i < 16; i++)
				rtn[i] = a[i] + b[i];
			return rtn;
		}
		
		public static Matrix4x4 Minus(this Matrix4x4 a, Matrix4x4 b)
		{
			Matrix4x4 rtn = Matrix4x4.zero;
			for (int i = 0; i < 16; i++)
				rtn[i] = a[i] - b[i];
			return rtn;
		}
		
		public static Matrix4x4 Times(this Matrix4x4 a, float b)
		{
			Matrix4x4 rtn = Matrix4x4.zero;
			for (int i = 0; i < 16; i++)
				rtn[i] = a[i] * b;
			return rtn;
		}
	}
}
#endif