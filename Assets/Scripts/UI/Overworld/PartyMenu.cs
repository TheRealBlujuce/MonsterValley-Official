using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PartyMenu : MonoBehaviour
{
    [SerializeField] private PartyMemberUI[] memberList;
    [SerializeField] private Color highlightColor;
    private List<Pemo> pemos;

    public void InitPartyMenuScreen()
    {
        memberList = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyMenuData(List<Pemo> pemoParty)
    {
        pemos = pemoParty;
        for (int i = 0; i < memberList.Length; i++)
        {
            if (i < pemos.Count) { memberList[i].SetData(pemos[i]); }
            else { memberList[i].gameObject.SetActive(false); }
            
        }
    }

    public void UpdatePartyMenuSelection(int selectedMember)
    {
        for (int i = 0; i<pemos.Count; i++)
        {
            if (i == selectedMember) { memberList[i].GetComponent<Image>().color = highlightColor; } else { memberList[i].GetComponent<Image>().color = Color.white; }
        }
    }

}
