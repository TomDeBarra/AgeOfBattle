using UnityEngine;
using UnityEngine.UI;

public class UnitButtonManager : AbstractButton
{
    public ButtonManager mainButtonManager; // Reference to the original ButtonManager instance
    public GameObject goblinPlayerPrefab;

    void Start()
    {
        Debug.Log("UnitButtonManager Start called!");
        CreateButtons();
    }

    public override void CreateButtons()
    {
        Debug.Log("Creating buttons in UnitButtonManager...");

        buttons = new GameObject[6]; // Initialize button array

        // Create 3 small square buttons
        for (int i = 0; i < 3; i++)
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

        // Create the fourth button with a large gap on the x-axis
        buttons[3] = Instantiate(buttonPrefab, transform);
        buttons[3].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50); // Small square size
        buttons[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(480, 200); // Large gap in x-axis

        // Assign image if available
        if (buttonImages != null && buttonImages.Length > 3 && buttonImages[3] != null)
        {
            buttons[3].GetComponent<Image>().sprite = buttonImages[3];
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

        // Add listeners for buttons
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(3));
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(5));
    }

    void OnButtonClick(int buttonIndex)
    {
        Debug.Log("Unit Button " + buttonIndex + " clicked!");

        switch (buttonIndex)
        {
            case 0:
                Debug.Log("Spawning goblin...");
                // Define spawn position (change the Vector3 values as needed)
                Vector3 spawnPosition = new Vector3(-24.53f, 2.384186e-07f, 6.467504f); // Example coordinates
                Quaternion spawnRotation = Quaternion.Euler(0f, 180f, 0f);

                // Instantiate the GoblinPlayer prefab at the specified position
                GameObject goblin = Instantiate(goblinPlayerPrefab, spawnPosition, spawnRotation);

                // Optionally, set the Goblin as a child of a specific parent (e.g., the game world or a unit manager)
                // goblin.transform.parent = someParentTransform;

                break;
                break;
            case 1:
                Debug.Log("Spawning units...");
                break;
            case 2:
                
                break;
            case 3:
                Debug.Log("Returning to main buttons...");
                if (mainButtonManager != null)
                {
                    Destroy(this.transform.root.gameObject);
                    //Destroy(gameObject); // Destroy the UnitButtonManager instance
                    mainButtonManager.CreateButtons(); // Call CreateButtons on ButtonManager
                }

                if (mainButtonManager != null)
                {
                    Debug.Log("Calling CreateButtons on ButtonManager...");
                    Destroy(this.transform.root.gameObject); // Ensure the root prefab is destroyed
                    mainButtonManager.CreateButtons();
                }
                else
                {
                    Debug.LogError("mainButtonManager is null!");
                }

                break;
            case 5:
                Debug.Log("Rectangle Button clicked!");
                break;
        }
    }
}



