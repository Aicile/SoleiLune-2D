using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int ingredientID; 
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public Transform originalParent;

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
        rectTransform.anchoredPosition += eventData.delta / FindObjectOfType<Canvas>().scaleFactor; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent); 
    }
}
