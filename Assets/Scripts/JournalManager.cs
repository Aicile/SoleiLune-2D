using System.Collections;
using UnityEngine;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public static JournalManager instance; // Singleton instance

    public GameObject journalUI;
    public TextMeshProUGUI journalText;
    private bool isJournalOpen = false;
    public float fadeInDuration = 0.5f;

    public int healthPotionCount;
    public int manaPotionCount;
    public int energyPotionCount;

    private Coroutine lastFadeCoroutine = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the journal manager across scenes
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }

    private void ToggleJournal()
    {
        isJournalOpen = !isJournalOpen;
        journalUI.SetActive(isJournalOpen);

        if (isJournalOpen)
        {
            // Update to show current potion counts when the journal is opened
            UpdateJournalText();  // This will now update the text with the current potion stock
            StopAllCoroutines();  // Stop any ongoing effects if needed
            lastFadeCoroutine = StartCoroutine(DisplayEntryWithEffects(journalText.text));
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
    // Method to update the potion stock in the journal
    public void UpdatePotionStock(string potionType, int change)
    {
        switch (potionType)
        {
            case "Health":
                healthPotionCount += change;
                break;
            case "Mana":
                manaPotionCount += change;
                break;
            case "Energy":
                energyPotionCount += change;
                break;
        }
        UpdateJournalText();
    }

    // Update the journal text to reflect the current stock
    private void UpdateJournalText()
    {
        journalText.text = $"Health Potions: {healthPotionCount}\nMana Potions: {manaPotionCount}\nEnergy Potions: {energyPotionCount}";
    }

    public bool CheckPotionStock(string potionType)
    {
        switch (potionType)
        {
            case "Health":
                return healthPotionCount > 0;
            case "Mana":
                return manaPotionCount > 0;
            case "Energy":
                return energyPotionCount > 0;
            default:
                return false;
        }
    }

}



