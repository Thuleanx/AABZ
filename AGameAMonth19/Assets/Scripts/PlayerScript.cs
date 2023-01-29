using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
	private PlayerInputData _inputData;
	private Rigidbody2D     _rigidbody;

  private bool _playerInCircle = false;

  // Movement
  [SerializeField] private float _moveSpeed = 3f;
	[SerializeField] private float _accelerationLambda = 6f;
	[SerializeField] private float _toZeroAccelerationLambdaModifier = 1f;

  [SerializeField] private List<GameObject> _witches;

  // Interaction
  private GameObject _currentWitch = null;
  private bool _canPerfectDeflect = true;
  [SerializeField] private float _perfectDeflectWindow   = 0.075f;
  [SerializeField] private float _perfectDeflectCooldown = 0.125f;

  // Temporary
  private Color _white  = new Color(1f, 1f, 1f, 1f);
  private Color _red    = new Color(1f, 0f, 0f, 1f);
  private Color _yellow = new Color(1f, 1f, 0f, 1f);

  // ================== Methods

  void Awake()
	{
		_inputData = GetComponent<PlayerInputScript>().InputData;
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
    checkInCircle();
    handleMovement();
    handleInteraction();
  }

	// ================== Helpers

  private void checkInCircle()
  {
    _playerInCircle = !!Physics2D.OverlapPoint(_rigidbody.position, LayerMask.GetMask("BloodCircle"));
  }

	private void handleMovement()
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

  private void handleInteraction()
  {
    if (!_inputData.LDown) return;

    // Handle mouse click
    // how to handle target changes??
    if (_canPerfectDeflect)
    {
      setCurrentWitch();

      if (!_currentWitch) return;

      StartCoroutine(startPerfectDeflect());
    }
  }
  
  private IEnumerator startPerfectDeflect()
  {
    _canPerfectDeflect = false;

    // Start perfect deflect
    _currentWitch.GetComponent<SpriteRenderer>().material.color = _red;

    yield return new WaitForSeconds(_perfectDeflectWindow);

    // End perfect deflect
    _currentWitch.GetComponent<SpriteRenderer>().material.color = _white;

    // Start continuous deflect
    while (_inputData.LDown)
    {
      if (_playerInCircle)
      {
        foreach (GameObject witch in _witches)
        {
          witch.GetComponent<SpriteRenderer>().material.color = _yellow;
        }
      }
      else if (_currentWitch)
      {
        foreach (GameObject witch in _witches)
        {
          witch.GetComponent<SpriteRenderer>().material.color = _white;
        }
        _currentWitch.GetComponent<SpriteRenderer>().material.color = _yellow;
      }

      yield return null;
    }

    // End continuous deflect
    foreach (GameObject witch in _witches)
    {
      witch.GetComponent<SpriteRenderer>().material.color = _white;
    }

    // Start cooldown
    yield return new WaitForSeconds(_perfectDeflectCooldown);
    
    _canPerfectDeflect = true;

  }

  private void setCurrentWitch()
  {
    Vector3 worldSpacePos = Camera.main.ScreenToWorldPoint(new Vector3(
      _inputData.Mouse.x,
      _inputData.Mouse.y,
      Camera.main.nearClipPlane));

    // Select correct witch
    Collider2D mouseInteractionCollider = Physics2D.OverlapPoint(
      worldSpacePos,
      LayerMask.GetMask("MouseInteraction"));

    // Set
    _currentWitch = mouseInteractionCollider ? mouseInteractionCollider.transform.parent.gameObject : null;
  }
}
