using UnityEngine;
using TMPro; // Required for TextMeshPro
using System.Collections;

public class BaseHealthDisplay : MonoBehaviour
{
    public BaseHealthManager baseHealthManager; // Reference to the base health manager
    public TextMeshProUGUI healthText; // Reference to TextMeshPro UI element

    private void Start()
    {
        // Start the coroutine to update health display every 0.1 seconds
        StartCoroutine(UpdateBaseHealthRoutine());
    }

    private IEnumerator UpdateBaseHealthRoutine()
    {
        while (true)
        {
            if (baseHealthManager != null && healthText != null)
            {
                // Get current health, ensuring it does not go below 0
                int displayedHealth = Mathf.Max(0, baseHealthManager.getCurrentBaseHealth());

                // Update the text display
                healthText.text = displayedHealth.ToString();

                // Turn red when health reaches 0
                if (displayedHealth == 0)
                {
                    healthText.color = Color.red;
                }
                else
                {
                    healthText.color = Color.black;
                }
            }

            yield return new WaitForSeconds(0.1f); // Runs every 0.1 seconds
        }
    }
}


