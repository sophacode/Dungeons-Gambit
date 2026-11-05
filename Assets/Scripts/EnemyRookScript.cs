using UnityEngine;
using UnityEngine.AI;

public class EnemyRookScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;

    public GameObject rookBullet;

    public int energyValue;

    private EnemyHealth healthScript;

    private float damageDelay;

    private GameManagerScript gameManager;

    private float shootDelay;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        healthScript.maxHealth = 10f;

        damageDelay = 0f;
        shootDelay = 0f;
    }

    private void Update()
    {
        if (healthScript.alive && gameManager.active) {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = target.transform.position;

            shootDelay -= Time.deltaTime;
            if (navMeshAgent.remainingDistance <= 10 && shootDelay <= 0f) {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(rookBullet, transform.position, Quaternion.Euler(0,0,Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
                bullet.GetComponent<EnemyRookBulletScript>().direction = Vector2.right;
                shootDelay = 2f;
            }
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
            damageDelay = 1f;
        }
    }
}
