using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class UnitButtonManager : AbstractButton
{
    private Sprite batteringRamButtonSprite;
    public Sprite goblinButtonSprite;
    public Sprite rectangleButtonSprite;
    public Sprite giantButtonSprite;

    public ButtonManager mainButtonManager;

    public GameObject goblinPlayerPrefab;
    public GameObject batteringRamPrefab;
    public GameObject giantPrefab;

    private float[] buttonCooldowns = { 2f, 10f, 20f };
    private bool[] isButtonCoolingDown;

    void Awake()
    {
        Debug.Log("Attempting to load GoblinPlayer,BatteringRam, Giant prefabs dynamically using Addressables...");

        // Load Goblin prefab
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

        // Load Battering Ram prefab
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Units/RamPlayer.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                batteringRamPrefab = handle.Result;
                Debug.Log("BatteringRam prefab successfully assigned dynamically.");
            }
            else
            {
                Debug.LogError("Failed to load BatteringRam prefab using Addressables!");
            }
        };

        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Units/GiantDummyPlayer.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                giantPrefab = handle.Result;
                Debug.Log("Giant prefab successfully assigned dynamically.");
            }
            else
            {
                Debug.LogError("Failed to load Giant prefab using Addressables!");
            }
        };

        // Load Button Sprites
        goblinButtonSprite = Resources.Load<Sprite>("Sprites/goblin");
        batteringRamButtonSprite = Resources.Load<Sprite>("Sprites/batteringram");
        rectangleButtonSprite = Resources.Load<Sprite>("Sprites/meteors");
        giantButtonSprite = Resources.Load<Sprite>("Sprites/giant");

        if (goblinButtonSprite == null)
            Debug.LogError("Failed to load Goblin button texture from Resources!");

        if (batteringRamButtonSprite == null)
            Debug.LogError("Failed to load Battering Ram button texture from Resources!");

        if (rectangleButtonSprite == null)
            Debug.LogError("Failed to load meteors.png for rectangle button!");

        if (giantButtonSprite == null)
            Debug.LogError("Failed to load Giant button texture from Resources!");
    }

    void Start()
    {
        Debug.Log("UnitButtonManager Start called!");
        isButtonCoolingDown = new bool[6];
        CreateButtons();
       // RestoreCooldowns();
    }

    public override void CreateButtons()
    {
        Debug.Log("Creating buttons in UnitButtonManager...");

        buttons = new GameObject[6];

        for (int i = 0; i < 3; i++)
        {
            buttons[i] = Instantiate(buttonPrefab, transform);
            buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(280 + (i * 60), 200);

            if (i == 0 && goblinButtonSprite != null)
            {
                buttons[i].GetComponent<Image>().sprite = goblinButtonSprite;
            }
            else if (i == 1 && batteringRamButtonSprite != null)
            {
                buttons[i].GetComponent<Image>().sprite = batteringRamButtonSprite;
            }
            else if (i == 2 && giantButtonSprite != null)
            {
                buttons[i].GetComponent<Image>().sprite = giantButtonSprite;
            }
            int index = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }

        buttons[3] = Instantiate(buttonPrefab, transform);
        buttons[3].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        buttons[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(480, 200);
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(3));

        buttons[5] = Instantiate(buttonPrefab, transform);
        buttons[5].GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50);
        buttons[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 130);
        if (rectangleButtonSprite != null)
        {
            buttons[5].GetComponent<Image>().sprite = rectangleButtonSprite;
        }
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(5));
        ResetAllCooldowns();
      //  RestoreCooldowns();
    }

    void OnButtonClick(int clickedIndex)
    {
        if (clickedIndex != 3 && IsButtonOnCooldown(clickedIndex)) return;

        Debug.Log("Unit Button " + clickedIndex + " clicked!");

        switch (clickedIndex)
        {
            case 0:
                SpawnGoblin();
                StartCooldown(clickedIndex);
                break;
            case 1:
                SpawnBatteringRam();
                StartCooldown(clickedIndex);
                break;
            case 2:
                SpawnGiant();
                StartCooldown(clickedIndex);
                break;
            case 3:
                ReturnToMainMenu();
                break;
        }
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

    private void SpawnBatteringRam()
    {
        Debug.Log("Spawning battering ram...");
        Vector3 spawnPosition = new Vector3(-20f, -0.015f, 6.467504f);
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 0f);
        if (batteringRamPrefab == null)
        {
            Debug.LogError("BatteringRam prefab is not assigned in the Inspector!");
            return;
        }
        Instantiate(batteringRamPrefab, spawnPosition, spawnRotation);
        Debug.Log("Battering Ram successfully spawned!");
    }

    private void SpawnGiant()
    {
        Debug.Log("Spawning giant...");
        Vector3 spawnPosition = new Vector3(-20f, -0.015f, 6.467504f);
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 0f);
        if (batteringRamPrefab == null)
        {
            Debug.LogError("Giant prefab is not assigned in the Inspector!");
            return;
        }
        GameObject instantiatedGiant = Instantiate(giantPrefab, spawnPosition, spawnRotation);
        instantiatedGiant.GetComponent<GiantUnit>().setPlayerControlled(true);
        instantiatedGiant.GetComponent<GiantUnit>().setDirection(1);
        Debug.Log("Giant successfully spawned!");
    }

    private void StartCooldown(int buttonIndex)
    {
        float cooldownEndTime = Time.time + buttonCooldowns[buttonIndex];
        PlayerPrefs.SetFloat("ButtonCooldown_" + buttonIndex, cooldownEndTime);
        PlayerPrefs.Save();

        StartCoroutine(ButtonCooldownRoutine(buttonIndex, cooldownEndTime));
    }

    void ResetAllCooldowns()
    {
        float cooldownEndTime = Time.time;
        for (int i = 0; i < buttonCooldowns.Length; i++)
        {
            PlayerPrefs.SetFloat("ButtonCooldown_" + i, cooldownEndTime);
            PlayerPrefs.Save();

            StartCoroutine(ButtonCooldownRoutine(i, cooldownEndTime));
        }

    }

    private bool IsButtonOnCooldown(int buttonIndex)
    {
        if (!PlayerPrefs.HasKey("ButtonCooldown_" + buttonIndex)) return false;

        float cooldownEndTime = PlayerPrefs.GetFloat("ButtonCooldown_" + buttonIndex);

        // Fix: Prevent eternal cooldowns by removing expired data
        if (Time.time >= cooldownEndTime)
        {
            PlayerPrefs.DeleteKey("ButtonCooldown_" + buttonIndex);
            return false;
        }

        return true;
    }

    private IEnumerator ButtonCooldownRoutine(int buttonIndex, float cooldownEndTime)
    {
        Button button = buttons[buttonIndex].GetComponent<Button>();
        Image buttonImage = buttons[buttonIndex].GetComponent<Image>();

        while (Time.time < cooldownEndTime)
        {
            float remainingTime = cooldownEndTime - Time.time;
            float fillAmount = remainingTime / buttonCooldowns[buttonIndex];

            buttonImage.color = new Color(1f, 1f, 1f, 0.5f + (fillAmount * 0.5f));
            button.interactable = false;
            yield return null;
        }

        button.interactable = true;
        buttonImage.color = Color.white;
        PlayerPrefs.DeleteKey("ButtonCooldown_" + buttonIndex);
    }

    private void RestoreCooldowns()
    {
        for (int i = 0; i < buttonCooldowns.Length; i++)
        {
            if (PlayerPrefs.HasKey("ButtonCooldown_" + i))
            {
                float cooldownEndTime = PlayerPrefs.GetFloat("ButtonCooldown_" + i);

                if (Time.time < cooldownEndTime)
                {
                    StartCoroutine(ButtonCooldownRoutine(i, cooldownEndTime));
                }
                else
                {
                    // Fix: Clear out expired cooldowns
                    PlayerPrefs.DeleteKey("ButtonCooldown_" + i);
                }
            }
        }
    }
}










