using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;

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
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().enemiesAlive--;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().AddEnergy(energyValue);
    }

}
