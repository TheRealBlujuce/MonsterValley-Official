using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PartyScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private PartyMemberUI[] memberList;
    [SerializeField] private Color highlightColor;
    private List<Pemo> pemos;
    public void InitPartyScreen()
    {
        memberList = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Pemo> pemoParty)
    {
        pemos = pemoParty;
        for (int i = 0; i < memberList.Length; i++)
        {
            if (i < pemos.Count) { memberList[i].SetData(pemos[i]); }
            else { memberList[i].gameObject.SetActive(false); }
            
        }
    }

    public void UpdatePartySelection(int selectedMember)
    {
        for (int i = 0; i<pemos.Count; i++)
        {
            if (i == selectedMember) { memberList[i].GetComponent<Image>().color = highlightColor; } else { memberList[i].GetComponent<Image>().color = Color.white; }
        }
    }

    public void SetMessageText(string newMessage)
    {
        messageText.text = newMessage;
    }
}
