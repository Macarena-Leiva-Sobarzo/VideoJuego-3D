using UnityEngine;
using UnityEngine.AI;

public class EnemigoSigue : MonoBehaviour
{
    public Transform objetivo;
    private NavMeshAgent agente;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agente != null && agente.isOnNavMesh && agente.enabled)
        {
            if (Vector3.Distance(transform.position, objetivo.position) < 2f)
            {
                agente.ResetPath();
            }
            else
            {
                agente.SetDestination(objetivo.position);
            }
        }
    }
}
