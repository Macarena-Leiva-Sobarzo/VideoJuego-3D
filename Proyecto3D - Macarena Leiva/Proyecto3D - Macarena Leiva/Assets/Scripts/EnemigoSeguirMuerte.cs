using UnityEngine;
using UnityEngine.AI;

public class EnemigoSeguirMuerte : MonoBehaviour
{
    private Transform objetivo;
    private NavMeshAgent agente;

    void Start()
    {
        objetivo = GameObject.FindGameObjectWithTag("Player").transform;
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (objetivo != null)
        {
            agente.SetDestination(objetivo.position);
        }
    }
}
