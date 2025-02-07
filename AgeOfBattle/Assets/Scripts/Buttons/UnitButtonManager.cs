using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class UnitButtonManager : AbstractButton
{
    private Sprite batteringRamButtonSprite;
    public Sprite goblinButtonSprite; // Assign this in the Inspector
    public ButtonManager mainButtonManager; // Reference to the original ButtonManager instance
    public GameObject goblinPlayerPrefab;

    private float[] buttonCooldowns = { 2f, 10f, 20f }; // Cooldowns for each button
    private bool[] isButtonCoolingDown; // Tracks if a button is on cooldown

    void Awake()
    {
        Debug.Log("Attempting to load GoblinPlayer prefab dynamically using Addressables...");

        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Units/GoblinPlayer.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                goblinPlayerPrefab = handle.Result;
                Debug.Log("GoblinPlayer prefab successfully assigned dynamically.");
            }
            else
            {
                Debug.LogError("Failed to load GoblinPlayer prefab using Addressables!");
            }
        };

        // Load the Goblin texture
        goblinButtonSprite = Resources.Load<Sprite>("Sprites/goblin");
        if (goblinButtonSprite == null)
        {
            Debug.LogError("Failed to load Goblin texture from Resources!");
        }
        else
        {
            Debug.Log("Goblin sprite successfully loaded from Resources.");
        }
        batteringRamButtonSprite = Resources.Load<Sprite>("Sprites/batteringram");
        if (batteringRamButtonSprite == null)
            Debug.LogError("Failed to load Battering Ram sprite from Resources!");
        else
            Debug.Log("Battering Ram sprite successfully loaded.");

    }

    void Start()
    {
        Debug.Log("UnitButtonManager Start called!");
        isButtonCoolingDown = new bool[6]; // Initialize cooldown tracker array
        CreateButtons();
    }

    public override void CreateButtons()
    {
        Debug.Log("Creating buttons in UnitButtonManager...");

        buttons = new GameObject[6]; // Initialize button array

        for (int i = 0; i < 3; i++)
        {
            buttons[i] = Instantiate(buttonPrefab, transform);
            buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(280 + (i * 60), 200);

            if (i == 0 && goblinButtonSprite != null)
            {
                buttons[i].GetComponent<Image>().sprite = goblinButtonSprite; // Assign goblin texture to button 0
            }
            else if (i == 1 && batteringRamButtonSprite != null)
            {
                buttons[i].GetComponent<Image>().sprite = batteringRamButtonSprite;
            }
            else if (buttonImages != null && i < buttonImages.Length && buttonImages[i] != null)
            {
                buttons[i].GetComponent<Image>().sprite = buttonImages[i];
            }
            else if (buttonImages != null && i < buttonImages.Length && buttonImages[i] != null)
            {
                buttons[i].GetComponent<Image>().sprite = buttonImages[i];
            }

            int index = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }

        buttons[3] = Instantiate(buttonPrefab, transform);
        buttons[3].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        buttons[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(480, 200);

        if (buttonImages != null && buttonImages.Length > 3 && buttonImages[3] != null)
        {
            buttons[3].GetComponent<Image>().sprite = buttonImages[3];
        }

        buttons[5] = Instantiate(buttonPrefab, transform);
        buttons[5].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50);
        buttons[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 130);

        if (buttonImages != null && buttonImages.Length > 5 && buttonImages[5] != null)
        {
            buttons[5].GetComponent<Image>().sprite = buttonImages[5];
        }

        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(3));
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(5));
    }

    void OnButtonClick(int clickedIndex)
    {
        if (isButtonCoolingDown[clickedIndex]) return; // Prevent pressing during cooldown

        Debug.Log("Unit Button " + clickedIndex + " clicked!");

        switch (clickedIndex)
        {
            case 0:
                SpawnGoblin();
                break;
            case 1:
                Debug.Log("Spawning units...");
                break;
            case 3:
                ReturnToMainMenu();
                break;
            case 5:
                Debug.Log("Rectangle Button clicked!");
                break;
        }

        // Start cooldown for ALL buttons when any is clicked
        StartCoroutine(StartCooldownForAllButtons());
    }

    private void SpawnGoblin()
    {
        Debug.Log("Spawning goblin...");
        Vector3 spawnPosition = new Vector3(-24.53f, -0.015f, 6.467504f);
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 0f);
        if (goblinPlayerPrefab == null)
        {
            Debug.LogError("GoblinPlayer prefab is not assigned in the Inspector!");
            return;
        }
        Instantiate(goblinPlayerPrefab, spawnPosition, spawnRotation);
        Debug.Log("GoblinPlayer successfully spawned!");
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to main buttons...");
        if (mainButtonManager != null)
        {
            Destroy(this.transform.root.gameObject);
            mainButtonManager.CreateButtons();
        }
        else
        {
            Debug.LogError("mainButtonManager is null!");
        }
    }

    private IEnumerator StartCooldownForAllButtons()
    {
        // Mark all buttons as cooling down
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                isButtonCoolingDown[i] = true;
                StartCoroutine(ButtonCooldownRoutine(i, buttonCooldowns[i]));
            }
        }
        yield return null;
    }

    private IEnumerator ButtonCooldownRoutine(int buttonIndex, float cooldownTime)
    {
        Button button = buttons[buttonIndex].GetComponent<Button>();
        Image buttonImage = buttons[buttonIndex].GetComponent<Image>();

        float elapsedTime = 0f;
        Color originalColor = buttonImage.color;

        while (elapsedTime < cooldownTime)
        {
            float fillAmount = elapsedTime / cooldownTime;

            // Darken the button gradually from right to left
            buttonImage.color = new Color(originalColor.r * (1 - fillAmount), originalColor.g * (1 - fillAmount), originalColor.b * (1 - fillAmount), originalColor.a);

            button.interactable = false;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.interactable = true;
        buttonImage.color = originalColor; // Reset to original color
        isButtonCoolingDown[buttonIndex] = false;
    }
}




