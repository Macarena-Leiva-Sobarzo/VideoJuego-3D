using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class GeneradorRecolectables : MonoBehaviour
{
    public List<GameObject> prefabsRecolectables;
    public float intervalo = 2f;

    public Vector3 limiteInferior;
    public Vector3 limiteSuperior;

    public TextMeshProUGUI textoGanador;
    private int puntos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        if (textoGanador != null)
            textoGanador.gameObject.SetActive(false);

        StartCoroutine(GenerarContinuamente());
    }

    IEnumerator GenerarContinuamente()
    {
        while (!juegoTerminado)
        {
            GenerarObjeto();
            yield return new WaitForSeconds(intervalo);
        }
    }

    void GenerarObjeto()
    {
        if (prefabsRecolectables == null || prefabsRecolectables.Count == 0)
        {
            Debug.LogWarning("Lista de prefabs vacía o nula.");
            return;
        }

        int maxIntentos = 30;
        int intentos = 0;
        Vector3 posicion = Vector3.zero;
        bool posicionValida = false;

        while (!posicionValida && intentos < maxIntentos)
        {
            Vector3 puntoAleatorio = new Vector3(
                Random.Range(limiteInferior.x, limiteSuperior.x),
                Random.Range(limiteInferior.y, limiteSuperior.y),
                Random.Range(limiteInferior.z, limiteSuperior.z)
            );

            NavMeshHit hit;

            if (NavMesh.SamplePosition(puntoAleatorio, out hit, 1f, NavMesh.AllAreas))
            {
                posicion = hit.position;
                posicionValida = true;
            }
            else
            {
                intentos++;
            }
        }

        if (posicionValida)
        {
            int indice = Random.Range(0, prefabsRecolectables.Count);
            GameObject prefab = prefabsRecolectables[indice];

            if (prefab == null)
            {
                Debug.LogWarning("Prefab en la lista es null, no se puede instanciar.");
                return;
            }

            Instantiate(prefab, posicion, Quaternion.identity);
        }
        else
        {
            Debug.Log("No se encontró posición válida en NavMesh para generar el prefab.");
        }
    }

    public void SumarPunto()
    {
        if (juegoTerminado) return;

        puntos++;

        if (puntos >= 100)
        {
            juegoTerminado = true;

            if (textoGanador != null)
            {
                textoGanador.gameObject.SetActive(true);
                textoGanador.text = "¡Ganaste!";
            }

            StartCoroutine(CambiarEscena());
        }
    }

    IEnumerator CambiarEscena()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SegundaEscena");
    }
}
