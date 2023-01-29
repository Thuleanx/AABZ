using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
	[SerializeField] private int _maxHealth = 10;

	public UnityEvent<Status> OnDeath;
	public UnityEvent<Status> OnHit;

	[field: SerializeField, ProgressBar("Health", 10)]
	public int Health { get; private set; }
	public bool IsDead => Health == 0;

	private void OnEnable()
	{
		Health = _maxHealth;
	}

	/// <summary>
	/// Accounts for an instance of damage. Will trigger <see cref="OnDeath"> if Health falls to zero.
	/// </summary>
	/// <param name="dmg"></param>
	public void TakeDamage(int dmg, bool isHit)
	{
		if (Health > 0)
		{
			Health = Mathf.Max(Health - dmg, 0);
			if (Health == 0) OnDeath?.Invoke(this);
			else if (isHit) OnHit?.Invoke(this);
		}
	}
}