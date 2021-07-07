using UnityEngine;
using TMPro;
public class Cherry : Item
{
    public Canvas uiCancas;

    public GameObject player;

    public void OnDestroy()
    {
        if (uiCancas != null)
        {
            uiCancas.gameObject.SetActive(true);
        }
        if (player != null)
        {
            player.GetComponent<PlayerMovementComplex>().Won = true;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<Animator>().SetFloat("Speed", 0);
        }
    }
}
