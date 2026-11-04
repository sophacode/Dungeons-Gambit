using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyQueenScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;

    public int energyValue;

    private EnemyHealth healthScript;

    private float damageDelay;

    private GameManagerScript gameManager;

    private float attackCooldown;

    public GameObject laser;
    public Transform laserAnchor;
    public GameObject bullet;
    public GameObject pawn;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        healthScript = GetComponent<EnemyHealth>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        healthScript.maxHealth = 3f;

        damageDelay = 0f;
        attackCooldown = 3f;
    }

    private void Update()
    {
        if (healthScript.alive && gameManager.active) {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = target.transform.position;
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f) {
                navMeshAgent.isStopped = true;
                int attack = Random.Range(1, 2);
                if (attack == 1) { LaserAttack(); }
                else if (attack == 2) { BulletRain(); }
                else if (attack == 3) { SpawnTroops(); }
                else { DashAttack(); }
                attackCooldown = 3f;
            }
        }
        else {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        damageDelay -= Time.deltaTime;
    }

    private void LaserAttack() {
        GameObject newLaser = Instantiate(laser, laserAnchor.position, Quaternion.identity);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        newLaser.GetComponent<EnemyQueenLaserScript>().anchor = laserAnchor;
        newLaser.GetComponent<EnemyQueenLaserScript>().targetDirection = direction;
    }

    private void BulletRain() {

    }

    private void SpawnTroops() {

    }

    private void DashAttack() {

    }

    private void OnDestroy()
    {
        gameManager.enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }
}
