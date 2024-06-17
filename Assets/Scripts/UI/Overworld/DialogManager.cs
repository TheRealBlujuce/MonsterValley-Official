using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] float lettersPerSecond;
    public event Action onShowDialog;
    public event Action onCloseDialog;
    public static DialogManager dialogInstance {get; private set;}

    private int currentLine = 0;
    private Dialog dialog;
    private bool isTyping;
    public bool isDisplayed;
    private string speaker;
    private Action onDialogFinished;

    private void Awake() 
    {
        dialogInstance = this;
    }

    public IEnumerator ShowDialog(string name, Dialog newDialog, Action onFinished=null)
    {
        yield return new WaitForEndOfFrame();
        speaker = name;
        onShowDialog.Invoke();
        isDisplayed = true;
        onDialogFinished = onFinished;
        dialog = newDialog;
        dialogBox.SetActive(true);
        if (speaker != ""){ StartCoroutine(TypeDialog(speaker + ": " + dialog.GetCharacterDialog()[0])); }
        else
        { StartCoroutine(TypeDialog(dialog.GetCharacterDialog()[0])); }
        
    }
    
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    public void HandleDialogUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            currentLine++;
            if (currentLine < dialog.GetCharacterDialog().Count)
            {
                if (speaker != ""){ StartCoroutine(TypeDialog(speaker + ": " + dialog.GetCharacterDialog()[currentLine])); }
                else
                { StartCoroutine(TypeDialog(dialog.GetCharacterDialog()[currentLine])); }
            }
            else
            {   
                dialogBox.SetActive(false);
                currentLine = 0;
                onCloseDialog?.Invoke();
                isDisplayed = false;
                onDialogFinished?.Invoke();
            }
        }
    }

}
