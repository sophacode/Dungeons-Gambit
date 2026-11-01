using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    private float life;
    private bool canHit;
    private GameObject player;

    private void Awake()
    {
        life = 1f;
        canHit = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = player.transform.position;
        life -= Time.deltaTime;
        if (life < 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag != "Player" && tag != "Bullet") {
            if (tag == "Enemy" && canHit) {
                other.GetComponent<EnemyHealth>().TakeDamage(1f);
                canHit = false;
            }
        }

    }
}
