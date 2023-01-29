using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceFieldScript : MonoBehaviour
{
	private PlayerInputData _inputData;
	private Rigidbody2D     _rigidbody;

	[SerializeField] private float _angularSpeed = 45;

	// ================== Methods

	void Awake()
	{
		_inputData = transform.parent.gameObject.GetComponent<PlayerInputScript>().InputData;
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag != "Ball") return;
		
		// other.gameObject.GetComponent<BallScript>().AngularVelocity = 
		// 	_inputData.PolarityCW ? -_angularSpeed : _angularSpeed;

		if (!(_inputData.LDown ^ _inputData.RDown))
		{
			other.gameObject.GetComponent<BallScript>().AngularVelocity = 0;
		}
		else if (_inputData.LDown)
		{
			other.gameObject.GetComponent<BallScript>().AngularVelocity = -_angularSpeed;
		}
		else if (_inputData.RDown)
		{
			other.gameObject.GetComponent<BallScript>().AngularVelocity = +_angularSpeed;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag != "Ball") return;
		
		other.gameObject.GetComponent<BallScript>().AngularVelocity = 0;
	}
}
