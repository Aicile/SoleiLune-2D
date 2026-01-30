using System.Collections;
using UnityEngine;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public static JournalManager instance; // Singleton instance

    public GameObject journalUI;
    public Canvas journalCanvas; // Reference to the Journal Canvas
    public TextMeshProUGUI potionJournalText;
    public TextMeshProUGUI ingredientJournalText;
    private bool isJournalOpen = false;
    public float fadeInDuration = 0.5f;

    public int healthPotionCount;
    public int manaPotionCount;
    public int energyPotionCount;

    // Ingredient counts
    public int coffeeBeanCount;
    public int roseCount;
    public int lilacCount;
    public int lavenderCount;
    public int crimsonLycorisCount;
    public int redBeanCount;

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

    void Start()
    {
        UpdateJournalText(); // Ensure journal text is updated on start
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
            // Update to show current potion and ingredient counts when the journal is opened
            UpdateJournalText();  // This will now update the text with the current potion and ingredient stock
            StopAllCoroutines();  // Stop any ongoing effects if needed
            lastFadeCoroutine = StartCoroutine(DisplayEntryWithEffects(potionJournalText.text, ingredientJournalText.text));
            SetCanvasSortingOrder(1000); // Set a high sorting order when the journal is opened
        }
        else
        {
            if (lastFadeCoroutine != null)
            {
                StopCoroutine(lastFadeCoroutine);
            }
            potionJournalText.text = "";
            ingredientJournalText.text = "";
            SetCanvasSortingOrder(0); // Reset sorting order when the journal is closed
        }
    }

    IEnumerator DisplayEntryWithEffects(string potionEntry, string ingredientEntry)
    {
        potionJournalText.text = potionEntry;
        ingredientJournalText.text = ingredientEntry;
        byte[] alphaValues = new byte[potionEntry.Length];
        byte[] alphaValuesIngredients = new byte[ingredientEntry.Length];

        for (int i = 0; i < potionEntry.Length; i++)
        {
            StartCoroutine(FadeInCharacter(i, alphaValues, potionJournalText));
            yield return new WaitForSeconds(fadeInDuration / potionEntry.Length);
        }
        for (int i = 0; i < ingredientEntry.Length; i++)
        {
            StartCoroutine(FadeInCharacter(i, alphaValuesIngredients, ingredientJournalText));
            yield return new WaitForSeconds(fadeInDuration / ingredientEntry.Length);
        }
    }

    IEnumerator FadeInCharacter(int index, byte[] alphaValues, TextMeshProUGUI journalText)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            alphaValues[index] = (byte)(alpha * 255);
            ApplyAlphaToText(alphaValues, journalText);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ApplyAlphaToText(byte[] alphaValues, TextMeshProUGUI journalText)
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

    // Method to update the ingredient stock in the journal
    public void UpdateIngredientStock(string ingredientType, int change)
    {
        switch (ingredientType)
        {
            case "Coffee Bean":
                coffeeBeanCount += change;
                break;
            case "Rose":
                roseCount += change;
                break;
            case "Lilac":
                lilacCount += change;
                break;
            case "Lavender":
                lavenderCount += change;
                break;
            case "Crimson Lycoris":
                crimsonLycorisCount += change;
                break;
            case "Red Bean":
                redBeanCount += change;
                break;
        }
        UpdateJournalText();
    }

    // Update the journal text to reflect the current stock
    private void UpdateJournalText()
    {
        potionJournalText.text = $"Potion Stock:\n" +
                           $"Health Potions: {healthPotionCount}\n" +
                           $"Mana Potions: {manaPotionCount}\n" +
                           $"Energy Potions: {energyPotionCount}";

        ingredientJournalText.text = $"Ingredient Stock:\n" +
                           $"Coffee Beans: {coffeeBeanCount}\n" +
                           $"Roses: {roseCount}\n" +
                           $"Lilacs: {lilacCount}\n" +
                           $"Lavender: {lavenderCount}\n" +
                           $"Crimson Lycoris: {crimsonLycorisCount}\n" +
                           $"Red Beans: {redBeanCount}";
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

    public bool CheckIngredientStock(string ingredientType)
    {
        switch (ingredientType)
        {
            case "Coffee Bean":
                return coffeeBeanCount > 0;
            case "Rose":
                return roseCount > 0;
            case "Lilac":
                return lilacCount > 0;
            case "Lavender":
                return lavenderCount > 0;
            case "Crimson Lycoris":
                return crimsonLycorisCount > 0;
            case "Red Bean":
                return redBeanCount > 0;
            default:
                return false;
        }
    }

    // Method to set the canvas sorting order
    private void SetCanvasSortingOrder(int order)
    {
        if (journalCanvas != null)
        {
            journalCanvas.sortingOrder = order;
        }
    }
}
