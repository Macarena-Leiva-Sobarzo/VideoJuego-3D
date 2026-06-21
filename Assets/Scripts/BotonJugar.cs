using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonJugar : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Escena");
    }
}
