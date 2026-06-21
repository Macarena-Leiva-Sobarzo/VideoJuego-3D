using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlPuntaje : MonoBehaviour
{
    public static ControlPuntaje Instancia;

    public int puntaje = 0;
    public Text textoPuntaje;
    public TextMeshProUGUI textoGanador;

    public GameObject animacionGanador;  // Aquí asignas tu animación en el Inspector

    private bool juegoTerminado = false;

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
            textoGanador.gameObject.SetActive(false);

        if (animacionGanador != null)
            animacionGanador.SetActive(false);
    }

    public void SumarPuntos(int puntos)
    {
        if (juegoTerminado) return;

        puntaje += puntos;
        ActualizarTexto();

        if (puntaje >= 100)
        {
            juegoTerminado = true;

            if (textoGanador != null)
            {
                textoGanador.gameObject.SetActive(true);
                textoGanador.text = "¡Ganaste!";
            }

            if (animacionGanador != null)
                animacionGanador.SetActive(true);  // Activa la animación

            Invoke(nameof(CambiarEscena), 2f);
        }
    }

    void ActualizarTexto()
    {
        textoPuntaje.text = "Puntos: " + puntaje;
    }

    void CambiarEscena()
    {
        SceneManager.LoadScene("SegundaEscena");
    }
}
