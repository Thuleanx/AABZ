using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
	private PlayerInputData _inputData;
	private Rigidbody2D     _rigidbody;

  // Override-style movement
	[SerializeField] private float _moveSpeed = 4f;
	[SerializeField] private float _accelerationLambda = 8;
	[SerializeField] private float _toZeroAccelerationLambdaModifier = 1f;

	// ================== Methods

	void Awake()
	{
		_inputData = GetComponent<PlayerInputScript>().InputData;
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		move();
    interact();
  }

	// ================== Helpers

	private void move()
	{
    // Override-style movement

		Vector2 currVel   = _rigidbody.velocity;
		Vector2 targetVel = _moveSpeed * _inputData.Move;

		// Different acceleration bonuses for different inputs
		float otherDirectionBonus = 
			targetVel == Vector2.zero ? 
			_toZeroAccelerationLambdaModifier : // When decelerating to zero (no player input)
			Mathf.Max(2 * (-Vector2.Dot(currVel.normalized, targetVel.normalized) + 1), 1); // When actively accelerating in opposite direction
		_rigidbody.velocity =
			Vector2.Lerp(currVel, targetVel, 1 - Mathf.Exp(-_accelerationLambda * otherDirectionBonus * Time.deltaTime));
	}

  private void interact()
  {
    if (_inputData.LDown)
    {
      Debug.Log("LDown");
    }
  }
}
