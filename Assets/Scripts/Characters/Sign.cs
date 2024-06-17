using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    public void Interact(Transform initiator)
    {
        // Interact with the npc
       StartCoroutine( DialogManager.dialogInstance.ShowDialog("",dialog));
    }

}
