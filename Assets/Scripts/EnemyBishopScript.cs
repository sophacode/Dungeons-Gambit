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

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();

        healthScript.maxHealth = 5f;

    }

    private void Update()
    {
        navMeshAgent.destination = target.transform.position;

        shootDelay -= Time.deltaTime;
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && shootDelay <= 0f) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(enemyBishopBUllet, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBishopBulletScript>().direction = direction;
            shootDelay = 1f;
        }
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }
}
