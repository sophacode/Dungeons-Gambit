using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private float life;
    private bool canHit;
    private GameObject player;
    private float y;

    private void Awake()
    {
        life = 0.5f;
        canHit = true;
        player = GameObject.FindGameObjectWithTag("Player");
        y = 0f;
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
        y += 0.1f;
        transform.localScale = new Vector3(1, y, 1);
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
                other.GetComponent<EnemyHealth>().TakeDamage(3f);
            }
        }

    }
}
