using UnityEngine;

public class RoomTriggerScript : MonoBehaviour
{
    GameManagerScript gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameManager.active) {
            gameManager.roomCounter++;
            gameManager.active = true;
            //increase room bounds in game manager
        }
    }
}
