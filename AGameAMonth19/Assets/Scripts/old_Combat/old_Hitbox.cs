using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
	[SerializeField] private int _damage;
	[SerializeField] private UnityEvent<Hitbox> _onHit;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Status stat = other.GetComponentInParent<Status>();
		stat.TakeDamage(_damage, isHit: true);
		_onHit?.Invoke(this);
	}
}