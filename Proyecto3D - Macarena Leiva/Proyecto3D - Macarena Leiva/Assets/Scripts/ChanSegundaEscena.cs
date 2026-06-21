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
    public TMP_Text textoMuerte;

    Animator anim;
    bool estaMuerto = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        //textoMuerte.gameObject.SetActive(false);
    }

    void Update()
    {
        if (estaMuerto)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection * speed * Time.deltaTime;

        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        float movimiento;

        if (Input.GetMouseButton(0))
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigos") ||
            (other.transform.parent != null && other.transform.parent.CompareTag("Enemigos")))
        {
            Debug.Log("¡Te vaz al cielo!");
            anim.SetTrigger("Muerte");
            estaMuerto = true;
            speed = 0;

            textoMuerte.gameObject.SetActive(true);

            StartCoroutine(CambioDeEscena(2f));
        }
    }

    IEnumerator CambioDeEscena(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        SceneManager.LoadScene("Cielo");
    }
}
