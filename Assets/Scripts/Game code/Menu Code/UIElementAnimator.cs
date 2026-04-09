using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIElementAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animatie Instellingen")]
    [Tooltip("Hoeveel keer groter the knop wordt als je eroverheen zweeft (Bijv. 1.1)")]
    public float hoverScaleMultiplier = 1.1f;
    [Tooltip("Hoeveel the knop naar binnen krimpt als je hem indrukt (Bijv. 0.95)")]
    public float clickScaleMultiplier = 0.95f;
    [Tooltip("Hoe snel de knop van formaat verandert")]
    public float animationSpeed = 15f;

    [Header("Extratjes (Geluiden & Bubbles!)")]
    [Tooltip("Geluid dat afspeelt als de muis erop komt")]
    public AudioClip hoverSound;
    [Tooltip("Geluid dat afspeelt als je de knop indrukt")]
    public AudioClip clickSound;
    [Tooltip("Sleep hier de Prefab van je Bubble Particle System in! Deze spawnt dan los op de knop.")]
    public ParticleSystem bubbelParticles;

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
        PlayUISound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _targetScale = _originalScale * clickScaleMultiplier;
        PlayUISound(clickSound);

        if (bubbelParticles != null)
        {
            ParticleSystem spawnedBubbles = Instantiate(bubbelParticles, transform.position, Quaternion.identity, transform);
            
            ParticleSystemRenderer rnd = spawnedBubbles.GetComponent<ParticleSystemRenderer>();
            if (rnd != null)
            {
                rnd.sortingOrder = 30000;
            }

            spawnedBubbles.Play();
            Destroy(spawnedBubbles.gameObject, spawnedBubbles.main.duration + spawnedBubbles.main.startLifetime.constantMax);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetScale = _originalScale * hoverScaleMultiplier;
    }

    private void PlayUISound(AudioClip clip)
    {
        if (clip == null) return;
        
        GameObject dummySound = new GameObject("UI_Sound_" + clip.name);
        AudioSource source = dummySound.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 0f; 
        source.ignoreListenerPause = true;
        source.Play();
        
        Destroy(dummySound, clip.length + 0.1f);
    }
}
