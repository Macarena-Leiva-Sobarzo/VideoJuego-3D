using UnityEngine;
using UnityEngine.AI;

public class EnemigoSigue : MonoBehaviour
{
    public Transform player;
    public float distanciaAlerta = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia <= distanciaAlerta)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}