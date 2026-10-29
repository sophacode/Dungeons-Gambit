using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        health = maxHealth;
    }
}
