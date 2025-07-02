using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputReader : MonoBehaviour, Control.IDialogueActions
{
    Control inputActions;
    Dialogues dialogues;

    public void OnEnable()
    {
        dialogues = GetComponent<Dialogues>();

        if (inputActions is not null) return;
        inputActions = new Control();
        inputActions.Dialogue.SetCallbacks(this);
        inputActions.Dialogue.Enable();
    }

    public void OnDisable()
    {
        inputActions.Dialogue.Disable();
    }

    public void OnNextPhrase(InputAction.CallbackContext context)
    {
        if (context.started && !dialogues.WaitChoice)
        {
            dialogues.ContinueStory();
        }
    }

    public void ChoiceButtonAction(int index)
    {
        if (dialogues.WaitChoice) dialogues.ChoiceButtonAction(index);
    }

    public void OnChange0(InputAction.CallbackContext context)
    {
        if (context.started) ChoiceButtonAction(0);
    }

    public void OnChange1(InputAction.CallbackContext context)
    {
        if (context.started) ChoiceButtonAction(1);
    }

    public void OnChange2(InputAction.CallbackContext context)
    {
        if (context.started) ChoiceButtonAction(2);
    }

    public void OnChange3(InputAction.CallbackContext context)
    {
        if (context.started) ChoiceButtonAction(3);
    }

    public void OnChange4(InputAction.CallbackContext context)
    {
        if (context.started) ChoiceButtonAction(4);
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        this.GetComponent<Dialogues>().MenuButtonAction();
    }

    public void OnInfo(InputAction.CallbackContext context)
    {
        dialogues.InfoButtonAction();
    }
}
