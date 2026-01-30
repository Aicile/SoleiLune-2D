using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Script_MiniGame_SkillCheck : MonoBehaviour
{
    #region Inspector Variables and other variables
    [Header("Skill Check Settings")]
    public RectTransform skillCheckArea; // Defines the area where the skill check takes place. Adjusting its size or position affects the layout.
    public RectTransform successZone; // The zone the indicator must be in for a successful skill check. Adjusting its size makes success easier or harder.
    public RectTransform movingIndicator; // Represents the moving part of the skill check. Its position updates based on the shape and movement logic.

    [Range(0.1f, 100f)] public float indicatorSpeed = 1f; // Controls the speed of the indicator. Higher values make the skill check more challenging.
    public bool isClockwise = true; // Determines the direction of the indicator's movement. Set to false for counterclockwise motion.
    public bool faceCenter = true; // When enabled the Indcator will always face the center of the skill check.

    [Header("Customizable Shape Settings")]
    public ShapeType skillCheckShape = ShapeType.Circle; // Defines the shape of the skill check area. Options include Circle, Oval, Rectangle, and Custom.
    public enum ShapeType { Circle, Oval, Rectangle, Custom } // Enum to represent available shapes.

    [Header("Callbacks")]
    public UnityEvent onSuccess; // Called when the skill check is successful. Can be assigned in the Inspector.
    public UnityEvent onFail; // Called when the skill check fails. Can be assigned in the Inspector.

    private bool isActive = false; // Tracks whether the skill check is currently active.
    private float currentAngle = 0f; // Current angle of the indicator, primarily for circular and oval shapes.
    #endregion
    private void Start()
    {
        RandomizeSuccessZone(); // Ensures the success zone is randomized on play(Workaround not sure if this will be the way to do it yet).
    }



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
        RandomizeSuccessZone(); // Place success zone at a new perimeter position.
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
        if (faceCenter) // Its an If statement which only activates if the faceCenter is set to true in Inspector.
        {
            FaceCenter(); // Ensure the indicator always faces the center.
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

        float width = skillCheckArea.rect.width / 2;
        float height = skillCheckArea.rect.height / 2;

        // Ensure the square logic uses the smaller dimension.
        float squareSize = Mathf.Min(width, height); // This ensures the square uses the smallest dimension.

        // Move the indicator along the rectangle's edges.
        // Assume movement is incremental along the edges of the rectangle (in a clockwise or counterclockwise direction)
        Vector2 newPosition = Vector2.zero;

        if (currentAngle < 90)
        {
            newPosition = new Vector2(currentAngle * squareSize / 90, squareSize); // Top edge
        }
        else if (currentAngle < 180)
        {
            newPosition = new Vector2(squareSize, squareSize - (currentAngle - 90) * squareSize / 90); // Right edge
        }
        else if (currentAngle < 270)
        {
            newPosition = new Vector2(squareSize - (currentAngle - 180) * squareSize / 90, -squareSize); // Bottom edge
        }
        else
        {
            newPosition = new Vector2(-squareSize, -squareSize + (currentAngle - 270) * squareSize / 90); // Left edge
        }

        movingIndicator.localPosition = newPosition; // Update the indicator's position.
        currentAngle = (currentAngle + movement) % 360; // Increment the angle and loop.
    }




    private void RandomizeSuccessZone()
    {
        Vector2 newPosition = Vector2.zero;

        switch (skillCheckShape)
        {
            case ShapeType.Circle:
                newPosition = GetRandomPointOnCircle();
                break;
            case ShapeType.Oval:
                newPosition = GetRandomPointOnOval();
                break;
            case ShapeType.Rectangle:
                newPosition = GetRandomPointOnRectangleEdge();
                break;
            case ShapeType.Custom:
                break;
        }

        successZone.localPosition = newPosition;
        Debug.Log("Success Zone moved to: " + newPosition); // Debugging output

        // Make successZone face the center
        FaceSuccessZoneToCenter();
    }

    private Vector2 GetRandomPointOnCircle()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * (skillCheckArea.rect.width / 2);
        float y = Mathf.Sin(angle) * (skillCheckArea.rect.height / 2);
        return new Vector2(x, y);
    }

    private Vector2 GetRandomPointOnOval()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * (skillCheckArea.rect.width / 2);
        float y = Mathf.Sin(angle) * (skillCheckArea.rect.height / 4);
        return new Vector2(x, y);
    }

    private Vector2 GetRandomPointOnRectangleEdge()
    {
        int edge = Random.Range(0, 4); // 0 = top, 1 = right, 2 = bottom, 3 = left
        float width = skillCheckArea.rect.width / 2;
        float height = skillCheckArea.rect.height / 2;

        switch (edge)
        {
            case 0: return new Vector2(Random.Range(-width, width), height); // Top edge
            case 1: return new Vector2(width, Random.Range(-height, height)); // Right edge
            case 2: return new Vector2(Random.Range(-width, width), -height); // Bottom edge
            case 3: return new Vector2(-width, Random.Range(-height, height)); // Left edge
            default: return Vector2.zero;
        }
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
            Debug.LogWarning("Back to the hook you go");
            onFail?.Invoke(); // Trigger failure callback if the indicator is outside the success zone.
        }
    }


    private void FaceCenter()
    {
        Vector2 centerPosition = skillCheckArea.position; // Get the center position of the skill check
        Vector2 indicatorPosition = movingIndicator.position; // Get the current position of the Indicator

        Vector2 direction = centerPosition - indicatorPosition; // Calculate the direction to the center
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert to an angle in degrees

        movingIndicator.rotation = Quaternion.Euler(0, 0, angle + 90); // Apply rotation (adjusted for correct orientation)
    }

    private void FaceSuccessZoneToCenter()
    {
        Vector2 directionToCenter = -successZone.localPosition.normalized; // Get direction toward the center
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg; // Convert to angle
        successZone.localEulerAngles = new Vector3(0, 0, angle); // Apply rotation in 2D UI space
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
