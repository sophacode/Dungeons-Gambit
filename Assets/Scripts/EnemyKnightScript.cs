using UnityEngine;
using UnityEngine.AI;

public class EnemyKnightScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;

    public int energyValue;

    private EnemyHealth healthScript;

    private float damageDelay;

    private GameManagerScript gameManager;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        healthScript.maxHealth = 3f;

        damageDelay = 0f;
    }

    private void Update()
    {
        if (healthScript.alive && gameManager) {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = target.transform.position;
        }
        else {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        damageDelay -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        gameManager.enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == target && damageDelay <= 0f) {
            target.GetComponent<PlayerScript>().TakeDamage(2f, navMeshAgent.velocity.normalized);
            damageDelay = 0.5f;
        }
    }
}
