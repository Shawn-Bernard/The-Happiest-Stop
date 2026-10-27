using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Inputs",menuName = "Input Reader/Player Inputs")]
public class PlayerInputs : ScriptableObject,PlayerInput_Actions.IPlayerActions
{
    public PlayerInput_Actions inputActions;


    #region Move Input
    private bool canMove;
    public UnityAction<Vector2> onMove;
    public UnityAction onMoveStarted;
    public UnityAction onMoveCanceled;
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove) return;

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
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
    }
    #endregion

    #region Look Input
    private bool canLook;
    public UnityAction<Vector2> onLook;
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
    private bool canInteract;
    public UnityAction onInteractStarted;
    public UnityAction onInteractCanceled;
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
    private bool canPickUp;
    public UnityAction onPickUpStarted;
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
    private bool canSprint;
    public UnityAction onSprintStarted;
    public UnityAction onSprintCanceled;
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
    private bool canJump;
    public UnityAction onJumpStarted;
    public UnityAction onJumpCanceled;
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

    public void EnableAllInputs()
    {
        EnableMove();
        EnableLook();
        EnableInteract();
        EnablePickUp();
        EnableSprint();
        EnableJump();
    }
    public void DisableAllInputs()
    {
        DisableMove();
        DisableLook();
        DisableInteract();
        DisablePickUp();
        DisableSprint();
        DisableJump();
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
