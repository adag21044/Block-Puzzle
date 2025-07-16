using UnityEngine;
using System.Collections;

public class ButtonUpdater : MonoBehaviour
{
    [SerializeField] private LevelController levelController;

    private void OnEnable()
    {
        StartCoroutine(DelayedUpdate());
    }

    private IEnumerator DelayedUpdate()
    {
        yield return null; // Bir frame bekle

        if (levelController != null)
        {
            Debug.Log("Level panel opened, updating buttons.");
            levelController.UpdateLevelButtons();
        }
        else
        {
            Debug.LogWarning("LevelController not found!");
        }
    }
}
