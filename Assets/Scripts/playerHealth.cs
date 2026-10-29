using UnityEngine;

public class Health : MonoBehaviour
{
    public int health; 
    
    void Update(){}

    public void takeDamage (int damage) 
        {
            health -= damage;
            if(health <= 0)
            {
                health = 0;
                Die();
            }
        }

    public void Die() {
            Destroy(gameObject);
        }
}
