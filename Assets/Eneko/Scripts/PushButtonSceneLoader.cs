using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PushButtonSceneLoader : MonoBehaviour
{
    public string sceneToLoad; // Configurar en el Inspector

    private XRBaseInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        Debug.Log(" Boton presionado! Cargando escena: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnButtonPressed);
        }
    }
}