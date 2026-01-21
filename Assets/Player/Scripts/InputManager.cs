using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    WeaponOne,
    WeaponTwo,
    ConsumableOne,
    ConsumableTwo,
    Shield,
    Dash,
    Menu,
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Dictionary<InputType, KeyCode> keyCodes = new Dictionary<InputType, KeyCode>();
    public bool receivingInputs = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetDefaultInput();
    }

    void Update()
    {
        if (PressMenu())
        {
            receivingInputs = !receivingInputs;
            if (!receivingInputs)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    private void SetDefaultInput()
    {
        keyCodes[InputType.WeaponOne] = KeyCode.Mouse0;
        keyCodes[InputType.WeaponTwo] = KeyCode.Mouse1;
        keyCodes[InputType.ConsumableOne] = KeyCode.Q;
        keyCodes[InputType.ConsumableTwo] = KeyCode.E;
        keyCodes[InputType.Shield] = KeyCode.LeftShift;
        keyCodes[InputType.Dash] = KeyCode.Space;
        keyCodes[InputType.Menu] = KeyCode.Tab;
    }

    public bool PressMenu()
    {
        return Input.GetKeyDown(keyCodes[InputType.Menu]) || Input.GetKeyDown(KeyCode.Escape);
    }
}
