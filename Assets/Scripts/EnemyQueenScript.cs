using System;
using System.Collections;
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

        healthScript.maxHealth = 100f;

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
                int attack = UnityEngine.Random.Range(1, 5);
                if (attack == 1) { LaserAttack(); }
                else if (attack == 2) { BulletRain(); }
                else if (attack == 3) { SpawnTroops(); }
                else { DashAttack(); }
                attackCooldown = UnityEngine.Random.Range(3f, 6f);
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
        StartCoroutine(BulletRainCR());
    }

    private IEnumerator BulletRainCR() {
        float a = 0f;
        float b = 15f;
        for (int j = 0; j < 5; j ++) {
            for (int i = 0; i < 8; i++) {
                float angle = a + (45f * i);
                float angleR = angle * Mathf.Deg2Rad;
                Vector2 v = new(Mathf.Cos(angleR), Mathf.Sin(angleR));
                GameObject newBullet = Instantiate(bullet, laserAnchor.position, Quaternion.identity);
                newBullet.GetComponent<EnemyBishopBulletScript>().direction = v;
                newBullet.GetComponent<EnemyBishopBulletScript>().bulletSpeed = 7f;
            }
            yield return new WaitForSeconds(0.75f);
            for (int i = 0; i < 8; i++) {
                float angle = b + (45f * i);
                float angleR = angle * Mathf.Deg2Rad;
                Vector2 v = new(Mathf.Cos(angleR), Mathf.Sin(angleR));
                GameObject newBullet = Instantiate(bullet, laserAnchor.position, Quaternion.identity);
                newBullet.GetComponent<EnemyBishopBulletScript>().direction = v;
                newBullet.GetComponent<EnemyBishopBulletScript>().bulletSpeed = 7f;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }

    private void SpawnTroops() {
        int r = UnityEngine.Random.Range(4, 8);
        Debug.Log(r);
        StartCoroutine(Spawn(r));
    }

    private IEnumerator Spawn(int number) {
        for (int i = 0; i < number; i++){
            Debug.Log("Spawn new pawn");
            GameObject newPawn = Instantiate(pawn, laserAnchor.position, Quaternion.identity);
            newPawn.GetComponent<EnemyScript>().energyValue = 1;
            gameManager.enemiesAlive += 1;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void DashAttack() {
        StartCoroutine(Dash());
    }

    private IEnumerator Dash() {
        navMeshAgent.isStopped = false;
        //warm up
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(1);
        //dash
        navMeshAgent.acceleration = 20f;
        navMeshAgent.speed = 50f;
        yield return new WaitForSeconds(1);
        navMeshAgent.speed = 1.5f;
    }

    private void OnDestroy()
    {
        gameManager.enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }
}
