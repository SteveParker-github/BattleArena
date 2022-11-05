using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrizeUI : MonoBehaviour
{
    [SerializeField] private Transform prizesTransform;
    [SerializeField] private Transform winnersTransform;

    private List<TextMeshProUGUI> prizeTexts;
    private List<TextMeshProUGUI> winnerTexts;
    private GameObject mainPanel;
    // Start is called before the first frame update
    void Awake()
    {
        mainPanel = transform.GetChild(0).gameObject;

        prizeTexts = new List<TextMeshProUGUI>();
        winnerTexts = new List<TextMeshProUGUI>();

        for (int i = 0; i < prizesTransform.childCount; i++)
        {
            prizeTexts.Add(prizesTransform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>());
            winnerTexts.Add(winnersTransform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>());
        }

        HideUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowUI(Prize[] prizes, string[] winners)
    {
        for (int i = 0; i < prizes.Length; i++)
        {
            string prize = prizes[i].prizeMoney.ToString() + " money, " + prizes[i].prizeEXP.ToString() + " XP";
            prizeTexts[i].text = prize;
            winnerTexts[i].text = winners[i];
        }

        mainPanel.SetActive(true);
    }

    public void HideUI()
    {
        mainPanel.SetActive(false);
    }
}
