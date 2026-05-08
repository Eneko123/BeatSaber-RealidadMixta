using UnityEngine;

public class PlayerHandsSetupMR : MonoBehaviour
{
    [Header("Referencias de las manos del XR Origin Hands")]
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Configuracion")]
    public bool useGameConfig = true;

    private void Start()
    {
        SetupHands();

        // Resetear posicion del jugador
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void SetupHands()
    {
        if (useGameConfig)
        {
            // Configurar segun GameConfig
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

        // Verificar que las manos tienen los componentes necesarios
        VerifyHandComponents(leftHand, "Izquierda");
        VerifyHandComponents(rightHand, "Derecha");
    }

    void VerifyHandComponents(GameObject hand, string handName)
    {
        if (hand == null) return;

        // El XR Origin Hands ya viene con XRDirectInteractor
        // Solo verificamos que este presente
        var interactor = hand.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();

        if (interactor != null)
        {
            Debug.Log($"Mano {handName} configurada correctamente con XRDirectInteractor");
        }
        else
        {
            Debug.LogWarning($"Mano {handName} no tiene XRDirectInteractor. Verifica la configuracion del XR Origin Hands.");
        }
    }

    // Metodo para activar/desactivar manos en runtime
    public void SetLeftHandActive(bool active)
    {
        if (leftHand != null)
        {
            leftHand.SetActive(active);
            GameConfig.leftSableActive = active;
        }
    }

    public void SetRightHandActive(bool active)
    {
        if (rightHand != null)
        {
            rightHand.SetActive(active);
            GameConfig.rightSableActive = active;
        }
    }
}
