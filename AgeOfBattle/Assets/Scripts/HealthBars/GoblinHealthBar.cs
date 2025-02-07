using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoblinHealthBar : MonoBehaviour
{
    public Image healthFill; // Assign in Inspector (HealthFill Image)
    private AbstractUnit goblinUnit;
    private CanvasGroup canvasGroup; // Used to show/hide on hover
    private Camera mainCamera; // Reference to the main camera
    public Vector3 offset = new Vector3(0, 2f, 0); // Adjust Y position above the Goblin

    private void Start()
    {
        goblinUnit = GetComponentInParent<AbstractUnit>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCamera = Camera.main;

        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup was missing on HealthBar. Adding one automatically.");
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (goblinUnit != null)
        {
            UpdateHealthBar();
        }

        HideHealthBar(); // Hide initially

        // Start the coroutine instead of Update()
        StartCoroutine(UpdateHealthBarRoutine());
    }

    // Coroutine to update every 0.1 seconds instead of Update()
    private IEnumerator UpdateHealthBarRoutine()
    {
        while (true)
        {
            if (goblinUnit != null)
            {
                UpdateHealthBar();
                UpdatePosition();
            }

            DetectMouseHover();

            yield return new WaitForSeconds(0.1f); // Runs every 0.1 seconds
        }
    }

    private void DetectMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == goblinUnit.gameObject) // Ensure it's hitting the goblin itself
            {
                Debug.Log($"{gameObject.name}: Mouse detected via Raycast - Showing health bar.");
                ShowHealthBar();
                return;
            }
        }

        HideHealthBar();
    }

    private void UpdatePosition()
    {
        if (mainCamera != null)
        {
            // Convert goblin's world position to screen position and add offset
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(goblinUnit.transform.position + offset);
            transform.position = screenPosition;
        }
    }

    public void UpdateHealthBar()
    {
        if (goblinUnit != null && healthFill != null)
        {
            float healthPercent = (float)goblinUnit.getHealth() / goblinUnit.getMaxHealth();

            // Shrink the bar from right to left
            healthFill.fillAmount = healthPercent;

            // Keep the color RED (only change size, not color)
            healthFill.color = Color.red;
        }
    }

    public void ShowHealthBar()
    {
        if (canvasGroup != null)
        {
            Debug.Log($"{gameObject.name}: Showing health bar.");
            canvasGroup.alpha = 1; // Make visible
            canvasGroup.interactable = true;  // Allow interactions
            canvasGroup.blocksRaycasts = true; // Allow interactions
        }
    }

    public void HideHealthBar()
    {
        if (canvasGroup != null)
        {
            Debug.Log($"{gameObject.name}: Hiding health bar.");
            canvasGroup.alpha = 0; // Hide
            canvasGroup.blocksRaycasts = false; // Prevent interactions
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log($"{gameObject.name}: Mouse entered, showing health bar.");
        ShowHealthBar();
    }

    private void OnMouseExit()
    {
        Debug.Log($"{gameObject.name}: Mouse exited, hiding health bar.");
        HideHealthBar();
    }
}

