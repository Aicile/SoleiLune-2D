using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // Singleton instance

    public InventorySlot[] inventorySlots;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance already exists
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the inventory manager across scenes
        }
    }

    public bool AddPotionToInventory(string potionType, Sprite icon)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemCount == 0)
            {
                slot.AddItem(icon, 1);
                return true;
            }
        }
        return false;
    }

    public bool RemovePotion(string potionType, int count)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemCount > 0 && slot.icon.sprite.name == potionType)
            {
                slot.itemCount -= count;
                slot.UpdateCountText();
                if (slot.itemCount <= 0)
                {
                    slot.ClearSlot();
                }
                return true;
            }
        }
        return false;
    }

    public bool CheckPotionInInventory(string potionType)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemCount > 0 && slot.icon.sprite.name == potionType)
            {
                return true;
            }
        }
        return false;
    }

}
