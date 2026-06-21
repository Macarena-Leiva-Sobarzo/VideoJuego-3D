using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanMuerte : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset = new Vector3(0, 5, -10);

    void Start()
    {
        if (jugador == null)
        {
            GameObject jugadorGO = GameObject.FindWithTag("Player");
            if (jugadorGO != null)
            {
                jugador = jugadorGO.transform;
            }
            else
            {
                Debug.LogError("No se encontró ningún objeto con el tag 'Player'. Asigna el jugador manualmente o corrige el tag.");
            }
        }
    }

    void LateUpdate()
    {
        if (jugador != null)
        {
            transform.position = jugador.position + offset;
        }
    }
}
