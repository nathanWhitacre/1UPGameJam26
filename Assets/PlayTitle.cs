using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayTitle : MonoBehaviour
{
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private Vector3 playScale;
    [SerializeField] private AudioSource playSound;
    [SerializeField] private float playDelay = 0.1f;
    [SerializeField] private GameObject black;

    [SerializeField] private float delayMultiplier = 1.5f;
    [SerializeField] private float delayMin = 0.3f;

    private bool playing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playing = false;
        black.SetActive(false);
        playSound.volume = 0f;
        playSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playing) {
            playing = true;
            StartCoroutine(PlayCoroutine());
        }
    }

    private IEnumerator PlayCoroutine()
    {
        //jumpscare (sorry andrew)
        playSound.Stop();
        playSound.volume = 1f;

        yield return new WaitForSeconds((Random.value * delayMultiplier) + delayMin);

        playSound.Play();
        transform.position = playPosition;
        transform.localScale = playScale;

        yield return new WaitForSeconds(playDelay);

        //playSound.Stop();
        black.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        playSound.Stop();
        SceneManager.LoadScene("SampleScene");
    }
}
