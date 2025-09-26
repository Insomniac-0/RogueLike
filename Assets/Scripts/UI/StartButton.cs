using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button button;
    void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SelectGameScene()
    {

        Debug.Log("SelectGameClicked");

    }
}
