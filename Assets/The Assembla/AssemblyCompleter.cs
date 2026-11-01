using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class AssemblyCompleter : MonoBehaviour
{
    [SerializeField] private Button completeButton;
    [SerializeField] private GameObject tray;
    [SerializeField] private Transform mirror;
    [SerializeField] private Transform skeleton;
    [SerializeField] private Vector2 mirrorOffset = Vector2.zero;
    [SerializeField] private float mirrorDuration = 1f;
    [SerializeField] private Ease mirrorEase = Ease.OutCubic;
    [SerializeField] private GameObject playAgain;
    [SerializeField] private GameObject quit;
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private TextMeshProUGUI meatSuckedText;
    [SerializeField] private TextMeshProUGUI peakMeatText;

    private Tween mirrorTween;
    private bool completed;

    private void Awake()
    {
        if (completeButton != null) completeButton.onClick.AddListener(Complete);
        if (playAgain != null) playAgain.GetComponent<Button>().onClick.AddListener(PlayAgain);
        if (quit != null) quit.GetComponent<Button>().onClick.AddListener(Quit);
    }

    public void Complete()
    {
        if (completed) return;
        completed = true;

        if (completeButton != null) completeButton.interactable = false;
        if (tray != null) tray.SetActive(false);

        if (mirror == null || skeleton == null) return;

        Vector3 target = GetSkeletonCenter() + (Vector3)mirrorOffset;
        target.z = mirror.position.z;
        mirrorTween = Tween.Position(mirror, target, mirrorDuration, mirrorEase);
        StartCoroutine(EnableButtons(mirrorDuration));
    }

    IEnumerator EnableButtons(float duration)
    {
        completeButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(duration);

        particle.Play();

        playAgain.SetActive(true);
        quit.SetActive(true);

        meatSuckedText.text = "Meat Sucked:\n" + PlayerPrefs.GetFloat("LatestScore", 0f);
        meatSuckedText.gameObject.SetActive(true);
        peakMeatText.text = "Biggest Suck:\n" + PlayerPrefs.GetFloat("HighScore", 0f);
        peakMeatText.gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        ScreenFader.Instance.FadeToScene("SampleScene");
    }

    public void Quit()
    {
        ScreenFader.Instance.FadeToScene("TitleScene");
    }

    private Vector3 GetSkeletonCenter()
    {
        SpriteRenderer[] renderers = skeleton.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) return skeleton.position;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
        return bounds.center;
    }

    private void OnDestroy()
    {
        if (completeButton != null) completeButton.onClick.RemoveListener(Complete);
        mirrorTween.Stop();
    }
}
