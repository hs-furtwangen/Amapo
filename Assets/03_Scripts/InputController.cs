using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputController
{
    public static InputData GetInputData()
    {
        return new InputData
        {
            Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")),
            Jump = Input.GetKeyDown(KeyCode.Space),
            ChangeDaytime = Input.GetKeyDown(KeyCode.LeftShift),
            PauseGame = Input.GetKeyDown(KeyCode.Escape),
            ResumeGame = Input.GetMouseButtonDown(0)
        };
    }
}
