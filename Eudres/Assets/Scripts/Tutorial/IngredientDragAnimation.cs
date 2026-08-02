using DG.Tweening;
using UnityEngine;

public class IngredientDragAnimation : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Transform target;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void PlayTween()
    {
        transform.DOMove(target.position, 3.0f).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    }

}
