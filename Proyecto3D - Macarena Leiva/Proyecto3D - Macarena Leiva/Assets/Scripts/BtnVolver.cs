using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnVolver : MonoBehaviour
{
    public void Volver()
    {
        SceneManager.LoadScene("Menú");
    }
}
