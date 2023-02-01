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
		[field:SerializeField, Tooltip("in multiples of 1/BPS")] public float ShotChargingTime {get; private set; } // in multiples of 1/bps
		[field:SerializeField, Tooltip("in multiples of 1/BPS")] public float SpawnTime {get; private set; } // in multiples of 1/bps
		[SerializeField] PooledObjectIndex _bulletObj;
		float _lastTimeMarker; 
		float _shotCooldownTime; // determined by spawner

		[field:SerializeField] public float BulletSpeed {get; private set; }

		void OnEnable() {
			_state = State.Spawning;
		}

		public void SetShotCooldown(float cdTime) => _shotCooldownTime = cdTime;

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
			if (target) {
				Vector2 bulletDirection = (target.transform.position - transform.position).normalized;

				GameObject projectileObj = ObjectPooler.Instance.GetPooledObject(_bulletObj);
				projectileObj.transform.position = transform.position;
				projectileObj.transform.rotation = Quaternion.identity;
				projectileObj.SetActive(true);

				Projectile projectile = projectileObj.GetComponent<Projectile>();
				projectile.Initialize(bulletDirection * BulletSpeed, rotatedToFaceDir: true);
			}
		}

		void Update() {
			if (_state == State.Spawning) 		UpdateSpawn();
			else if (_state == State.Idle) 		UpdateIdle();
			else if (_state == State.Charging)  UpdateCharging();
		}

		void UpdateSpawn() {
			float currentTime = Time.time;
			// TODO: animate spawn

			if (currentTime - _lastTimeMarker >= ShotChargingTime * 60f / BPM) {
				_state = State.Idle;
				_lastTimeMarker = currentTime;
			}
		}

		void UpdateCharging() {
			float currentTime = Time.time;
			// TODO: animate charging

			if (currentTime - _lastTimeMarker >= ShotChargingTime * 60f / BPM)
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