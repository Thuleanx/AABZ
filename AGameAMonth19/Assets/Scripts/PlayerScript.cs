using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputData))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
	private PlayerInputData _inputData;
	private Rigidbody2D     _rigidbody;

	[SerializeField] private float _moveSpeed = 7.5f;
	[SerializeField] private float _accelerationLambda = 6;
	[SerializeField] private float _toZeroAccelerationLambdaModifier = 1.2f;

	// ================== Methods

	void Awake()
	{
		_inputData = GetComponent<PlayerInputScript>().InputData;
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		move();
	}

	// ================== Helpers

	private void move()
	{
		// Can introduce lerp-based movement here later

		// - Direct, normalized WASD movement
		// _rigidbody.velocity = _moveSpeed * _inputData.Move;
		
		// - Override-style movement
		Vector2 currVel = _rigidbody.velocity;
		Vector2 targetVel = _moveSpeed * _inputData.Move;
		// Different acceleration bonuses for different inputs
		float otherDirectionBonus = 
			targetVel == Vector2.zero ? 
			_toZeroAccelerationLambdaModifier : // When decelerating to zero (no player input)
			Mathf.Max(2 * (-Vector2.Dot(currVel.normalized, targetVel.normalized) + 1), 1); // When actively accelerating in opposite direction
		_rigidbody.velocity =
			Vector2.Lerp(currVel, targetVel, 1 - Mathf.Exp(-_accelerationLambda * otherDirectionBonus * Time.deltaTime));

		// Vector2 toMouse = (_inputData.Look - (Vector2) Camera.main.WorldToScreenPoint(_rigidbody.position));
		// float toMouseMag = toMouse.magnitude;
		// if (toMouseMag < _moveSpeed) {
		//   _rigidbody.velocity = toMouse;
		// }
		// else
		// {
		//   _rigidbody.velocity = _moveSpeed * toMouse.normalized;
		// }
	}
}
