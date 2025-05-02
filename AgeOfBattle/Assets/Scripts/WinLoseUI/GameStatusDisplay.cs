using UnityEngine;
using UnityEngine.UI;

public class GameStatusDisplay : MonoBehaviour
{
    public BaseHealthManager playerBaseHealth;
    public BaseHealthManager enemyBaseHealth;
    private Text statusText;

    void Start()
    {

            // Create a UI Text object dynamically
            GameObject textObj = new GameObject("StatusText");
            textObj.transform.SetParent(transform, false); // Maintain proper scaling

            statusText = textObj.AddComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.rectTransform.sizeDelta = new Vector2(600, 150); // Make it bigger

            // Set anchor to middle of screen
            RectTransform rectTransform = statusText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(0, 0); // Position exactly in center
            statusText.fontSize = 50; // Increase actual text size
    }

    void Update()
    {
        if (playerBaseHealth.getCurrentBaseHealth() <= 0)
        {
            DisplayMessage("You lost!", Color.red);
        }
        else if (enemyBaseHealth.getCurrentBaseHealth() <= 0)
        {
            DisplayMessage("You won!", Color.green);
            Debug.Log($"Enemy Base Health: {enemyBaseHealth.getCurrentBaseHealth()}");
        }
    }

    void DisplayMessage(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
    }
}

