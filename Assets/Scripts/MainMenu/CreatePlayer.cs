using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreatePlayer : MonoBehaviour
{
    private Button createButton;
    private TextMeshProUGUI statPoints;
    private TMP_InputField playerNameText;
    private PlayerInfo playerInfo;
    // Start is called before the first frame update
    void Awake()
    {
        Transform buttons = transform.GetChild(0).GetChild(0);
        createButton = buttons.GetChild(0).GetComponent<Button>();
        Button resetButton = buttons.GetChild(1).GetComponent<Button>();
        Button backButton = buttons.GetChild(2).GetComponent<Button>();
        statPoints = transform.GetChild(0).GetChild(1).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        playerNameText = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        createButton.onClick.AddListener(CreateNewPlayer);
        resetButton.onClick.AddListener(ResetPlayer);
        backButton.onClick.AddListener(BackToMainMenu);
        playerNameText.onEndEdit.AddListener(UpdatePlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        bool playerNameStatus = !string.IsNullOrWhiteSpace(playerNameText.text);
        createButton.interactable = int.Parse(statPoints.text) == 0 && playerNameStatus;
    }

    //create the player and save it somewhere
    private void CreateNewPlayer()
    {
        //Check if the user has filled all the details before continuing!
        SceneManager.LoadScene("GameMenuScene");
    }

    //reset the stats on the screen to default
    private void ResetPlayer()
    {
        SceneManager.LoadScene("CreatePlayerScene");
    }

    //Go back to the main menu
    private void BackToMainMenu()
    {
        Destroy(playerInfo.gameObject);
        SceneManager.LoadScene("MenuScene");
    }

    private void UpdatePlayerName(string data)
    {
        print("Update PlayerName!");
        playerInfo.PlayerName = data;
    }
}
