using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public TextMeshProUGUI countText;
    public int itemCount = 0;

    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    void Awake()
    {
        originalParent = transform;
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.localPosition;
    }

    public void AddItem(Sprite newIcon, int count)
    {
        icon.sprite = newIcon;
        icon.enabled = true;
        itemCount += count;
        UpdateCountText();
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        itemCount = 0;
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        countText.text = itemCount > 0 ? itemCount.ToString() : "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggablePotion draggablePotion = eventData.pointerDrag.GetComponent<DraggablePotion>();
        if (draggablePotion != null)
        {
            AddItem(draggablePotion.icon.sprite, 1);
            Destroy(draggablePotion.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemCount > 0)
        {
            canvasGroup.blocksRaycasts = false;
            transform.SetParent(transform.root);
            originalPosition = transform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (icon.sprite != null)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (icon.sprite != null)
        {
            RaycastResult raycastResult = eventData.pointerCurrentRaycast;
            if (raycastResult.gameObject != null && raycastResult.gameObject.CompareTag("Customer"))
            {
                Customer customer = raycastResult.gameObject.GetComponent<Customer>();
                if (customer != null && customer.potionNeeded == icon.sprite.name)
                {
                    customer.CompleteTransaction(0); // No additional points for direct drag
                    itemCount--;
                    UpdateCountText();
                    if (itemCount <= 0)
                    {
                        ClearSlot();
                    }
                    return;
                }
            }
            transform.SetParent(originalParent);
            transform.localPosition = originalPosition;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
