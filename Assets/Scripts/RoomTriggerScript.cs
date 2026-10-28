using UnityEngine;

public class RoomTriggerScript : MonoBehaviour
{
    GameManagerScript gameManager;

    public int room;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameManager.active) {
            gameManager.roomCounter = room;
            gameManager.active = true;
        }
    }
}
