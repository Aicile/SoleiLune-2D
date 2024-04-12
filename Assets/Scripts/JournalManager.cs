using System.Collections;
using UnityEngine;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public GameObject journalUI; 
    public TextMeshProUGUI journalText; 
    private bool isJournalOpen = false; 
    public float fadeInDuration = 0.5f; 

    private Coroutine lastFadeCoroutine = null; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isJournalOpen = !isJournalOpen;
            journalUI.SetActive(isJournalOpen);

            if (isJournalOpen)
            {
                string entryText = "Your journal entry text here...";
                StopAllCoroutines(); 
                journalText.text = ""; 
                lastFadeCoroutine = StartCoroutine(DisplayEntryWithEffects(entryText));
            }
            else
            {
                if (lastFadeCoroutine != null)
                {
                    StopCoroutine(lastFadeCoroutine); 
                }
                journalText.text = ""; 
            }
        }
    }

    IEnumerator DisplayEntryWithEffects(string entry)
    {
        journalText.text = entry; 
        byte[] alphaValues = new byte[entry.Length]; 

        for (int i = 0; i < entry.Length; i++)
        {
            StartCoroutine(FadeInCharacter(i, alphaValues));
            yield return new WaitForSeconds(fadeInDuration / entry.Length); 
        }
    }

    IEnumerator FadeInCharacter(int index, byte[] alphaValues)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            alphaValues[index] = (byte)(alpha * 255);
            ApplyAlphaToText(alphaValues);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ApplyAlphaToText(byte[] alphaValues)
    {
        TMP_TextInfo textInfo = journalText.textInfo;
        Color32[] newVertexColors;
        int currentCharacter = 0;

        foreach (TMP_CharacterInfo charInfo in textInfo.characterInfo)
        {
            if (charInfo.isVisible)
            {
                newVertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                int vertexIndex = charInfo.vertexIndex;

                byte alpha = alphaValues[currentCharacter];
                for (int i = 0; i < 4; i++)
                {
                    newVertexColors[vertexIndex + i].a = alpha;
                }
            }
            currentCharacter++;
        }

        journalText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); 
    }
}
