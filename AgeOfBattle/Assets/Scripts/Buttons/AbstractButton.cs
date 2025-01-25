using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractButton : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign a button prefab in the inspector
    public Sprite[] buttonImages; // Array to assign different images to buttons
    protected GameObject[] buttons; // Array of buttons, size depends on implementation
    static protected bool mainButtonsOn = false; // Shared across all implementations of AbstractButton

    // Abstract method to create buttons, to be implemented by derived classes
    public abstract void CreateButtons();

    // Common method to destroy all buttons
    protected void DestroyAllButtons()
    {
        if (buttons != null)
        {
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    Destroy(button);
                }
            }
        }

    }
}