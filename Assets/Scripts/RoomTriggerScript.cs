using UnityEngine;

public class RoomTriggerScript : MonoBehaviour
{
    GameManagerScript gameManager;

    public int room;
    private bool activated;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        activated = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            if (!gameManager.active && !activated) {
                gameManager.EnterRoom(room);
                activated = true;
            }
        }
    }
}
