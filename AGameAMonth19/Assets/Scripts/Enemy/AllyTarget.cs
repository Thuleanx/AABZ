using System.Collections.Generic;
using UnityEngine;

namespace Modules.Enemy
{
	public class AllyTarget : MonoBehaviour
	{
		public static List<AllyTarget> AllTargets = new List<AllyTarget>();

		void OnEnable() { AllTargets.Add(this); }
		void OnDisable() { AllTargets.Remove(this); }
	}
}