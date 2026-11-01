using UnityEngine;

public class SuckySuckyEffectManager : MonoBehaviour
{

    [SerializeField] private AudioSource suckStart;
    [SerializeField] private AudioSource suckLoop;
    [SerializeField] private AudioSource suckEnd;

    private ParticleSystem particles;
    private bool isEmitting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !isEmitting)
        {
            particles.Play();
            isEmitting = true;
            suckStart.Play();
            //suckLoop.Play();
            suckEnd.Stop();
        }
        else if (!Input.GetMouseButton(0) && isEmitting)
        {
            particles.Stop();
            isEmitting = false;
            suckEnd.Play();
            suckStart.Stop();
            suckLoop.Stop();
        }

        if (isEmitting && suckStart.time >= 0.7f)
        {
            suckLoop.Play();
        }
    }
}
