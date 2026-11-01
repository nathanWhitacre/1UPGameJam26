using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyCompleter : MonoBehaviour
{
    [SerializeField] private Button completeButton;
    [SerializeField] private GameObject tray;
    [SerializeField] private Transform mirror;
    [SerializeField] private Transform skeleton;
    [SerializeField] private Vector2 mirrorOffset = Vector2.zero;
    [SerializeField] private float mirrorDuration = 1f;
    [SerializeField] private Ease mirrorEase = Ease.OutCubic;

    private Tween mirrorTween;
    private bool completed;

    private void Awake()
    {
        if (completeButton != null) completeButton.onClick.AddListener(Complete);
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
