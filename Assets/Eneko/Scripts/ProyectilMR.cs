using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ProyectilMR : MonoBehaviour
{
    private Transform end;
    private float speed;
    private Vector3 direction;
    private Counter counter;
    public float maxLifeTime = 5f;
    private float lifeTime;

    // Sistema de bombas
    private bool isBomb;
    private Renderer proyectilRenderer;
    public Color normalColor = Color.blue;
    public Color bombColor = Color.red;

    // Sistema de interacción XR
    private XRSimpleInteractable interactable;
    private bool hasBeenHit = false;

    private void Awake()
    {
        // Buscar EndPoint con protección contra null
        if (EndPoint.Instance != null)
        {
            end = EndPoint.Instance.transform;
        }
        else
        {
            Debug.LogWarning(" EndPoint.Instance no encontrado. Buscando por tipo...");
            EndPoint endPoint = FindAnyObjectByType<EndPoint>();
            if (endPoint != null)
            {
                end = endPoint.transform;
                Debug.Log(" EndPoint encontrado por busqueda");
            }
            else
            {
                Debug.LogError(" No se encuentra EndPoint en la escena. Por favor añade un GameObject con el script EndPoint.");
            }
        }

        // Buscar Counter con protección contra null
        counter = FindAnyObjectByType<Counter>();
        if (counter == null)
        {
            Debug.LogError(" No se encuentra Counter en la escena. Por favor aniade un GameObject con el script Counter.");
        }
        else
        {
            Debug.Log(" Counter encontrado");
        }

        // Obtener Renderer
        proyectilRenderer = GetComponent<Renderer>();
        if (proyectilRenderer == null)
        {
            Debug.LogWarning(" Este proyectil no tiene Renderer. No se podran cambiar los colores.");
        }

        // Obtener o agregar el componente XRSimpleInteractable
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<XRSimpleInteractable>();
            Debug.Log(" XRSimpleInteractable aniadido automaticamente");
        }
    }

    private void OnEnable()
    {
        hasBeenHit = false;

        // Suscribirse a los eventos de hover
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHandTouch);
        }
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHandTouch);
        }
    }

    public void Launch(float destinationOffsetRange, bool bomb, float projectileSpeed)
    {
        speed = projectileSpeed;
        lifeTime = maxLifeTime;
        isBomb = bomb;
        hasBeenHit = false;

        // Cambiar color según tipo
        if (proyectilRenderer != null)
        {
            proyectilRenderer.material.color = isBomb ? bombColor : normalColor;
        }

        // Verificar que tenemos un punto de destino
        if (end == null)
        {
            Debug.LogError(" No hay EndPoint configurado. El proyectil no puede calcular dirección.");
            // Dirección por defecto: hacia adelante de la cámara
            direction = Camera.main != null ? Camera.main.transform.forward : Vector3.forward;
            return;
        }

        // Calcular dirección con offset aleatorio
        float offset = Random.Range(-destinationOffsetRange, destinationOffsetRange);
        Vector3 targetPos = new Vector3(end.position.x + offset, end.position.y, end.position.z);
        direction = (targetPos - transform.position).normalized;
    }

    private void Update()
    {
        if (!hasBeenHit)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            LifeTime();
        }
    }

    void LifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Este método se llama cuando una mano toca el proyectil
    private void OnHandTouch(HoverEnterEventArgs args)
    {
        if (hasBeenHit) return; // Evitar múltiples hits

        hasBeenHit = true;

        // Verificar que tenemos contador
        if (counter == null)
        {
            Debug.LogError("❌ No hay Counter disponible para actualizar puntos!");
            gameObject.SetActive(false);
            return;
        }

        if (isBomb)
        {
            // Las bombas restan 1 punto
            counter.counter--;
            Debug.Log(" ¡Bomba golpeada! -1 punto. Total: " + counter.counter);
        }
        else
        {
            // Proyectil normal suma 1 punto
            counter.counter++;
            Debug.Log(" ¡Proyectil golpeado! +1 punto. Total: " + counter.counter);
        }

        // Desactivar el proyectil
        gameObject.SetActive(false);
    }

    public bool IsBomb()
    {
        return isBomb;
    }
}