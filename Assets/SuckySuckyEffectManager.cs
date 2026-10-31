using UnityEngine;

public class SuckySuckyEffectManager : MonoBehaviour
{

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
        }
        else if (!Input.GetMouseButton(0) && isEmitting)
        {
            particles.Stop();
            isEmitting = false;
        }
    }
}
