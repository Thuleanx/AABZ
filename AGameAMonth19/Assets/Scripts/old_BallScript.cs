using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallScript : MonoBehaviour
{
	private Rigidbody2D _rigidbody;

	// ================== Accessors

	public float AngularVelocity
	{
		get { return _rigidbody.angularVelocity; }
		set { _rigidbody.angularVelocity = value; }
	}
	
	// ================== Methods

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}
}
