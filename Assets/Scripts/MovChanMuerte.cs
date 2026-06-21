using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MovChanMuerte : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadRotacion = 200f;
    private Rigidbody rb;
    private Animator animator;

    public TextMeshProUGUI textoSobrevivir;
    public TextMeshProUGUI textoCronometro;

    public TextMeshProUGUI textoMuerte;

    public GameObject joystickUI;
    public GameObject botonA;
    public GameObject panelMenu;

    private float tiempoRestante = 45f;
    private bool estaMuerta = false;

    private bool joystickActivado = false;
    private bool corriendoDesdeUI = false;
    private bool juegoPausado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(MostrarTextoInicio());

        joystickActivado = true;
        joystickUI.SetActive(true);
        botonA.SetActive(true);

        panelMenu.SetActive(false);

        if (textoMuerte != null)
            textoMuerte.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (estaMuerta || juegoPausado) return;

        Vector3 movimiento = Vector3.zero;

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        if (!joystickActivado && Input.GetMouseButton(0))
        {
            float rotacionMouse = Input.GetAxis("Mouse X") * velocidadRotacion * Time.fixedDeltaTime;
            transform.Rotate(0f, rotacionMouse, 0f);

            movimiento = transform.forward * velocidad;
        }
        else
        {
            Vector3 direccionEntrada = new Vector3(movimientoHorizontal, 0f, movimientoVertical);
            movimiento = transform.TransformDirection(direccionEntrada.normalized) * velocidad;

            if (direccionEntrada != Vector3.zero && movimientoVertical > 0f)
            {
                Quaternion nuevaRotacion = Quaternion.LookRotation(new Vector3(movimientoHorizontal, 0f, movimientoVertical));
                transform.rotation = Quaternion.Slerp(transform.rotation, nuevaRotacion, 0.15f);
            }
        }

        if ((joystickActivado && Input.GetKey("joystick button 0")) || corriendoDesdeUI)
        {
            movimiento *= 2f;
        }

        rb.velocity = movimiento;

        float velocidadActual = new Vector2(movimientoHorizontal, movimientoVertical).magnitude;
        animator.SetFloat("Velocidad", velocidadActual > 0 ? 1f : 0f);
    }

    void Update()
    {
        if (estaMuerta) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            joystickActivado = !joystickActivado;
            joystickUI.SetActive(joystickActivado);
            botonA.SetActive(joystickActivado);
        }

        if (!joystickActivado && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        if (joystickActivado && Input.GetKeyDown("joystick button 1"))
        {
            ToggleMenu();
        }

        if (!juegoPausado)
        {
            tiempoRestante -= Time.deltaTime;
            int segundos = Mathf.CeilToInt(tiempoRestante);
            textoCronometro.text = "Tiempo: " + segundos.ToString() + "s";

            if (tiempoRestante <= 0f)
            {
                SceneManager.LoadScene("Escena");
            }
        }
    }

    IEnumerator MostrarTextoInicio()
    {
        textoSobrevivir.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        textoSobrevivir.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Enemigos"))
        {
            Debug.Log("Vuelves a empezar");
            estaMuerta = true;
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Muerte");
            animator.SetFloat("Velocidad", 0f);

            if (textoMuerte != null)
            {
                textoMuerte.gameObject.SetActive(true);
            }

            StartCoroutine(ReiniciarEscena());
        }
        else if (other.CompareTag("Premio"))
        {
            Debug.Log("Haz vuelto a la vida");
            SceneManager.LoadScene("Escena");
        }
    }

    IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Cielo");
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

    public void BotonXClickPresionado()
    {
        if (joystickActivado)
        {
            ToggleMenu();
        }
    }

    public void ReanudarDesdeMenu()
    {
        juegoPausado = false;
        panelMenu.SetActive(false);
        joystickActivado = true;
        joystickUI.SetActive(true);
        botonA.SetActive(true);
        rb.velocity = Vector3.zero;
        corriendoDesdeUI = false;
    }
}
