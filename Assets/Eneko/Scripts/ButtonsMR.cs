using UnityEngine;
using TMPro;

public class ButtonsMR : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    Counter counter;
    public TMP_Text countText;
    public TMP_Text leftText;
    public TMP_Text rightText;

    private void Start()
    {
        UpdateHandStatusText();
    }

    void UpdateHandStatusText()
    {
        if (GameConfig.leftSableActive)
            leftText.text = "Activada";
        else
            leftText.text = "Desactivada";

        if (GameConfig.rightSableActive)
            rightText.text = "Activada";
        else
            rightText.text = "Desactivada";
    }

    public void ActiveDesactiveLeftHand()
    {
        GameConfig.leftSableActive = !GameConfig.leftSableActive;
        UpdateHandStatusText();
    }

    public void ActiveDesactiveRightHand()
    {
        GameConfig.rightSableActive = !GameConfig.rightSableActive;
        UpdateHandStatusText();
    }

    public void UpMax()
    {
        GameConfig.counterMax++;
    }

    public void DownMax()
    {
        if (GameConfig.counterMax > 1)
            GameConfig.counterMax--;
    }

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        countText.text = GameConfig.counterMax.ToString();
    }
}
