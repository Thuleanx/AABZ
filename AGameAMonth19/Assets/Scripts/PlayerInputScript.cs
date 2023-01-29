using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

// The struct that exposes player inputs to other classes
public class PlayerInputData {
	public Vector2 Move  { get; internal set; }
  public Vector2 Mouse { get; internal set; }
  public bool    LDown { get; internal set; }
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputScript : MonoBehaviour
{
	// ================== Accessors

	public PlayerInputData InputData { get; private set; } = new PlayerInputData();

	// ================== Methods

	public void OnMove(InputAction.CallbackContext context)
	{
		InputData.Move = context.ReadValue<Vector2>();
	}

  public void OnMouse(InputAction.CallbackContext context)
	{
		InputData.Mouse = context.ReadValue<Vector2>();
	}

	public void OnLeftClick(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			InputData.LDown = true;
		}
		else if (context.canceled)
		{
			InputData.LDown = false;
		}
	}
}
