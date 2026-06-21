using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float suavizado = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Obtiene la rotación del personaje
        Vector3 offsetRotado = target.rotation * offset;

        // Calcula la posición que necesito
        Vector3 posicionDeseada = target.position + offsetRotado;

        // Mueve suavemente hacia la posición que quiero
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        // Hace que la cámara siempre mire al Chan
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
