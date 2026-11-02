using UnityEngine;
using UnityEngine.AI;

public class EnemyBishopScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;
    private float shootDelay;
    public GameObject enemyBishopBUllet;

    public int energyValue;
    private EnemyHealth healthScript;

    private GameManagerScript gameManager;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        healthScript.maxHealth = 5f;

    }

    private void Update()
    {
        if (healthScript.alive && gameManager.active) {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = target.transform.position;

            shootDelay -= Time.deltaTime;
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && shootDelay <= 0f) {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(enemyBishopBUllet, transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBishopBulletScript>().direction = direction;
                shootDelay = 1f;
            }
        }
        else {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        gameManager.enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }
}
