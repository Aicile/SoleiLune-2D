using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Script_MiniGame_SkillCheck : MonoBehaviour
{
    [Header("Skill Check Settings")]
    public RectTransform skillCheckArea; // Defines the area where the skill check takes place. Adjusting its size or position affects the layout.
    public RectTransform successZone; // The zone the indicator must be in for a successful skill check. Adjusting its size makes success easier or harder.
    public RectTransform movingIndicator; // Represents the moving part of the skill check. Its position updates based on the shape and movement logic.

    [Range(0.1f, 100f)] public float indicatorSpeed = 1f; // Controls the speed of the indicator. Higher values make the skill check more challenging.
    public bool isClockwise = true; // Determines the direction of the indicator's movement. Set to false for counterclockwise motion.

    [Header("Customizable Shape Settings")]
    public ShapeType skillCheckShape = ShapeType.Circle; // Defines the shape of the skill check area. Options include Circle, Oval, Rectangle, and Custom.
    public enum ShapeType { Circle, Oval, Rectangle, Custom } // Enum to represent available shapes.

    [Header("Callbacks")]
    public UnityEvent onSuccess; // Called when the skill check is successful. Can be assigned in the Inspector.
    public UnityEvent onFail; // Called when the skill check fails. Can be assigned in the Inspector.

    private bool isActive = false; // Tracks whether the skill check is currently active.
    private float currentAngle = 0f; // Current angle of the indicator, primarily for circular and oval shapes.

    private void Update()
    {
        //if (!isActive) return; // Prevent updates when the skill check is inactive.

        MoveIndicator(); // Update the position of the moving indicator.

        if (Input.GetKeyDown(KeyCode.Space)) // Example input for triggering the skill check.
        {
            CheckSkill(); // Evaluate if the skill check was successful or failed.
        }
    }

    public void StartSkillCheck()
    {
        isActive = true; // Activates the skill check.
        currentAngle = 0f; // Resets the indicator's angle.
    }

    public void StopSkillCheck()
    {
        isActive = false; // Deactivates the skill check.
    }

    private void MoveIndicator()
    {
        float movement = indicatorSpeed * Time.deltaTime * (isClockwise ? 1 : -1); // Calculate movement based on speed and direction.

        switch (skillCheckShape)
        {
            case ShapeType.Circle:
                currentAngle = (currentAngle + movement) % 360; // Update angle for circular motion.
                PositionIndicatorInCircle(); // Position the indicator within a circular area.
                break;
            case ShapeType.Oval:
                PositionIndicatorInOval(movement); // Position the indicator within an oval area.
                break;
            case ShapeType.Rectangle:
                PositionIndicatorInRectangle(movement); // Position the indicator within a rectangular area (logic to be implemented).
                break;
            case ShapeType.Custom:
                // Extend here for custom shapes as needed.
                break;
        }
    }

    private void PositionIndicatorInCircle()
    {
        float radians = currentAngle * Mathf.Deg2Rad; // Convert angle to radians.
        float x = Mathf.Cos(radians) * (skillCheckArea.rect.width / 2); // Calculate x position based on the circle's radius.
        float y = Mathf.Sin(radians) * (skillCheckArea.rect.height / 2); // Calculate y position based on the circle's radius.
        movingIndicator.localPosition = new Vector2(x, y); // Update the indicator's position.
    }

    private void PositionIndicatorInOval(float movement)
    {
        currentAngle = (currentAngle + movement) % 360; // Update angle for oval motion.
        float radians = currentAngle * Mathf.Deg2Rad; // Convert angle to radians.
        float x = Mathf.Cos(radians) * (skillCheckArea.rect.width / 2); // Calculate x position for the oval.
        float y = Mathf.Sin(radians) * (skillCheckArea.rect.height / 4); // Calculate y position for the oval (adjusted height for oval effect).
        movingIndicator.localPosition = new Vector2(x, y); // Update the indicator's position.
    }

    private void PositionIndicatorInRectangle(float movement)
    {
        // Add logic for rectangular movement here.
        // This could involve moving along the edges of the rectangle.
    }

    private void CheckSkill()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(successZone, movingIndicator.position, null))
        {
            Debug.Log("Im working really hard master i promise");
            onSuccess?.Invoke(); // Trigger success callback if the indicator is within the success zone.
        }
        else
        {
            onFail?.Invoke(); // Trigger failure callback if the indicator is outside the success zone.
        }
    }
}
