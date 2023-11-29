using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputSystemScript : MonoBehaviour
{
    public event EventHandler OnInteractionAction;
    public event EventHandler OnUIEquipmentOn;
    public event EventHandler OnUIJournalOn;

    private static InputSystemScript _instance;
    public static InputSystemScript Instance
    {
        get
        {
            return _instance;
        }
    }

    public InputSystemManager inputSystemManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        inputSystemManager = new InputSystemManager();
        inputSystemManager.Enable();
    }
    private void Update()
    {
        inputSystemManager.Player.Interact.performed += Interact_performed;
        inputSystemManager.Player.EQ_ON.performed += EQ_ON_performed;
        inputSystemManager.Player.JournalON.performed += JournalON_performed;
    }

    private void JournalON_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUIJournalOn?.Invoke(this, EventArgs.Empty);
    }

    private void EQ_ON_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnUIEquipmentOn?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractionAction?.Invoke(this, EventArgs.Empty);
    }

    // get set

    public Vector2 GetNormalizedMovementVector()
    {
        return inputSystemManager.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseVector()
    {
        return inputSystemManager.Player.Look.ReadValue<Vector2>();
    }

    public bool WasShiftRealeased()
    {
        return inputSystemManager.Player.Sprint.WasReleasedThisFrame();
    }

    public bool WasShiftPressed()
    {
        return inputSystemManager.Player.Sprint.WasPressedThisFrame();
    }
}
