using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    private void Start()
    {
        // Example usage of DOTween for a simple animation
        transform.DOMove(new Vector3(0, 5, 0), 2f).SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("Movement completed!"));
    }
}