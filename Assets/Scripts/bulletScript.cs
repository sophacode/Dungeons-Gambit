using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public Vector3 direction;
    private float bulletLife;

    private GameManagerScript gameManager;
    private PlayerScript player;

    private void Awake()
    {
        bulletSpeed = 5f;
        bulletLife = 1.5f;

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
        if (tag != "Player" && tag != "Bullet") {
            if (tag == "Enemy") {
                int energyValue = other.GetComponent<EnemyScript>().energyValue;
                Destroy(other.gameObject);
                gameManager.enemiesAlive--;
                player.AddEnergy(energyValue);
            }
            Destroy(gameObject);
        }

    }
}
