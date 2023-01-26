using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputData {
	public Vector2 Move       { get; internal set; }
	public Vector2 Look       { get; internal set; }
	public bool    PolarityCW { get; internal set; }
	public bool    LDown      { get; internal set; }
	public bool    RDown      { get; internal set; }
}

public class PlayerInputScript : MonoBehaviour
{
	// ================== Accessors

	public PlayerInputData InputData { get; private set; } = new PlayerInputData();

	// ================== Methods

	public void OnMove(InputAction.CallbackContext context)
	{
		InputData.Move = context.ReadValue<Vector2>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		InputData.Look = context.ReadValue<Vector2>();
	}

	public void OnPolaritySwitch(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			InputData.PolarityCW = !InputData.PolarityCW;
		}
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

	public void OnRightClick(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			InputData.RDown = true;
		}
		else if (context.canceled)
		{
			InputData.RDown = false;
		}
	}
}
