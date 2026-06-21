using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraIntro : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadRotacion = 10f;
    public float distancia = 5f;

    private float tiempo = 0f;

    void Update()
    {
        tiempo += Time.deltaTime;

        //Se mueve de manera circular. Para mostrar el personaje 
        transform.RotateAround(objetivo.position, Vector3.up, velocidadRotacion * Time.deltaTime);
        transform.LookAt(objetivo);

        if (tiempo >= 20f || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Menú");
        }
    }
}
