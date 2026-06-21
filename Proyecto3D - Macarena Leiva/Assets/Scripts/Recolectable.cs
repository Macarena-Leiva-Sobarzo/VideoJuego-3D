using UnityEngine;

public class Recolectable : MonoBehaviour
{
    public int valor = 10;

    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            ControlPuntaje.Instancia.SumarPuntos(valor);
            Destroy(gameObject);
        }
    }
}
