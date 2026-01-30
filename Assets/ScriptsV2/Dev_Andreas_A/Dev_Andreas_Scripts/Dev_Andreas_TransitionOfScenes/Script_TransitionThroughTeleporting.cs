using UnityEngine;
public class Script_TransitionThroughTeleporting : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;
    public bool resetVelocityOnTeleport = true;

    [Header("Interaction Prompt")]
    public GameObject interactionText;

    [Header("Input")]
    public KeyCode interactionKey = KeyCode.F;

    private GameObject player;
    private Rigidbody2D playerRb;
    private bool playerInRange = false;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false);
            Debug.Log("[Teleport] Interaction text hidden on start");
        }
        else
        {
            Debug.LogWarning("[Teleport] interactionText is NOT assigned");
        }

        if (teleportTarget == null)
        {
            Debug.LogWarning("[Teleport] teleportTarget is NOT assigned");
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            Debug.Log("[Teleport] Player is in range");

            if (Input.GetKeyDown(interactionKey))
            {
                Debug.Log("[Teleport] Interaction key pressed");
                TeleportPlayer2DSafe();
            }
        }
    }

    void TeleportPlayer2DSafe()
    {
        Debug.Log("[Teleport] Attempting teleport");

        if (player == null)
        {
            Debug.LogWarning("[Teleport] player reference is NULL");
            return;
        }

        if (playerRb == null)
        {
            Debug.LogWarning("[Teleport] Rigidbody2D is NULL on player");
            return;
        }

        if (teleportTarget == null)
        {
            Debug.LogWarning("[Teleport] teleportTarget is NULL");
            return;
        }

        Vector3 current3D = player.transform.position;
        Vector2 target2D = new Vector2(teleportTarget.position.x, teleportTarget.position.y);

        Debug.Log(
            "[Teleport] Current position: " + current3D +
            " Target position (XY): " + target2D
        );

        if (resetVelocityOnTeleport)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            Debug.Log("[Teleport] Velocity reset");
        }

        playerRb.position = target2D;
        player.transform.position = new Vector3(target2D.x, target2D.y, current3D.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[Teleport] OnTriggerEnter2D fired by object: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Teleport] Object entering is Player");

            player = other.gameObject;
            playerRb = player.GetComponent<Rigidbody2D>();

            if (playerRb == null)
            {
                Debug.LogWarning("[Teleport] Player has NO Rigidbody2D");
            }
            else
            {
                Debug.Log("[Teleport] Rigidbody2D found on player");
            }

            playerInRange = true;

            if (interactionText != null)
            {
                interactionText.SetActive(true);
                Debug.Log("[Teleport] Interaction text shown");
            }
        }
        else
        {
            Debug.Log("[Teleport] Object entering is NOT Player. Tag: " + other.tag);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("[Teleport] OnTriggerExit2D fired by object: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerRb = null;

            if (interactionText != null)
            {
                interactionText.SetActive(false);                
            }
        }
    }
}