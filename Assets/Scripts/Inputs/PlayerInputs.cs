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
    private bool canMove;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        onMove?.Invoke(context.ReadValue<Vector2>());

        Debug.Log(context.ReadValue<Vector2>() + $" {canMove}");
        if (context.started)
        {
            Debug.Log("Move started");
            onMoveStarted?.Invoke();
        }

        if (context.canceled)
        {
            Debug.Log("Move canceled");
            onMoveCanceled?.Invoke();
        }
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
    }
    #endregion

    #region Look Input
    public UnityAction<Vector2> onLook;
    private bool canLook;

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!canLook) return;

        onLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void EnableLook()
    {
        canLook = true;
    }

    public void DisableLook()
    {
        canLook = false;
    }
    #endregion

    #region Interact Input
    public UnityAction onInteractStarted;
    public UnityAction onInteractCanceled;
    public UnityAction onInteractPerformed;
    private bool canInteract;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;

        if (context.started)
        {
            onInteractStarted?.Invoke();
        } 

        if (context.canceled)
        {
            onInteractCanceled?.Invoke();
        }

        if (context.performed)
        {
            onInteractPerformed?.Invoke();
        }
    }

    public void EnableInteract()
    {
        canInteract = true;
    }

    public void DisableInteract()
    {
        canInteract = false;
    }
    #endregion

    #region Pick up Input
    public UnityAction onPickUpStarted;
    private bool canPickUp;

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (!canPickUp) return;

        if (context.started)
        {
            onPickUpStarted?.Invoke();
        }
    }

    public void EnablePickUp()
    {
        canPickUp = true;
    }

    public void DisablePickUp()
    {
        canPickUp = false;
    }
    #endregion

    #region Sprint Input
    public UnityAction onSprintStarted;
    public UnityAction onSprintCanceled;
    private bool canSprint;

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!canSprint) return;

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
        canSprint = true;
    }

    public void DisableSprint()
    {
        canSprint = false;
    }
    #endregion

    #region Jump Input
    public UnityAction onJumpStarted;
    public UnityAction onJumpCanceled;
    private bool canJump;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!canJump) return;

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
        canJump = true;
    }

    public void DisableJump()
    {
        canJump = false;
    }
    #endregion

    #region Pause Input
    public UnityAction onPauseStarted;
    private bool canPause;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!canPause) return;

        if (context.started)
        {
            onPauseStarted?.Invoke();
        }
    }

    public void EnablePause()
    {
        canPause = true;
    }

    public void DisablePause()
    {
        canPause = false;
    }
    #endregion

    #region Task List Input
    public UnityAction onTaskStarted;
    private bool canTaskList;

    public void OnTaskList(InputAction.CallbackContext context)
    {
        if (!canTaskList) return;

        if (context.started)
        {
            onTaskStarted?.Invoke();
        }
    }
    public void EnableTaskList()
    {
        canTaskList = true;
    }

    public void DisableTaskList()
    {
        canTaskList = false;
    }
    #endregion

    #region Cigarettes Input

    public UnityAction onCigarettesStarted;
    private bool canCigarettes;

    public void OnCigarettes(InputAction.CallbackContext context)
    {
        if (!canCigarettes) return;

        if (context.started)
        {
            onCigarettesStarted?.Invoke();
        }
    }
    public void EnableCigarettes()
    {
        canCigarettes = true;
    }

    public void DisableCigarettes()
    {
        canCigarettes = false;
    }
    #endregion

    #region Use Input

    public UnityAction onUseStarted;
    private bool canUse;
    public void OnUse(InputAction.CallbackContext context)
    {
        if (!canUse) return;

        if (context.started)
        {
            onUseStarted?.Invoke();
        }
    }

    public void EnableUse()
    {
        canUse = true;
    }

    public void DisableUse()
    {
        canUse = false;
    }
    #endregion
    public void EnableAllInputs()
    {
        EnableMove();
        EnableLook();
        EnableInteract();
        EnablePickUp();
        EnableSprint();
        EnableJump();
        EnablePause();
        EnableTaskList();
        EnableCigarettes();
        EnableUse();
    }
    public void DisableAllInputs()
    {
        DisableMove();
        DisableLook();
        DisableInteract();
        DisablePickUp();
        DisableSprint();
        DisableJump();
        DisablePause();
        DisableTaskList();
        DisableCigarettes();
        DisableUse();
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
