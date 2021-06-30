using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovementDuplicate playerMovement = collision.gameObject.GetComponent<PlayerMovementDuplicate>();
        if (playerMovement != null)
        {
            playerMovement.InLadderZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovementDuplicate playerMovement = collision.gameObject.GetComponent<PlayerMovementDuplicate>();
        if (playerMovement != null) {
            playerMovement.InLadderZone = false;
        }
    }
}
