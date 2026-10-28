using UnityEngine;

public class EnemyBishopBulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public Vector3 direction;
    private float bulletLife;
    private bool canKill;

    private GameManagerScript gameManager;
    private PlayerScript player;

    private void Awake()
    {
        bulletSpeed = 5f;
        bulletLife = 1.5f;
        canKill = true;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * bulletSpeed);
        bulletLife -= Time.deltaTime;
        if (bulletLife < 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag != "Bullet" && tag != "Enemy") {
            if (tag == "Player" && canKill) {
                Destroy(other.gameObject);
                canKill = false;
            }
            Destroy(gameObject);
        }

    }
}
