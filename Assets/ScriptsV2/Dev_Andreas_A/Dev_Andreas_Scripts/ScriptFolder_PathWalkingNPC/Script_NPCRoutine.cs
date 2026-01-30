using System.Collections;
using UnityEngine;
// IS DIS ONE
#if UNITY_EDITOR
using UnityEditor;
#endif

// Serializable data structure for each waypoint in the NPC's path
[System.Serializable]
public class WaypointNode
{
    public Transform point;              // The location the NPC should walk to
    public float waitTime = 1f;          // Time to wait after reaching the point
    public string animationTrigger;      // Name of the animation to trigger at this point
    public bool performActionHere = false; // Whether to play the animation here
}

public class Script_NPCRoutine : MonoBehaviour
{
    [Header("Path Settings")]
    public WaypointNode[] path;              // Array of waypoints defining the path
    public bool loopPath = true;             // Whether to restart from the beginning when done
    public bool randomizeStartPoint = false; // Start at a random point on the path

    [Header("Movement Settings")]
    public float walkSpeed = 2f;             // Speed at which the NPC moves

    [Header("Routine Control")]
    public bool isActiveRoutine = true;      // Whether the NPC routine should run
    public bool canBeInterrupted = true;     // Whether the player can pause this routine

    [Header("References")]
    public Animator animator;                // Optional Animator component for triggering animations

    [Header("Debug Visuals")]
    public Color pathColor = Color.cyan;     // Color used to draw the waypoint path in the Scene view

    // Internal state tracking
    private int currentNodeIndex = 0;
    private bool isMoving = false;
    private bool isInterrupted = false;

    private void Start()
    {
        if (path == null || path.Length == 0) return;

        // Start at a random path index if enabled
        if (randomizeStartPoint)
            currentNodeIndex = Random.Range(0, path.Length);

        // Begin walking routine
        StartCoroutine(RoutineLoop());
    }

    // Main loop coroutine that drives movement along the path
    private IEnumerator RoutineLoop()
    {
        while (isActiveRoutine)
        {
            if (!isInterrupted)
            {
                // Move to the current waypoint and perform any actions
                yield return StartCoroutine(MoveToWaypoint(path[currentNodeIndex]));

                // Move to the next waypoint
                currentNodeIndex++;

                // If we’re out of waypoints, restart or stop based on loop setting
                if (currentNodeIndex >= path.Length)
                {
                    if (loopPath)
                        currentNodeIndex = 0;
                    else
                        yield break; // Stop the routine if not looping
                }
            }
            else
            {
                // If interrupted, wait until resumed
                yield return null;
            }
        }
    }

    // Handles walking toward a waypoint and waiting/animating at the destination
    private IEnumerator MoveToWaypoint(WaypointNode node)
    {
        isMoving = true;

        // Move toward the point until we are close enough
        while (Vector2.Distance(transform.position, node.point.position) > 0.05f)
        {
            Vector2 direction = (node.point.position - transform.position).normalized;
            transform.position += (Vector3)direction * walkSpeed * Time.deltaTime;

            // Trigger walk animation if available
            animator?.SetBool("IsWalking", true);

            yield return null;
        }

        // Stop walk animation once we arrive
        animator?.SetBool("IsWalking", false);

        // If this node has an animation to play, trigger it
        if (node.performActionHere && animator != null && !string.IsNullOrEmpty(node.animationTrigger))
        {
            animator.SetTrigger(node.animationTrigger);
        }

        // Wait before moving on
        yield return new WaitForSeconds(node.waitTime);

        isMoving = false;
    }

    // Call this from another system (e.g. player interaction) to pause the NPC
    public void InterruptRoutine()
    {
        if (canBeInterrupted)
        {
            isInterrupted = true;
            animator?.SetBool("IsWalking", false);
        }
    }

    // Call this to resume the NPC after an interruption
    public void ResumeRoutine()
    {
        isInterrupted = false;
    }

    // Dynamically switch the NPC's path during runtime
    public void SetNewPath(WaypointNode[] newPath, bool restart = true)
    {
        StopAllCoroutines();     // Stop any ongoing movement
        path = newPath;          // Assign new path
        currentNodeIndex = 0;    // Reset index

        if (restart)
            StartCoroutine(RoutineLoop()); // Restart routine
    }

    // Always draw the waypoint path in the Scene view
    private void OnDrawGizmos()
    {
        if (path == null || path.Length == 0)
            return;

        Gizmos.color = pathColor;

        for (int i = 0; i < path.Length; i++)
        {
            if (path[i].point == null) continue;

            // Draw a sphere at the waypoint position
            Gizmos.DrawWireSphere(path[i].point.position, 0.2f);

            // Draw a line to the next point
            if (i + 1 < path.Length && path[i + 1].point != null)
            {
                Gizmos.DrawLine(path[i].point.position, path[i + 1].point.position);
            }
            else if (loopPath && path[0].point != null)
            {
                Gizmos.DrawLine(path[i].point.position, path[0].point.position);
            }
        }
    }
}
