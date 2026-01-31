using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    WeaponBasic,
    WeaponSpecial,
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
    private const string PlayerMap = "Player";

    private void Awake()
    {
        Instance = this;
        SetInputDict();
        LoadBindings();
    }

    void Start()
    {
        actionsAsset.FindActionMap(PlayerMap).Enable();
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
        actionsAsset.FindActionMap(PlayerMap).Disable();
    }

    public void EndBinding()
    {
        actionsAsset.FindActionMap(PlayerMap).Enable();
    }

    public void SaveBindings()
    {
        var rebinds = actionsAsset.FindActionMap(PlayerMap).SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(BindingsKey, rebinds);
        PlayerPrefs.Save();
    }

    public InputAction GetInputAction(InputType type)
    {
        return inputDict[type];
    }

    private void SetInputDict()
    {
        inputDict = Enum.GetValues(typeof(InputType)).Cast<InputType>().ToDictionary(e => e, e => actionsAsset.FindActionMap(PlayerMap).FindAction(e.ToString()));
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
            actionsAsset.FindActionMap(PlayerMap).LoadBindingOverridesFromJson(rebinds);
        }
    }
}
