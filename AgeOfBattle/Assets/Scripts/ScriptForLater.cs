using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign a button prefab in the inspector
    private GameObject[] buttons = new GameObject[5];

    void Start()
    {
        CreateButtons();
    }

    void CreateButtons()
    {
        // Create 4 small square buttons
        for (int i = 0; i < 4; i++)
        {
            buttons[i] = Instantiate(buttonPrefab, transform);
            buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50); // Small square size
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-60 + (i * 60), -60); // Adjust position
            buttons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(i));
        }

        // Create a slightly bigger rectangle button
        buttons[4] = Instantiate(buttonPrefab, transform);
        buttons[4].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50); // Rectangle size
        buttons[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120); // Adjust position
        buttons[4].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(4));
    }

    void OnButtonClick(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + " clicked!");
        // Trigger your game event here
    }
}

