using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadEasyLevel()
    {
        SceneManager.LoadScene("Easy");
    }

    public void LoadHardLevel()
    {
        SceneManager.LoadScene("Hard");
    }
}