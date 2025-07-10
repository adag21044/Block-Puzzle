using UnityEngine;

public class PieceView : MonoBehaviour
{
    public void SetColor(Color color)
    {
        foreach (Transform block in transform)
        {
            var sr = block.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = color;
                Debug.Log($"Set color {color} for block {block.name} in piece {gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"⚠️ SpriteRenderer missing in {block.name}");
            }
        }
    }
}