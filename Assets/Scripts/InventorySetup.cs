using UnityEngine;
using UnityEngine.UI;

public class InventorySetup : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform inventoryBar;

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            Instantiate(slotPrefab, inventoryBar);
        }
    }
}
