using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private PlayerInputData _inputData;
	private Rigidbody2D     _rigidbody;

	[SerializeField] private float _moveSpeed = 5;

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
		_rigidbody.velocity = _moveSpeed * _inputData.Move;
	}
}
