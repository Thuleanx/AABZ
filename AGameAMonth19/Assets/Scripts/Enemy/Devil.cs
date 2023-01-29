using UnityEngine;
using Utils;
using Modules.Combat;

namespace Modules.Enemy
{
	public class Devil : MonoBehaviour
	{
		const int BPM = 80;

		public enum State {
			Spawning,
			Charging,
			Idle
		}

		State _state;
		[SerializeField, Tooltip("in multiples of BPM")] float _shotChargingTime; // in multiples of bpm
		[SerializeField, Tooltip("in multiples of BPM")] float _shotCooldownTime;
		[SerializeField] int _spawnTime;
		[SerializeField] PooledObjectIndex _bulletObj;
		float _lastTimeMarker; 

		[field:SerializeField] public float BulletSpeed {get; private set; }

		void OnEnable() {
			_state = State.Spawning;
		}

		void Shoot() {
			AllyTarget target = null;
			{ // get closest target
				float closestDistance = -1;
				foreach (AllyTarget candidate in AllyTarget.AllTargets)
				{
					float distanceToCandidate = Vector2.Distance(candidate.transform.position, transform.position);
					if (target == null || distanceToCandidate < closestDistance) {
						target = candidate;
						closestDistance = distanceToCandidate;
					}
				}
			}

			Vector2 bulletDirection = (target.transform.position - transform.position).normalized;

			GameObject projectileObj = ObjectPooler.Instance.GetPooledObject(_bulletObj);
			projectileObj.transform.position = transform.position;
			projectileObj.transform.rotation = Quaternion.identity;
			projectileObj.SetActive(true);

			Projectile projectile = projectileObj.GetComponent<Projectile>();
			projectile.Initialize(bulletDirection * BulletSpeed, rotatedToFaceDir: true);
		}

		void Update() {
			if (_state == State.Spawning) 		UpdateSpawn();
			else if (_state == State.Idle) 		UpdateIdle();
			else if (_state == State.Charging)  UpdateCharging();
		}

		void UpdateSpawn() {
			float currentTime = Time.time;
			// TODO: animate spawn

			if (currentTime - _lastTimeMarker >= _shotChargingTime * 60f / BPM) {
				_state = State.Idle;
				_lastTimeMarker = currentTime;
			}
		}

		void UpdateCharging() {
			float currentTime = Time.time;
			// TODO: animate charging

			if (currentTime - _lastTimeMarker >= _shotChargingTime * 60f / BPM)
			{
				Shoot();
				_state = State.Idle;
				_lastTimeMarker = currentTime;
			}
		}

		void UpdateIdle() {
			float currentTime = Time.time;
			if (currentTime - _lastTimeMarker >= _shotCooldownTime * 60f / BPM)
			{
				_state = State.Charging;
				_lastTimeMarker = currentTime;
			}
		}
	}
}