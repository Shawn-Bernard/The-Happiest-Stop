using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Inputs",menuName = "Input Reader/Player Inputs")]
public class PlayerInputs : ScriptableObject,PlayerInput_Actions.IPlayerActions
{
    public PlayerInput_Actions inputActions;


    #region Move Input
    public UnityAction<Vector2> onMove;
    public UnityAction onMoveStarted;
    public UnityAction onMoveCanceled;
    public void OnMove(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>());

        if (context.started)
        {
            onMoveStarted?.Invoke();
        }
        if (context.canceled)
        {
            onMoveCanceled?.Invoke();
        }
    }
    public void EnableMove()
    {
        inputActions.Player.Move.Enable();
    }

    public void DisableMove()
    {
        inputActions.Player.Move.Disable();
    }
    #endregion

    #region Look Input
    public UnityAction<Vector2> onLook;
    public void OnLook(InputAction.CallbackContext context)
    {
        onLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void EnableLook()
    {
        inputActions.Player.Look.Enable();
    }

    public void DisableLook()
    {
        inputActions.Player.Look.Disable();
    }
    #endregion

    #region Interact Input
    public UnityAction onInteractStarted;
    public UnityAction onInteractCanceled;
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            onInteractStarted?.Invoke();
        } 
        if (context.canceled)
        {
            onInteractCanceled?.Invoke();
        }
    }
    public void EnableInteract()
    {
        inputActions.Player.Interact.Enable();
    }

    public void DisableInteract()
    {
        inputActions.Player.Interact.Disable();
    }
    #endregion

    #region Pick up Input
    public UnityAction onPickUpStarted;
    public void OnPickup(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onPickUpStarted?.Invoke();
        }
    }
    public void EnablePickUp()
    {
        inputActions.Player.Pickup.Enable();
        inputActions.Player.Pickup.Disable();
    }

    public void DisablePickUp()
    {
        inputActions.Player.Pickup.Disable();
    }
    #endregion

    #region Sprint Input
    public UnityAction onSprintStarted;
    public UnityAction onSprintCanceled;
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onSprintStarted?.Invoke();
        }
        if (context.canceled)
        {
            onSprintCanceled?.Invoke();
        }
    }
    public void EnableSprint()
    {
        inputActions.Player.Sprint.Enable();
    }

    public void DisableSprint()
    {
        inputActions.Player.Sprint.Disable();
    }
    #endregion

    #region Jump Input
    public UnityAction onJumpStarted;
    public UnityAction onJumpCanceled;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onJumpStarted?.Invoke();
        }
        if (context.canceled)
        {
            onJumpCanceled?.Invoke();
        }
    }
    public void EnableJump()
    {
        inputActions.Player.Jump.Enable();
    }

    public void DisableJump()
    {
        inputActions.Player.Jump.Disable();
    }
    #endregion
    #region Pause Input
    public UnityAction onPauseStarted;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onPauseStarted?.Invoke();
        }
    }
    public void EnablePause()
    {
        inputActions.Player.Pause.Enable();
    }

    public void DisablePause()
    {
        inputActions.Player.Pause.Disable();
    }
    #endregion
    #region Task List Input
    public UnityAction onTaskStarted;
    public void OnTaskList(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onTaskStarted?.Invoke();
        }
    }
    public void EnableTaskList()
    {
        inputActions.Player.TaskList.Enable();
    }

    public void DisableTaskList()
    {
        inputActions.Player.TaskList.Disable();
    }
    #endregion
    public void EnableAllInputs()
    {
        inputActions.Player.Enable();
    }
    public void DisableAllInputs()
    {
        inputActions.Player.Disable();
    }

    private void OnEnable()
    {
        inputActions = new PlayerInput_Actions();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();
    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    
}
