using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    // [SerializeField] private List<Button> buttons;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);

        buttons[0].GetComponent<Button>().onClick.AddListener(StartGameOnClick);
        buttons[1].GetComponent<Button>().onClick.AddListener(LoadGameOnClick);
        buttons[2].GetComponent<Button>().onClick.AddListener(ExitGameOnClick);

        WaterMark waterMark = GameObject.FindObjectOfType<WaterMark>();

        if (waterMark == null)
        {
            Instantiate(Resources.Load<GameObject>("UI/WaterMark"));
        }

        PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        if (playerInfo == null)
        {
            Instantiate(Resources.Load<GameObject>("PlayerInfo/PlayerInfo"));
        }

        string path = Application.persistentDataPath + "/SaveGame/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] dirs = dir.GetDirectories();

        buttons[1].GetComponent<Button>().interactable = dirs.Length > 0;
    }

    private void StartGameOnClick()
    {
        SceneManager.LoadScene("CreatePlayerScene");
    }

    private void LoadGameOnClick()
    {
        SceneManager.LoadScene("LoadGameScene");
    }

    private void ExitGameOnClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
