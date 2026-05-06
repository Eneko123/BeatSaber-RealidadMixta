using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Counter : MonoBehaviour
{
    public int counter;
    public TMP_Text text;
    private int counterMax;
    private bool hasWon = false;

    void Start()
    {
        counterMax = GameConfig.counterMax;
        counter = 0;
        hasWon = false;
        UpdateText();
    }

    void Update()
    {
        WinCondition();
    }

    void WinCondition()
    {
        if (counter >= counterMax && !hasWon)
        {
            hasWon = true;
            text.text = "¡HAS GANADO!";
            StartCoroutine(LoadMenuAfterDelay());
        }
        else if (!hasWon)
        {
            UpdateText();
        }
    }

    IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MenuMain");
    }

    void UpdateText()
    {
        text.text = counter + " / " + counterMax;
    }

    public int GetMax() { return counterMax; }
    public void SetMax(int newMax) { counterMax = newMax; }
}