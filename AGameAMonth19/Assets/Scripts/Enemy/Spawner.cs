using UnityEngine;

namespace Modules.Enemy
{
	public class Spawner : MonoBehaviour
	{
		const int BPM = 80;
		[SerializeField] PooledObjectIndex _enemyPoolIndex;
		[SerializeField] float allyShieldRadius;

		void Update() {
		}

		void Spawn(float bulletTravelBeats) {
			GameObject enemyObj = ObjectPooler.Instance.GetPooledObject(_enemyPoolIndex);
			enemyObj.SetActive(true); // just so no other Spawn can overtake, not that it matters

			Devil enemy = enemyObj.GetComponent<Devil>();

			float bulletTravelTime = bulletTravelBeats * 60f/BPM;
			float distanceToCandidate = bulletTravelTime * enemy.BulletSpeed + allyShieldRadius;

			// pick placement
			Vector2 spawnPos = Vector2.zero;
			int playerIndex = AllyTarget.AllTargets.FindIndex((target) => {
				return target.tag == "Player";
			});
			do {
				AllyTarget target = AllyTarget.AllTargets[Random.Range(0,AllyTarget.AllTargets.Count)];
				if (target.tag == "Player") continue;

				float angle = Random.Range(0, Mathf.PI * 2);
				Vector2 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

				Vector2 pos = offset + (Vector2) target.transform.position;

				// find if pos is in convex hull of 
				for (int i = 0; i < AllyTarget.AllTargets.Count; i++) if (i != playerIndex) {
					for (int j = 0; j < AllyTarget.AllTargets.Count; j++) if (j != playerIndex) {
						for (int k = 0; k < AllyTarget.AllTargets.Count; k++) if (k != playerIndex && i != j && j != k) {
							
						}
					}
				}

			} while (true);
		}
	}
}