using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int ingredientID;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 dragStartPosition;
    private Vector2 velocity;
    private bool isDragging = false;
    private Transform canvasTransform; // To store the canvas transform

    private float friction = 0.95f; // Friction coefficient

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("Missing CanvasGroup component on " + gameObject.name);
        }

        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("Missing RectTransform component on " + gameObject.name);
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasTransform = canvas.transform; // Ensure the Canvas is the parent
        }
        else
        {
            Debug.LogError("DraggableIngredient must be a child of a Canvas.");
        }

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        dragStartPosition = rectTransform.anchoredPosition;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
            velocity = (localPoint - dragStartPosition) / Time.deltaTime;
            dragStartPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        StartCoroutine(ApplyPhysics());
    }

    private IEnumerator ApplyPhysics()
    {
        while (!isDragging && velocity.magnitude > 0.01f)
        {
            rectTransform.anchoredPosition += velocity * Time.deltaTime;
            velocity *= friction;
            yield return null;
        }
    }

    private void Update()
    {
        if (!isDragging)
        {
            if (!IsWithinCanvas(rectTransform.anchoredPosition))
            {
                ResetPosition();
            }
        }
    }

    private bool IsWithinCanvas(Vector2 position)
    {
        var canvasRect = (canvasTransform as RectTransform).rect;
        return canvasRect.Contains(position);
    }

    private void ResetPosition()
    {
        rectTransform.anchoredPosition = startPosition;
        velocity = Vector2.zero;
    }
}
