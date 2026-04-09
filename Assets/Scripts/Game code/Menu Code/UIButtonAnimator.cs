using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animatie Instellingen")]
    [Tooltip("Hoeveel keer groter the knop wordt als je eroverheen zweeft (Bijv. 1.1)")]
    public float hoverScaleMultiplier = 1.1f;
    [Tooltip("Hoeveel the knop naar binnen krimpt als je hem indrukt (Bijv. 0.95)")]
    public float clickScaleMultiplier = 0.95f;
    [Tooltip("Hoe snel de knop van formaat verandert")]
    public float animationSpeed = 15f;

    private RectTransform _rectTransform;
    private Vector3 _originalScale;
    private Vector3 _targetScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _originalScale = _rectTransform.localScale;
        _targetScale = _originalScale;
    }

    private void Update()
    {
        _rectTransform.localScale = Vector3.Lerp(_rectTransform.localScale, _targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetScale = _originalScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _targetScale = _originalScale * clickScaleMultiplier;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetScale = _originalScale * hoverScaleMultiplier;
    }
}
