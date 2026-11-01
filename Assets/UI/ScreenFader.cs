using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private float fadeInDuration = 0.6f;
    [SerializeField] private float fadeOutDuration = 0.6f;
    [SerializeField] private Ease fadeInEase = Ease.OutQuad;
    [SerializeField] private Ease fadeOutEase = Ease.InQuad;

    private CanvasGroup canvasGroup;
    private Tween fadeTween;
    private bool loadingScene;

    public bool IsFading => fadeTween.isAlive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void Start()
    {
        FadeIn();
    }

    public Tween FadeIn()
    {
        fadeTween.Stop();
        canvasGroup.blocksRaycasts = true;
        fadeTween = Tween.Alpha(canvasGroup, 0f, fadeInDuration, fadeInEase)
            .OnComplete(this, fader => fader.canvasGroup.blocksRaycasts = false);
        return fadeTween;
    }

    public Tween FadeOut()
    {
        fadeTween.Stop();
        canvasGroup.blocksRaycasts = true;
        fadeTween = Tween.Alpha(canvasGroup, 1f, fadeOutDuration, fadeOutEase);
        return fadeTween;
    }

    public void FadeToScene(string sceneName)
    {
        if (loadingScene) return;
        loadingScene = true;
        FadeOut().OnComplete(sceneName, name => SceneManager.LoadScene(name));
    }

    public static void LoadScene(string sceneName)
    {
        if (Instance != null) Instance.FadeToScene(sceneName);
        else SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        fadeTween.Stop();
    }
}
