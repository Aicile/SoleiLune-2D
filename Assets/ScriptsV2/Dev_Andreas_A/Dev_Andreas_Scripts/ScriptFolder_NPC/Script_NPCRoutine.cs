using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Serializable class for defining individual waypoint behaviors
[System.Serializable]
public class WaypointNode
{
    public Transform point;              // The position the NPC will walk to
    public float waitTime = 1f;          // How long to wait at this point
    public string animationTrigger;      // Optional animation to play
    public bool performActionHere = false; // Whether to perform animation here
}

public class Script_NPCRoutine : MonoBehaviour
{
    [Header("Path Settings")]
    public WaypointNode[] path;              // Array of waypoints with behavior
    public bool loopPath = true;             // Should the NPC repeat the path
    public bool randomizeStartPoint = false; // Start at a random node on spawn

    [Header("Movement Settings")]
    public float walkSpeed = 2f;             // How fast the NPC moves

    [Header("Routine Control")]
    public bool isActiveRoutine = true;      // Toggle to start/stop routine
    public bool canBeInterrupted = true;     // Can player interrupt movement?

    [Header("References")]
    public Animator animator;                // Reference to Animator (optional)

    private int currentNodeIndex = 0;        // Track current node
    private bool isMoving = false;           // Prevent overlapping coroutines
    private bool isInterrupted = false;      // Flag for pausing when interrupted

    [Header("Debug Visuals")]
    public Color pathColor = Color.cyan;

    private void Start()
    {
        if (path == null || path.Length == 0) return;

        // Start at random node if desired
        if (randomizeStartPoint)
            currentNodeIndex = Random.Range(0, path.Length);

        StartCoroutine(RoutineLoop());
    }

    // Main routine loop
    private IEnumerator RoutineLoop()
    {
        while (isActiveRoutine)
        {
            if (!isInterrupted)
            {
                yield return StartCoroutine(MoveToWaypoint(path[currentNodeIndex]));

                // Advance to next node
                currentNodeIndex++;
                if (currentNodeIndex >= path.Length)
                {
                    if (loopPath)
                        currentNodeIndex = 0;
                    else
                        yield break; // End routine
                }
            }
            else
            {
                // Wait while interrupted
                yield return null;
            }
        }
    }

    // Move toward a waypoint and optionally do an action
    private IEnumerator MoveToWaypoint(WaypointNode node)
    {
        isMoving = true;

        // Move until close enough
        while (Vector2.Distance(transform.position, node.point.position) > 0.05f)
        {
            Vector2 direction = (node.point.position - transform.position).normalized;
            transform.position += (Vector3)direction * walkSpeed * Time.deltaTime;

            // Optionally trigger a "walking" animation here
            animator?.SetBool("IsWalking", true);

            yield return null;
        }

        // Stop walking animation
        animator?.SetBool("IsWalking", false);

        // Perform optional animation/action
        if (node.performActionHere && animator != null && !string.IsNullOrEmpty(node.animationTrigger))
        {
            animator.SetTrigger(node.animationTrigger);
        }

        // Wait before moving to next node
        yield return new WaitForSeconds(node.waitTime);

        isMoving = false;
    }

    // Call this from dialogue or other systems to pause NPC
    public void InterruptRoutine()
    {
        if (canBeInterrupted)
        {
            isInterrupted = true;
            animator?.SetBool("IsWalking", false);
        }
    }

    // Resume routine after interruption
    public void ResumeRoutine()
    {
        isInterrupted = false;
    }

    // Optional: Call to forcefully switch path
    public void SetNewPath(WaypointNode[] newPath, bool restart = true)
    {
        StopAllCoroutines();
        path = newPath;
        currentNodeIndex = 0;
        if (restart)
        {
            StartCoroutine(RoutineLoop());
        }
    }

    // Draw lines and spheres between waypoints for visual clarity in the editor
    private void OnDrawGizmos()
    {
        if (path == null || path.Length == 0)
            return;

        Gizmos.color = pathColor;

        for (int i = 0; i < path.Length; i++)
        {
            if (path[i].point == null) continue;

            Gizmos.DrawWireSphere(path[i].point.position, 0.2f);

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
