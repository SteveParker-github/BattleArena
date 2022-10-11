using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameMenu : MonoBehaviour
{
    private Button quickGameButton;
    private Button exitMenuButton;
    private Button exitGameButton;
    private PlayerInfo playerInfo;
    // private Button button;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);

        Transform buttons = transform.GetChild(0).GetChild(1);
        quickGameButton = buttons.GetChild(0).GetComponent<Button>();
        exitMenuButton = buttons.GetChild(1).GetComponent<Button>();
        exitGameButton = buttons.GetChild(2).GetComponent<Button>();

        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        quickGameButton.GetComponent<Button>().onClick.AddListener(QuickGameOnClick);
        exitMenuButton.GetComponent<Button>().onClick.AddListener(ExitMenuOnClick);
        exitGameButton.GetComponent<Button>().onClick.AddListener(ExitGameOnClick);
    }

    private void QuickGameOnClick()
    {
        SceneManager.LoadScene("BattleScene");
    }

    private void ExitMenuOnClick()
    {
        playerInfo.SaveGame();
        Destroy(playerInfo.gameObject);
        SceneManager.LoadScene("MenuScene");
    }

    private void ExitGameOnClick()
    {
        playerInfo.SaveGame();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
