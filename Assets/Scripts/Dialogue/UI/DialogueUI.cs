using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : SingleMonoBase<DialogueUI>
{
    [Header("Basic Elements")]
    public Text talkerName;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")]
    public DialogueData_SO currentData;
    int currentIndex = 0;

    private protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
        {
            UpdataMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else
            dialoguePanel.SetActive(false);
    }

    public void UpdataDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }

    public void UpdataMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;
        if (currentData.talkerName != null)
        {
            talkerName.enabled = true;
            talkerName.text = currentData.talkerName.ToString();
        }
        else talkerName.enabled = false;

        mainText.text = "";
        //mainText.text = piece.text;
        mainText.DOText(piece.text, 1f);

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.gameObject.SetActive(true);
            
        }
        else
            nextButton.gameObject.SetActive(false);

        //´´½¨options
        CreatOptions(piece);
    }

    void CreatOptions(DialoguePiece piece)
    {
        if(optionPanel.childCount > 0)
        {
            for(int i = 0; i<optionPanel.childCount;i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }

            for(int i =0;i<piece.options.Count;i++)
            {
                var option = Instantiate(optionPrefab, optionPanel);
                option.UpdateOption(piece, piece.options[i]);
            }
        }
    }

}
