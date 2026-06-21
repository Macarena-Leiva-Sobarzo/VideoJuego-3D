using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public GameObject panelBotonesMenu;
    public GameObject panelInstrucciones;
    public MovChanMuerte controlMovimiento;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelBotonesMenu.activeSelf || panelInstrucciones.activeSelf)
                ReanudarJuego();
            else
                MostrarMenu();
        }
    }

    public void ReanudarJuego()
    {
        panelBotonesMenu.SetActive(false);
        panelInstrucciones.SetActive(false);
        Time.timeScale = 1f;

        if (controlMovimiento != null)
            controlMovimiento.ReanudarDesdeMenu();
    }


    public void MostrarMenu()
    {
        panelBotonesMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MostrarInstrucciones()
    {
        panelBotonesMenu.SetActive(false);
        panelInstrucciones.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelInstrucciones.SetActive(false);
        panelBotonesMenu.SetActive(true);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
