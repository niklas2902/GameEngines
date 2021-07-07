using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovementComplex playerMovement = collision.gameObject.GetComponent<PlayerMovementComplex>();
        if (playerMovement != null)
        {
            playerMovement.InLadderZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovementComplex playerMovement = collision.gameObject.GetComponent<PlayerMovementComplex>();
        if (playerMovement != null) {
            playerMovement.InLadderZone = false;
        }
    }
}
