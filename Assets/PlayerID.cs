using System.Collections;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [SerializeField] private AudioSource slurpSound;

    public static PlayerID Instance;

    public float meatSucked = 0f;
    public float timeSuckin = 0f;
    public float timeToSuck = 45f;

    [SerializeField] private string assemblySceneName = "Assembly Scene";
    private bool loadingAssembly = false;

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
        timeSuckin += Time.deltaTime;

        if (timeToSuck <= 0f && !loadingAssembly)
        {
            loadingAssembly = true;
            BodyPartInventory.FinishRun();
            ScreenFader.LoadScene(assemblySceneName);
        }
    }

    public void PlaySlurp()
    {
        //slurpSound.PlayOneShot(slurpSound.clip);
        StartCoroutine(SlurpDelay());
    }

    private IEnumerator SlurpDelay()
    {
        yield return new WaitForSeconds(0.5f);
        slurpSound.PlayOneShot(slurpSound.clip);
    }
}
