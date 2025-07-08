using UnityEngine;

public class InputLocker : MonoBehaviour
{
    public static InputLocker Instance { get; private set; }
    private bool _isInputLocked = true; // Private flag
    public bool IsInputLocked => _isInputLocked; // Public getter

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }   

    public void LockInput()
    {
        _isInputLocked = true;
        Debug.Log("Input locked");
    }

    public void UnlockInput()
    {
        _isInputLocked = false;
        Debug.Log("Input unlocked");
    }
}