using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : AbstractButton
{
    public GameObject unitButtonManagerPrefab; // Reference to UnitButtonManager prefab

    void Start()
    {
        CreateButtons();
    }

    public override void CreateButtons()
    {
        if (buttons != null && buttons.Length > 0)
        {
            Debug.LogWarning("Buttons already exist. Destroying before creating new ones...");
            DestroyAllButtons();
        }

        buttons = new GameObject[6]; // Initialize the button array
        Debug.Log("ButtonManager CreateButtons called!");
        // Create 5 small square buttons
        for (int i = 0; i < 5; i++)
        {
            buttons[i] = Instantiate(buttonPrefab, transform);
            buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50); // Small square size
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(280 + (i * 60), 200); // Adjust position

            // Assign image if available
            if (buttonImages != null && i < buttonImages.Length && buttonImages[i] != null)
            {
                buttons[i].GetComponent<Image>().sprite = buttonImages[i];
            }

            // Add listener to handle button clicks
            int index = i; // Capture the correct index for the lambda
            buttons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }

        // Create a slightly bigger rectangle button
        buttons[5] = Instantiate(buttonPrefab, transform);
        buttons[5].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50); // Rectangle size
        buttons[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 130); // Adjust position

        // Assign image if available
        if (buttonImages != null && buttonImages.Length > 5 && buttonImages[5] != null)
        {
            buttons[5].GetComponent<Image>().sprite = buttonImages[5];
        }

        // Add listener for the rectangular button
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(5));
    }

    void OnButtonClick(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                Debug.Log("Square Button 1 clicked!");
                DestroyAllButtons();

                // Instantiate UnitButtonManagerPrefab
                GameObject unitManager = Instantiate(unitButtonManagerPrefab, transform.parent);
                // unitManager.SetActive(true); // Ensure it's active
                UnitButtonManager unitButtonManager = unitManager.GetComponentInChildren<UnitButtonManager>();

                if (unitButtonManager != null)
                {
                    unitButtonManager.mainButtonManager = this; // Pass the reference to this ButtonManager
                }

                break;
            case 1:
                Debug.Log("Square Button 2 clicked!");
                break;
            case 2:
                Debug.Log("Square Button 3 clicked!");
                break;
            case 3:
                Debug.Log("Square Button 4 clicked!");
                break;
            case 4:
                Debug.Log("Square Button 5 clicked!");
                break;
            case 5:
                Debug.Log("Rectangle Button clicked!");
                break;
        }
    }
}




