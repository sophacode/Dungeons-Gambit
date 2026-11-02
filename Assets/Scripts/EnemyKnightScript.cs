using UnityEngine;
using UnityEngine.AI;

public class EnemyKnightScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;

    public int energyValue;

    private EnemyHealth healthScript;

    private float damageDelay;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();

        healthScript.maxHealth = 3f;

        damageDelay = 0f;
    }

    private void Update()
    {
        if (healthScript.alive) {
            navMeshAgent.destination = target.transform.position;
        }
        damageDelay -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == target && damageDelay <= 0f) {
            target.GetComponent<PlayerScript>().TakeDamage(2f);
            damageDelay = 1.5f;
        }
    }
}
