using TMPro;
using UnityEngine;

public class SweetSweetMeatSweetMeat : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI meatSuckedText;
    [SerializeField] private TextMeshProUGUI timeSuckinText;
    [SerializeField] private TextMeshProUGUI timeToSuckText;
    private PlayerID player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerID.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        meatSuckedText.text = "Meat Sucked:\n" + player.meatSucked.ToString();

        timeSuckinText.text = "Time Suckin:\n" + Mathf.FloorToInt(player.timeSuckin).ToString() + " s";

        timeToSuckText.text = "Time to Suck!\n" + Mathf.FloorToInt(player.timeToSuck).ToString();
    }
}
