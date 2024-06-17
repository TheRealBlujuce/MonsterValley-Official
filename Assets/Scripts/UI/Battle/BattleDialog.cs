using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialog : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<TextMeshProUGUI> actionOptions;
    [SerializeField] List<TextMeshProUGUI> moveOptions;

    [SerializeField] TextMeshProUGUI movePoints;
    [SerializeField] TextMeshProUGUI moveType;
    [SerializeField] TextMeshProUGUI moveDescription;

    [SerializeField] Color highlightColor;
    [SerializeField] Color defaultColor;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void EnableDialogText(bool active)
    {
        dialogText.enabled = active;
    }
    public void EnableActionSelector(bool active)
    {
        actionSelector.SetActive(active);
    }
    public void EnableMoveSelector(bool active)
    {
        moveSelector.SetActive(active);
        moveDetails.SetActive(active);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionOptions.Count; i++)
        {
            if (i== selectedAction) { actionOptions[i].color = highlightColor; actionOptions[i].fontSize = 6.75f; }
            else { actionOptions[i].color = defaultColor; actionOptions[i].fontSize = 6f; }
        }
    }    
    
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveOptions.Count; i++)
        {
            if (i == selectedMove && move.GetMP() > 0) { moveOptions[i].color = highlightColor; moveOptions[i].fontSize = 6.75f; }
            else if (i == selectedMove && move.GetMP() <= 0) {moveOptions[i].color = Color.red; moveOptions[i].fontSize = 6.75f;}
            else { moveOptions[i].color = defaultColor; moveOptions[i].fontSize = 6f; }

            movePoints.text = "MP: " + move.GetMP() + " / " + move.GetBaseMove().GetMovePoints();
            moveType.text = "Type: " + move.GetBaseMove().GetMoveType();
            moveDescription.text = move.GetBaseMove().GetMoveDescription();
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveOptions.Count; i++)
        {
            if (i < moves.Count)
            {
                moveOptions[i].text = moves[i].GetBaseMove().GetMoveName();
            }
            else
            {
                moveOptions[i].text = "";
            }
        }
    }

}
