using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileScript : MonoBehaviour
{
	[SerializeField] private float _lifetime = 3;
	private Rigidbody2D _rigidbody;

  	// ================== Accessors
	public Vector2 Velocity { get => _rigidbody.velocity; set => _rigidbody.velocity = value; }
	public Timer CurrentLifetime {get; private set; }

  	// ================== Methods

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnEnable() {
		CurrentLifetime = _lifetime;
	}

	private void Update() {
		if (!CurrentLifetime) {
			// TODO: switch to disable after implementing object pooling
			Destroy(gameObject);
		}
	}

	public void Initialize(Vector2 velocity, bool rotatedToFaceDir)
	{
		Velocity = velocity;
		if (rotatedToFaceDir) transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x));
	}
}