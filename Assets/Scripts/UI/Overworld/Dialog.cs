using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> dialogList;

    public List<string> GetCharacterDialog()
    {
        return dialogList;
    }
}
