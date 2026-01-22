using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] private InputActionAsset actionsAsset;
    public Dictionary<InputType, InputAction> inputDict = new Dictionary<InputType, InputAction>();
    public bool receivingInputs = true;

    private const string BindingsKey = "PlayerBindings";

    private void Awake()
    {
        Instance = this;
        SetInputDict();
        LoadBindings();
    }

    void Start()
    {
        actionsAsset.FindActionMap("Player").Enable();
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

    public bool PressMenu()
    {
        return inputDict[InputType.Menu].WasPressedThisFrame();
    }

    public void StartBinding()
    {
        actionsAsset.FindActionMap("Player").Disable();
    }

    public void EndBinding()
    {
        actionsAsset.FindActionMap("Player").Enable();
    }

    public void SaveBindings()
    {
        var rebinds = actionsAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(BindingsKey, rebinds);
        PlayerPrefs.Save();
    }

    public InputAction GetInputAction(InputType type)
    {
        return inputDict[type];
    }

    private void SetInputDict()
    {
        inputDict = Enum.GetValues(typeof(InputType)).Cast<InputType>().ToDictionary(e => e, e => actionsAsset.FindActionMap("Player").FindAction(e.ToString()));
        foreach (var input in inputDict)
        {
            input.Value.Enable();
        }
    }

    private void LoadBindings()
    {
        if (PlayerPrefs.HasKey(BindingsKey))
        {
            var rebinds = PlayerPrefs.GetString(BindingsKey);
            actionsAsset.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
