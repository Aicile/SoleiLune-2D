using System.Collections.Generic;
using UnityEngine;

public class Script_EnvironmentalHazard : MonoBehaviour
{
    public enum HazardType { Wind, Mud, Ice, Quicksand }
    public HazardType hazardType;

    [Header("Wind Settings")]
    public Vector2 windDirection = Vector2.right;
    public float windStrength = 2f;

    [Header("Mud Settings")]
    [Range(0.1f, 1f)]
    public float mudSpeedMultiplier = 0.5f;

    [Header("Ice Settings")]
    public float iceMaxSpeed = 10f;
    public float iceAcceleration = 5f;
    public float iceDeceleration = 1f;

    [Header("Quicksand Settings")]
    public float quicksandSinkRate = 0.2f;
    public float quicksandMaxSlowMultiplier = 0.2f;

    // For tracking quicksand depth per player
    private Dictionary<GameObject, float> quicksandDepth = new Dictionary<GameObject, float>();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Script_Movement2D player = other.GetComponent<Script_Movement2D>();
        if (player == null) return;

        switch (hazardType)
        {
            case HazardType.Wind:
                Vector2 windForce = windDirection.normalized * windStrength;
                player.ApplyExternalForce(windForce);
                Debug.Log("Wind Hazard: Applying force " + windForce);
                break;

            case HazardType.Mud:
                player.SetSpeedModifier(mudSpeedMultiplier);
                Debug.Log("Mud Hazard: Speed reduced to " + mudSpeedMultiplier);
                break;

            case HazardType.Ice:
                player.EnableIcePhysics(iceMaxSpeed, iceAcceleration, iceDeceleration);
                Debug.Log("Ice Hazard: Ice physics applied");
                break;

            case HazardType.Quicksand:
                if (!quicksandDepth.ContainsKey(other.gameObject))
                    quicksandDepth[other.gameObject] = 0f;

                quicksandDepth[other.gameObject] += Time.deltaTime * quicksandSinkRate;
                quicksandDepth[other.gameObject] = Mathf.Clamp01(quicksandDepth[other.gameObject]);

                float slowAmount = Mathf.Lerp(1f, quicksandMaxSlowMultiplier, quicksandDepth[other.gameObject]);
                player.SetSpeedModifier(slowAmount);
                player.ApplyQuicksandSink(Time.deltaTime * quicksandSinkRate * 0.1f);
                Debug.Log("Quicksand Hazard: Depth = " + quicksandDepth[other.gameObject] + ", Speed = " + slowAmount);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Script_Movement2D player = other.GetComponent<Script_Movement2D>();
        if (player == null) return;

        player.ResetSpeedModifier();
        player.DisableIcePhysics();
        player.ExitQuicksand();
        quicksandDepth.Remove(other.gameObject);

        Debug.Log("Hazard: Player exited hazard, reset effects.");
    }
}
