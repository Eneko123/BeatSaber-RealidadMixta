using UnityEngine;
using TMPro;

public class Buttons : MonoBehaviour
{
    public GameObject leftSable;
    public GameObject rigthSable;
    Counter counter;
    public TMP_Text countText;
    public TMP_Text leftText;
    public TMP_Text rigthText;

    private void Start()
    {
        if (GameConfig.leftSableActive)
            leftText.text = "Yes";
        else
            leftText.text = "No";
        if (GameConfig.rightSableActive)
            rigthText.text = "Yes";
        else
            rigthText.text = "No";
    }

    public void ActiveDesactiveLeftSable()
    {
        GameConfig.leftSableActive = !GameConfig.leftSableActive;
        if (GameConfig.leftSableActive)
            leftText.text = "Yes";
        else
            leftText.text = "No"; 
    }

    public void ActiveDesactiveRigthSable()
    {
        GameConfig.rightSableActive = !GameConfig.rightSableActive;
        if (GameConfig.rightSableActive)
            rigthText.text = "Yes";
        else
            rigthText.text = "No";
    }

    public void UpMax() { GameConfig.counterMax++; }
    public void DownMax() { 
        if (GameConfig.counterMax > 1)
            GameConfig.counterMax--; }

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        countText.text = GameConfig.counterMax.ToString();
    }
}
