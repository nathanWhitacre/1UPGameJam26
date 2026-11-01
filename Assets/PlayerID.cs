using System.Collections;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [SerializeField] private AudioSource slurpSound;
    [SerializeField] private AudioSource startBell;
    [SerializeField] private AudioSource loopBell;
    [SerializeField] private AudioSource endBell;

    public static PlayerID Instance;

    public float meatSucked = 0f;
    public float timeSuckin = 0f;
    public float timeToSuck = 45f;

    [SerializeField] private string assemblySceneName = "Assembly Scene";
    private bool loadingAssembly = false;

    private bool lowOnTime = false;
    private bool noTime = false;

    private void Awake()
    {
        Instance = this;
        BodyPartInventory.StartRun();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToSuck -= Time.deltaTime;
        timeToSuck = Mathf.Max(timeToSuck, 0f);
        timeSuckin += Time.deltaTime;

        if (timeToSuck <= 6f && !lowOnTime)
        {
            lowOnTime = true;
            startBell.Play();
            StartCoroutine(BellTolls());
        }

        if (timeToSuck <= 1f && !noTime)
        {
            noTime = true;
            endBell.Play();
        }

        if (timeToSuck <= 0f && !loadingAssembly)
        {
            loadingAssembly = true;
            BodyPartInventory.FinishRun();
            ScreenFader.LoadScene(assemblySceneName);
        }
    }

    public void PlaySlurp()
    {
        StartCoroutine(SlurpDelay());
    }

    private IEnumerator SlurpDelay()
    {
        yield return new WaitForSeconds(0.5f);
        slurpSound.PlayOneShot(slurpSound.clip);
    }

    private IEnumerator BellTolls()
    {
        while (true)
        {
            loopBell.PlayOneShot(loopBell.clip);
            yield return new WaitForSeconds(1f);
        }
    }
}
