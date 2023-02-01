using UnityEngine;
using System.Collections.Generic;

namespace Utils {
	public class Algorithm : MonoBehaviour {
		public static List<Vector2> ComputeHull(List<Vector2> points) {
			// from https://github.com/kth-competitive-programming/kactl/blob/main/content/geometry/ConvexHull.h
			if (points.Count <= 1) return points;
			points.Sort((Vector2 a, Vector2 b) => {
				if (a.x != b.x) return (int) Mathf.Sign(a.x - b.x);
				return (int) Mathf.Sign(a.y - b.y);
			});
			Vector2[] hull = new Vector2[points.Count + 1];
			int s = 0, t = 0;
			for (int it = 2; it --> 0; s = --t, points.Reverse()) {
				foreach (Vector2 p in points) {
					while (t >= s+2 && Vector3.Cross(hull[t-1] - hull[t-2], p - hull[t-2]).z <= 0) t--;
					hull[t++] = p;
				}
			}
			return new List<Vector2>(hull).GetRange(0, t - ((t == 2 && hull[0] == hull[1]) ? 1 : 0));
		}
	}
}