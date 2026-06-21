using UnityEngine;
using UnityEngine.UI;

public class ControlPuntaje : MonoBehaviour
{
    public static ControlPuntaje Instancia;

    public int puntaje = 0;
    public Text textoPuntaje;

    public GameObject textoGanador;
    public GameObject animacionGanador;

    void Awake()
    {
        if (Instancia == null)
            Instancia = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (textoGanador != null)
            textoGanador.SetActive(false);

        if (animacionGanador != null)
            animacionGanador.SetActive(false);
    }

    public void SumarPuntos(int puntos)
    {
        puntaje += puntos;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        textoPuntaje.text = "Puntos: " + puntaje;
    }

    public void MostrarGanador()
    {
        if (textoGanador != null)
            textoGanador.SetActive(true);

        if (animacionGanador != null)
            animacionGanador.SetActive(true);
    }
}
