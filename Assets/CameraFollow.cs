using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign your player's transform here in the inspector
    public Vector3 offset; // Adjust this in the inspector to set the camera's position relative to the player

    // Optional settings for smoothing the camera movement
    public bool smoothFollow = true; // Set to true if you want the camera to smoothly follow the player
    public float smoothSpeed = 0.125f; // Adjust the smoothing speed

    private void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        if (smoothFollow)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = desiredPosition;
        }

        // Keep the camera's rotation fixed
        transform.LookAt(player);
    }
}

