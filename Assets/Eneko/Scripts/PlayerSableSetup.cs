using UnityEngine;

public class PlayerSableSetup : MonoBehaviour
{
    public GameObject leftSable;
    public GameObject rightSable;

    private void Start()
    {
        if (leftSable != null)
            leftSable.SetActive(GameConfig.leftSableActive);

        if (rightSable != null)
            rightSable.SetActive(GameConfig.rightSableActive);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}