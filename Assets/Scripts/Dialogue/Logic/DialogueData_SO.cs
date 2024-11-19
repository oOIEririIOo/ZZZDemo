using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New  Dailogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public string talkerName;
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach(var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID,piece);
        }
    }
#else
    private void Awake()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }

#endif

    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest = null;
        foreach(var  piece in dialoguePieces)
        {
            if (piece.quest != null)
                currentQuest = piece.quest;
        }
        return currentQuest;
    }
}
