using DG.Tweening;
using UnityEngine;

public class IngredientDragAnimation : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private RectTransform target;

    void Start()
    {
        originalPosition = GetComponent<RectTransform>().position;
    }

    public void PlayTween()
    {
        GetComponent<RectTransform>().DOMove(target.position, 3.0f).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    }

}
