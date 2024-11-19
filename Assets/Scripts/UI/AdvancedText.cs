using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/*
public class AdvancedTextPreprocessor : ITextPreprocessor
{
    public Dictionary<int, float> IntervalDictionary;
    public AdvancedTextPreprocessor()
    {
        IntervalDictionary = new Dictionary<int, float>();
    }
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();

        string processingText = text;
        string pattern = "<.*?>";
        Match match = Regex.Match(processingText, pattern);
        while (match.Success)
        {
            string label = match.Value.Substring(1, match.Length - 2);
            if(float.TryParse(label,out float result))
            {
                IntervalDictionary[match.Index - 1] = result;
            }

            processingText.Remove(match.Index, match.Length);

            match = Regex.Match(processingText, pattern);
        }

        processingText = text;
        pattern = @"<(\d+)(\.\d+)?>";
        processingText = Regex.Replace(processingText, pattern, "");
        return processingText;
    }
}
public class AdvancedText : TextMeshProUGUI
{
    protected override void Start()
    {
        textPreprocessor = new AdvancedTextPreprocessor();
    }

    private AdvancedTextPreprocessor SelfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor;
    public void ShowTextByTyping(string content)
    {
        SetText(content);
        StartCoroutine(Typing());
    }

    private int _typingIndex;
    private float _defaultInterval = 0.06f;
    IEnumerator Typing()
    {
        ForceMeshUpdate();
        for(int i = 0;i<m_characterCount;i++)
        {
            SetSingleCharacterAlpha(i, 0);
        }
        _typingIndex = 0;
        while(_typingIndex < m_characterCount)
        {
            if (textInfo.characterInfo[_typingIndex].isVisible)
            {
                StartCoroutine(FadeInChatacter(_typingIndex));
            }
            
            if (SelfPreprocessor.IntervalDictionary.TryGetValue(_typingIndex, out float result))
            {
                yield return new WaitForSecondsRealtime(result);
            }
            else yield return new WaitForSecondsRealtime(_defaultInterval);
        }
        _typingIndex++;
    }

    private void SetSingleCharacterAlpha(int index,byte newAlpha)
    {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
        int matIndex = charInfo.materialReferenceIndex;
        int vertIndex = charInfo.vertexIndex;
        for (int i = 0;i<4;i++)
        {
            textInfo.meshInfo[matIndex].colors32[vertIndex + i].a = newAlpha;
        }
        UpdateVertexData();
    }

    IEnumerator FadeInChatacter(int index,float duration = 0.2f)
    {
        if(duration <=0)
        {
            SetSingleCharacterAlpha(index, 255);
        }
        else
        {
            float timer = 0;
            while(timer < duration)
            {
                timer = Mathf.Min(duration,timer + Time.unscaledDeltaTime);
                SetSingleCharacterAlpha(index,(byte)(255*timer/duration));
                yield return null;
            }
        }
    }
}
*/
