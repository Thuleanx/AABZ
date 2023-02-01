using UnityEngine;
using System.Collections.Generic;

namespace Modules.Enemy {
	public class EnemyTarget : MonoBehaviour {
		public const int MAXIMUM_TARGETS = 40;
		public static List<EnemyTarget> AllTargets = new List<EnemyTarget>();

		void OnEnable() { AllTargets.Add(this); }
		void OnDisable() { AllTargets.Remove(this); }
	}
}