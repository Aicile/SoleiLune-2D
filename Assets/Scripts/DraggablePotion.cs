using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePotion : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public string potionType; // The type of potion this represents

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        RaycastResult raycastResult = eventData.pointerCurrentRaycast;
        if (raycastResult.gameObject != null && raycastResult.gameObject.CompareTag("Customer"))
        {
            Customer customer = raycastResult.gameObject.GetComponent<Customer>();
            if (customer != null && customer.potionNeeded == potionType)
            {
                customer.CompleteTransaction(0); // No additional points for direct drag
                Destroy(gameObject); // Remove the potion from the inventory UI
            }
        }
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector3.zero;
    }
}
