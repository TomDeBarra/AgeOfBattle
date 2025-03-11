using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ButtonManager : AbstractButton
{
    public GameObject unitButtonManagerPrefab; // Reference to UnitButtonManager prefab

    private GameObject meteorPrefab; // Assign in Inspector
    private float minX = -23f; // Minimum X spawn position
    private float maxX = 23f;  // Maximum X spawn position
    private float spawnY = 15f; // Height at which meteors spawn
    private float spawnZ = 6.4f;  // Fixed Z position for meteors

    void Start()
    {
        CreateButtons();
    }

    void Awake()
    {
        Debug.Log("Attempting to load meteorPrefab dynamically using Addressables...");

        // Load Goblin prefab
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Units/MeteorPrefab.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                meteorPrefab = handle.Result;
                Debug.Log("GoblinPlayer prefab successfully assigned dynamically.");
            }
            else
            {
                Debug.LogError("Failed to load GoblinPlayer prefab using Addressables!");
            }
        };
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
                Debug.Log("Rectangle Button clicked! Activating meteor rain!");
                StartCoroutine(RainMeteors());
                break;
        }
    }

    private IEnumerator RainMeteors()
    {
        float duration = 6f; // Meteor storm duration
        float timeElapsed = 0f;
        int meteorCount = 0;
        int totalMeteors = 20; // At least 20 meteors will fall

        while (timeElapsed < duration && meteorCount < totalMeteors)
        {
            float xPosition = UnityEngine.Random.Range(minX, maxX);
            Vector3 spawnPosition = new Vector3(xPosition, spawnY, spawnZ);

            // Spawn the meteor
            Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            meteorCount++;

            // Spawn intervals between 0.1s to 0.5s randomly
            float delay = UnityEngine.Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(delay);

            timeElapsed += delay;
        }
    }
}




