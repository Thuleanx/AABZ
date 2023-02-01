using UnityEngine;

namespace Modules.Enemy {
	[CreateAssetMenu(fileName = "SpawnCard", menuName = "~/SpawnCard", order = 0)]
	public class SpawnCard : ScriptableObject {
		[field:SerializeField, Range(1, 32)] public int BeatInterval {get; private set; }
		[field:SerializeField, Range(1, 32)] public int BeatOffset {get; private set; }
		[field:SerializeField, Range(1, 300)] public int Weight {get; private set; }
		[field:SerializeField, Range(1, 300)] public int Cost {get; private set; }
		[field:SerializeField] public PooledObjectIndex EnemyPoolIndex { get; private set; }
	}
}
