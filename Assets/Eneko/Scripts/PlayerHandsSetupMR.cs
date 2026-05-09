using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerHandsSetupMR : MonoBehaviour
{
    [Header("Referencias de las manos del XR Origin Hands")]
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Configuración")]
    public bool useGameConfig = true;

    [Header("Interaction Layer")]
    public string interactionLayerName = "HandTouch";

    private void Start()
    {
        SetupHands();

        // Resetear posición del jugador
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void SetupHands()
    {
        if (useGameConfig)
        {
            // Configurar según GameConfig
            if (leftHand != null)
            {
                leftHand.SetActive(GameConfig.leftSableActive);
                Debug.Log($"Mano izquierda: {(GameConfig.leftSableActive ? "Activada" : "Desactivada")}");
            }

            if (rightHand != null)
            {
                rightHand.SetActive(GameConfig.rightSableActive);
                Debug.Log($"Mano derecha: {(GameConfig.rightSableActive ? "Activada" : "Desactivada")}");
            }
        }
    }

    // Metodos para activar/desactivar manos en runtime
    public void SetLeftHandActive(bool active)
    {
        if (leftHand != null)
        {
            leftHand.SetActive(active);
            GameConfig.leftSableActive = active;
            Debug.Log($"Mano izquierda: {(active ? "Activada" : "Desactivada")}");
        }
    }

    public void SetRightHandActive(bool active)
    {
        if (rightHand != null)
        {
            rightHand.SetActive(active);
            GameConfig.rightSableActive = active;
            Debug.Log($"Mano derecha: {(active ? "Activada" : "Desactivada")}");
        }
    }
}