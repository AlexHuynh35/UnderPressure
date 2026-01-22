using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InputSettingManager : MonoBehaviour
{
    public InputType type;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI keyText;
    public Button changeKeyButton;
    public Button resetKeyButton;

    private InputAction action;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    void Start()
    {
        action = InputManager.Instance.GetInputAction(type);
        inputText.SetText(type.ToString());
        keyText.SetText(action.GetBindingDisplayString(0));
        changeKeyButton.onClick.AddListener(StartRebinding);
        resetKeyButton.onClick.AddListener(ResetKey);
    }

    void Update()
    {
        
    }

    private void ResetKey()
    {
        action.RemoveAllBindingOverrides();
        InputManager.Instance.SaveBindings();
        keyText.SetText(action.GetBindingDisplayString(0));
    }

    private void StartRebinding()
    {
        InputManager.Instance.StartBinding();

        changeKeyButton.interactable = false;
        keyText.SetText("...");

        rebindOperation = action.PerformInteractiveRebinding().OnComplete(operation => RebindComplete()).OnCancel(operation => RebindCancel()).WithCancelingThrough("<Keyboard>/escape").Start();
    }

    private void RebindComplete()
    {
        InputManager.Instance.SaveBindings();
        rebindOperation.Dispose();

        keyText.SetText(action.GetBindingDisplayString(0));
        changeKeyButton.interactable = true;
        InputManager.Instance.EndBinding();
    }

    private void RebindCancel()
    {
        rebindOperation.Dispose();

        keyText.SetText(action.GetBindingDisplayString(0));
        changeKeyButton.interactable = true;
        InputManager.Instance.EndBinding();
    }
}
