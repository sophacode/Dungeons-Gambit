using UnityEngine;

public class EnemyBishopBulletScript : MonoBehaviour
{
    public float bulletSpeed;
    private float currentSpeed;
    public Vector3 direction;
    private float bulletLife;
    private bool canKill;

    private GameManagerScript gameManager;
    private PlayerScript player;

    private void Awake()
    {
        bulletSpeed = 5f;
        currentSpeed = bulletSpeed;
        bulletLife = 3f;
        canKill = true;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

    }

    public void PauseBullet() {
        currentSpeed = 0f;
    }

    public void ResetSpeed() {
        currentSpeed = bulletSpeed;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * currentSpeed);
        bulletLife -= Time.deltaTime;
        if (bulletLife < 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag != "Bullet" && tag != "Enemy" && tag != "Trigger") {
            if (tag == "Player" && canKill) {
                player.TakeDamage(1f, new(direction.x, direction.y));
                canKill = false;
            }
            Destroy(gameObject);
        }

    }
}
