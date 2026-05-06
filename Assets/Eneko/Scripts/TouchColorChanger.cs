using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public sealed class TouchColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color touchedColor = Color.green;

    [SerializeField]
    private bool restoreWhenHandLeaves = true;

    [SerializeField]
    private Renderer targetRenderer;

    private XRSimpleInteractable interactable;
    private MaterialPropertyBlock propertyBlock;
    private Color initialColor = Color.white;
    private int activeHoverCount;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        propertyBlock = new MaterialPropertyBlock();
        initialColor = ReadInitialColor();
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("Hola");
        activeHoverCount++;
        SetColor(touchedColor);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        activeHoverCount = Mathf.Max(0, activeHoverCount - 1);

        if (restoreWhenHandLeaves && activeHoverCount == 0)
            SetColor(initialColor);
    }

    private Color ReadInitialColor()
    {
        if (targetRenderer == null || targetRenderer.sharedMaterial == null)
            return Color.white;

        Material material = targetRenderer.sharedMaterial;

        if (material.HasProperty(BaseColorId))
            return material.GetColor(BaseColorId);

        if (material.HasProperty(ColorId))
            return material.GetColor(ColorId);

        return Color.white;
    }

    private void SetColor(Color color)
    {
        if (targetRenderer == null)
            return;

        targetRenderer.GetPropertyBlock(propertyBlock);

        // URP/Lit suele usar _BaseColor.
        propertyBlock.SetColor(BaseColorId, color);

        // Shaders legacy suelen usar _Color.
        propertyBlock.SetColor(ColorId, color);

        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}