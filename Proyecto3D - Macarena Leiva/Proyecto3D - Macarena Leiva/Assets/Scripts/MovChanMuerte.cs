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

    public GameObject textoTMP;
    public TextMeshProUGUI textoCronometro;

    private float tiempoRestante = 45f;
    private bool estaMuerta = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(MostrarTexto());
    }

    void FixedUpdate()
    {
        if (estaMuerta) return;

        Vector3 movimiento = Vector3.zero;

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Se mueve con el mouse
        if (Input.GetMouseButton(0))
        {
            float rotacionMouse = Input.GetAxis("Mouse X") * velocidadRotacion * Time.fixedDeltaTime;
            transform.Rotate(0f, rotacionMouse, 0f);

            movimiento = transform.forward * velocidad;
        }
        else
        {
            // Se mueve con el teclado, pero no rota automáticamente
            Vector3 direccionEntrada = new Vector3(movimientoHorizontal, 0f, movimientoVertical);
            movimiento = transform.TransformDirection(direccionEntrada.normalized) * velocidad;

            // Rota solo si hay dirección
            if (direccionEntrada != Vector3.zero && movimientoVertical > 0f)
            {
                Quaternion nuevaRotacion = Quaternion.LookRotation(new Vector3(movimientoHorizontal, 0f, movimientoVertical));
                transform.rotation = Quaternion.Slerp(transform.rotation, nuevaRotacion, 0.15f);
            }
        }

        rb.velocity = movimiento;

        float velocidadActual = new Vector2(movimientoHorizontal, movimientoVertical).magnitude;
        animator.SetFloat("Velocidad", velocidadActual > 0 ? 1f : 0f);
    }

    void Update()
    {
        if (estaMuerta) return;

        tiempoRestante -= Time.deltaTime;
        int segundos = Mathf.CeilToInt(tiempoRestante);
        textoCronometro.text = "Tiempo: " + segundos.ToString() + "s";

        if (tiempoRestante <= 0f)
        {
            SceneManager.LoadScene("Escena");
        }
    }

    IEnumerator MostrarTexto()
    {
        textoTMP.SetActive(true);
        yield return new WaitForSeconds(5f);
        textoTMP.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Enemigos"))
        {
            Debug.Log("Chan ha muerto...");
            estaMuerta = true;
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Muerte");
            animator.SetFloat("Velocidad", 0f);
            StartCoroutine(ReiniciarEscena());
        }
        else if (other.CompareTag("Premio"))
        {
            Debug.Log("¡¡VUELVES A LA VIDA!!");
            SceneManager.LoadScene("Escena");
        }
    }

    IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Cielo");
    }
}
