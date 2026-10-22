using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public Vector3 direction;
    private float bulletLife;

    private void Awake()
    {
        bulletSpeed = 5f;
        bulletLife = 10f;

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
        if (other.gameObject.tag != "Player") {
            if (other.gameObject.tag == "Enemy") {
                Destroy(other.gameObject);
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().enemiesAlive--;
            }
            Destroy(gameObject);
        }

    }
}
