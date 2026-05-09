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

    // Sistema de interaccion XR
    private XRSimpleInteractable interactable;
    private bool hasBeenHit = false;

    private void Awake()
    {
        // Obtener XRSimpleInteractable (debe estar en el GameObject)
        interactable = GetComponent<XRSimpleInteractable>();

        if (EndPoint.Instance != null)
        {
            end = EndPoint.Instance.transform;
        }

        // Buscar Counter
        counter = FindAnyObjectByType<Counter>();

        // Obtener Renderer
        proyectilRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        hasBeenHit = false;

        // Suscribirse a los eventos de hover
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }
    }

    public void Launch(float destinationOffsetRange, bool bomb, float projectileSpeed)
    {
        speed = projectileSpeed;
        lifeTime = maxLifeTime;
        isBomb = bomb;
        hasBeenHit = false;

        // Cambiar color segun tipo
        if (proyectilRenderer != null)
        {
            proyectilRenderer.material.color = isBomb ? bombColor : normalColor;
        }

        // Calcular direccion con offset aleatorio
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

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (hasBeenHit) return; // Evitar multiples hits

        hasBeenHit = true;

        if (isBomb)
        {
            // Las bombas restan 1 punto
            counter.counter--;
        }
        else
        {
            // Proyectil normal suma 1 punto
            counter.counter++;
        }

        // Desactivar el proyectil
        gameObject.SetActive(false);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        // Este evento se dispara cuando la mano deja de tocar el proyectil
    }

    public bool IsBomb()
    {
        return isBomb;
    }
}
