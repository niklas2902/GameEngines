using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }

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
