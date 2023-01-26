using UnityEngine;
using UnityEngine.Assertions;
using Utils;

public class EnemyScript : MonoBehaviour
{
	static int LINE_OF_SIGHT_CASTS = 12;

	[SerializeField] private LayerMask _targetMask;
	[Tooltip("attacks per second"), SerializeField] private float _attackSpeed;
	[SerializeField] private ProjectileScript _projectile;
	[SerializeField] private float _attackRange;
	[SerializeField] private float _bulletSpeed;

	private Transform _target;
	private Timer _attackCooldown;

	private void Update()
	{
		FindTarget();
		if (!_attackCooldown && _target)
		{
			Attack();
			_attackCooldown = 1.0f / _attackSpeed;
		}
	}

	private void FindTarget()
	{
		_target = null;
		float distToCurrentTarget = 100f; // something really high

		// cast ray radially around position, find closest target
		for (int theta = 0; theta < LINE_OF_SIGHT_CASTS; theta++)
		{
			float angle = theta * 360 / LINE_OF_SIGHT_CASTS;
			Vector2 castDir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

			Debug.DrawRay(transform.position, castDir, Color.red, 0.2f);

			RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir, _attackRange, _targetMask);
			if (hit)
			{
				if (!_target || hit.distance < distToCurrentTarget)
				{
					distToCurrentTarget = hit.distance;
					_target = hit.collider.gameObject.transform;
				}
			}
		}
	}

	private void Attack()
	{
		if (!_target) return;

		Vector2 atkDir = _target.transform.position - transform.position;
		if (atkDir != Vector2.zero) atkDir = atkDir.normalized;

		// Grab a bullet
		GameObject projectile = ObjectPoolerScript.Instance.GetPooledObject(PooledObjectIndex.Bullet);
		projectile.transform.position = transform.position;
		projectile.transform.rotation = Quaternion.identity;
		projectile.SetActive(true);

		// Send it off
		ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
		projectileScript.Initialize(atkDir * _bulletSpeed, true);
	}
}