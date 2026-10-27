using UnityEngine;

public class BottomScript : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, 0.5f);
    }

}
