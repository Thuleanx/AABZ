using UnityEngine;
using System.Collections.Generic;
using Utils;

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

			Vector2 spawnPos = Vector2.zero;
			int playerIndex = AllyTarget.AllTargets.FindIndex((target) => {
				return target.tag == "Player";
			});

			List<Vector2> allyHull = new List<Vector2>();
			// find if pos is in convex hull of all allies (not player)
			for (int i = 0; i < AllyTarget.AllTargets.Count; i++) if (i != playerIndex)
				allyHull.Add(AllyTarget.AllTargets[i].transform.position);
			allyHull = Algorithm.ComputeHull(allyHull);
			
			do {
				int allyIndex = Random.Range(0, allyHull.Count);

				float angle = Random.Range(0, Mathf.PI * 2);
				Vector2 offset = (new Vector2(Mathf.Sin(angle), Mathf.Cos(angle))) * distanceToCandidate;

				spawnPos = offset + allyHull[allyIndex];

				// see if point is inside of convex hull
				if (allyHull.Count <= 2) break;
				bool outside = Vector3.Cross(allyHull[(allyIndex+1) % allyHull.Count] - allyHull[allyIndex], spawnPos - allyHull[allyIndex]).z <= 0 || 
					Vector3.Cross(allyHull[allyIndex+1] - allyHull[(allyIndex-1 + allyHull.Count)%allyHull.Count], spawnPos - allyHull[(allyIndex-1 + allyHull.Count)%allyHull.Count]).z <= 0;

				if (!outside) continue;

				bool closestToIndex = true;
				foreach (Vector2 allyPos in allyHull) {
					if (Vector2.Distance(allyPos, spawnPos) < distanceToCandidate) {
						closestToIndex = false;
						break;
					}
				}

				if (closestToIndex) break;

			} while (true);
		}
	}
}