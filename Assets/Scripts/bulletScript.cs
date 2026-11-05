using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float bulletSpeed;
    private float currentSpeed;
    public Vector3 direction;
    private float bulletLife;
    private bool canHit;

    public float damage;

    private GameManagerScript gameManager;
    private PlayerScript player;

    private void Awake()
    {
        bulletSpeed = 5f;
        currentSpeed = bulletSpeed;
        bulletLife = 1.5f;
        canHit = true;

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
        if (tag != "Player" && tag != "Bullet" && tag != "Trigger") {
            if (tag == "Enemy" && canHit) {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
                canHit = false;
            }
            Destroy(gameObject);
        }

    }
}
