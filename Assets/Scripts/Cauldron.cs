using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour, IDropHandler
{
    public List<int> ingredientIDs = new List<int>();
    public GameObject healthPotion;
    public GameObject manaPotion;
    public GameObject energyPotion;

    public TimingMinigameOne timingMinigameOne;
    public TimingMinigameTwo timingMinigameTwo;
    public StirringMinigame stirringMinigame;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableIngredient draggableIngredient = eventData.pointerDrag.GetComponent<DraggableIngredient>();
        if (draggableIngredient != null)
        {
            Debug.Log("Ingredient with ID " + draggableIngredient.ingredientID + " was dropped into the cauldron.");
            ingredientIDs.Add(draggableIngredient.ingredientID);

            if (ingredientIDs.Count == 2)
            {
                CraftPotion();
            }
        }
    }

    private void CraftPotion()
    {
        ingredientIDs.Sort();

        if (CheckIngredients())
        {
            StartMinigamesBasedOnIngredients();
        }
        else
        {
            Debug.Log("Not enough ingredients to craft the potion.");
            ingredientIDs.Clear();
        }
    }

    private bool CheckIngredients()
    {
        // Check for the required ingredients in the JournalManager
        foreach (int id in ingredientIDs)
        {
            string ingredientName = GetIngredientName(id);
            if (!JournalManager.instance.CheckIngredientStock(ingredientName))
            {
                return false;
            }
        }
        return true;
    }

    private string GetIngredientName(int id)
    {
        switch (id)
        {
            case 1: return "Coffee Bean";
            case 2: return "Rose";
            case 3: return "Lilac";
            case 4: return "Lavender";
            case 5: return "Crimson Lycoris";
            case 6: return "Red Bean";
            default: return "";
        }
    }

    private void StartMinigamesBasedOnIngredients()
    {
        ClearMinigames();
        if (ingredientIDs.Contains(1) && ingredientIDs.Contains(2))
        {
            timingMinigameOne.StartMinigame(() => StartStirringMinigame(), FailCallback);
        }
        else if (ingredientIDs.Contains(3) && ingredientIDs.Contains(4))
        {
            timingMinigameTwo.StartMinigame(() => StartStirringMinigame(), FailCallback);
        }
        else if (ingredientIDs.Contains(5) && ingredientIDs.Contains(6))
        {
            timingMinigameOne.StartMinigame(() => timingMinigameTwo.StartMinigame(() => StartStirringMinigame(), FailCallback), FailCallback);
        }
        else
        {
            Debug.Log("Incorrect ingredients. No potion crafted.");
            ingredientIDs.Clear();
        }
    }

    private void StartStirringMinigame()
    {
        stirringMinigame.StartMinigame(FinalSuccess, FailCallback);
    }

    private void ClearMinigames()
    {
        timingMinigameOne.gameObject.SetActive(false);
        timingMinigameTwo.gameObject.SetActive(false);
        stirringMinigame.gameObject.SetActive(false);
    }

    private void FinalSuccess()
    {
        ActivatePotionBasedOnIngredients();
        IngredientManager.Instance.ResetIngredients(); // Reset ingredients after crafting success
        ingredientIDs.Clear();
    }

    private void ActivatePotionBasedOnIngredients()
    {
        Debug.Log($"FinalSuccess called with ingredients: {string.Join(", ", ingredientIDs)}");

        if (ingredientIDs.Contains(1) && ingredientIDs.Contains(2))
        {
            if (healthPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Health", 1);
                healthPotion.SetActive(true);
                UseIngredients();
            }
            else
            {
                Debug.LogError("Health potion GameObject is not assigned in the inspector.");
            }
        }

        if (ingredientIDs.Contains(3) && ingredientIDs.Contains(4))
        {
            if (manaPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Mana", 1);
                manaPotion.SetActive(true);
                UseIngredients();
            }
            else
            {
                Debug.LogError("Mana potion GameObject is not assigned in the inspector.");
            }
        }

        if (ingredientIDs.Contains(5) && ingredientIDs.Contains(6))
        {
            if (energyPotion != null)
            {
                JournalManager.instance.UpdatePotionStock("Energy", 1);
                energyPotion.SetActive(true);
                UseIngredients();
            }
            else
            {
                Debug.LogError("Energy potion GameObject is not assigned in the inspector.");
            }
        }

        if (!ingredientIDs.Contains(1) && !ingredientIDs.Contains(2) &&
            !ingredientIDs.Contains(3) && !ingredientIDs.Contains(4) &&
            !ingredientIDs.Contains(5) && !ingredientIDs.Contains(6))
        {
            Debug.Log("No matching potion for the ingredient combination.");
        }
    }

    private void UseIngredients()
    {
        foreach (int id in ingredientIDs)
        {
            string ingredientName = GetIngredientName(id);
            JournalManager.instance.UpdateIngredientStock(ingredientName, -1);
        }
    }

    private void FailCallback()
    {
        Debug.Log("Failed to craft potion. Try again.");
        IngredientManager.Instance.ResetIngredients(); // Reset ingredients after crafting failure
        ingredientIDs.Clear();
    }
}
