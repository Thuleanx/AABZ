using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileScript : MonoBehaviour
{
	[SerializeField] private float _lifetime = 3;
	private Rigidbody2D _rigidbody;

	// ================== Accessors

	private Vector2 _velocityLastFrame; // need this because when collision happens, our speed is potentially decreased
	public Vector2 Velocity { get => _rigidbody.velocity; set => _rigidbody.velocity = value; }
	public Timer CurrentLifetime {get; private set; }

	// ================== Methods

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		CurrentLifetime = _lifetime;
	}

	private void Update()
	{
		if (!CurrentLifetime) gameObject.SetActive(false);
	}

	private void LateUpdate()
	{
		_velocityLastFrame = Velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 collisionNormal = collision.contacts[0].normal;
		// WARNING: this will override the bullet speed. 
		Velocity = Vector2.Reflect(_velocityLastFrame, collisionNormal);
	}

	public void Initialize(Vector2 velocity, bool rotatedToFaceDir)
	{
		Velocity = velocity;
		_velocityLastFrame = Velocity;
		if (rotatedToFaceDir) transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x));
	}
}