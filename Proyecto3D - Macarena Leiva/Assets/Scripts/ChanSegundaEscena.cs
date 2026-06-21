using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChanSegundaEscena : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;
    public float mouseSpeed = 3.0f;
    public float fuerzaSalto = 5f;

    public TMP_Text textoMuerte;
    public GameObject joystickUI;
    public GameObject botonA;
    public GameObject botonB;
    public GameObject panelMenu;

    private Rigidbody rb;
    private Animator anim;

    private bool joystickActivado = false;
    private bool estaMuerto = false;
    private bool enSuelo = true;
    private bool corriendoDesdeUI = false;
    private bool juegoPausado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        textoMuerte.gameObject.SetActive(false);
        panelMenu.SetActive(false);

        joystickActivado = true;
        joystickUI.SetActive(true);
        botonA.SetActive(true);
        botonB.SetActive(true);
    }

    void Update()
    {
        if (estaMuerto) return;

        VerificarSuelo();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            joystickActivado = !joystickActivado;

            joystickUI.SetActive(joystickActivado);
            botonA.SetActive(joystickActivado);
            botonB.SetActive(joystickActivado);
        }

        if (!joystickActivado && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (joystickActivado && Input.GetKeyDown("joystick button 1"))
        {
            ToggleMenu();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection * speed * Time.deltaTime;

        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        float movimiento = 0f;

        if (!joystickActivado && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX * mouseSpeed, 0);

            Vector3 forward = transform.forward;
            transform.position += forward * speed * Time.deltaTime;

            movimiento = 1.0f;
        }
        else
        {
            movimiento = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
        }

        anim.SetFloat("Movimiento", movimiento);
        anim.SetBool("EnSuelo", enSuelo);

        if ((joystickActivado && Input.GetKeyDown("joystick button 2")) ||
            (!joystickActivado && Input.GetKeyDown(KeyCode.Space)))
        {
            Saltar();
        }

        if ((joystickActivado && Input.GetKey("joystick button 0")) || corriendoDesdeUI)
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }
    }

    public void Saltar()
    {
        if (enSuelo && !estaMuerto)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            anim.SetTrigger("Salto");
            enSuelo = false;
        }
    }

    void VerificarSuelo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            enSuelo = true;
        }
        else
        {
            enSuelo = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigos") ||
            (other.transform.parent != null && other.transform.parent.CompareTag("Enemigos")))
        {
            Debug.Log("Te mataron. Te vas al cielo");
            anim.SetTrigger("Muerte");
            estaMuerto = true;
            speed = 0;

            textoMuerte.gameObject.SetActive(true);
            StartCoroutine(CambioDeEscena(2f));
        }

        if (other.CompareTag("Premio"))
        {
            if (ControlPuntaje.Instancia != null && ControlPuntaje.Instancia.puntaje >= 200)
            {
                Debug.Log("¡Haz ganado! Llegaste a los 200 puntos");

                ControlPuntaje.Instancia.MostrarGanador();

                estaMuerto = true;
                speed = 0f;
                Time.timeScale = 0f;

                StartCoroutine(CambioSegundaEscenaTiempoReal(2f));
            }
            else
            {
                Debug.Log("Tienes que llegar a los 200 puntos");
            }
        }
    }

    IEnumerator CambioDeEscena(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        SceneManager.LoadScene("Cielo");
    }

    IEnumerator CambioSegundaEscenaTiempoReal(float segundos)
    {
        yield return new WaitForSecondsRealtime(segundos);
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaFinal");
    }

    void ToggleMenu()
    {
        juegoPausado = !juegoPausado;
        panelMenu.SetActive(juegoPausado);
        Time.timeScale = juegoPausado ? 0 : 1;
    }

    public void BotonAClickPresionado()
    {
        corriendoDesdeUI = true;
    }

    public void BotonAClickSoltado()
    {
        corriendoDesdeUI = false;
    }

    public void BotonBClickPresionado()
    {
        Saltar();
    }

    public void BotonXClickPresionado()
    {
        if (joystickActivado)
        {
            ToggleMenu();
        }
    }
}
