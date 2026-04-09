using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UISceneAnimator : MonoBehaviour
{
    [Header("Animatie Instellingen")]
    public float duration = 0.35f;
    public Vector3 overshootScale = new Vector3(1.05f, 1.05f, 1.05f);

    private RectTransform _rectTransform;
    private Vector3 _originalScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(BounceAnimation());
    }

    private IEnumerator BounceAnimation()
    {
        Vector3 startScale = _originalScale * 0.01f;
        _rectTransform.localScale = startScale;

        float time = 0f;
        float overshootTime = duration * 0.65f;

        while (time < overshootTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / overshootTime;
            t = t * (2f - t); 
            _rectTransform.localScale = Vector3.Lerp(startScale, _originalScale * 1.05f, t);
            yield return null;
        }

        time = 0f;
        float returnTime = duration * 0.35f;

        while (time < returnTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / returnTime;
            t = t * t * (3f - 2f * t); 
            _rectTransform.localScale = Vector3.Lerp(_originalScale * 1.05f, _originalScale, t);
            yield return null;
        }

        _rectTransform.localScale = _originalScale;
    }
}
