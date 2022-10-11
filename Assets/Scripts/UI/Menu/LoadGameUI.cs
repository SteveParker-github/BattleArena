using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadGameUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    private Button LoadButton;
    private Button deleteButton;
    private Transform contentTransform;
    private string selectedFolderName;
    private int selectedIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadButton = buttons[0].GetComponent<Button>();
        deleteButton = buttons[1].GetComponent<Button>();
        LoadButton.onClick.AddListener(LoadGameOnClick);
        deleteButton.onClick.AddListener(DeleteSaveOnClick);
        buttons[2].GetComponent<Button>().onClick.AddListener(BackMenuOnClick);

        contentTransform = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0);
        LoadSaves();
    }

    // Update is called once per frame
    void Update()
    {
        bool folderSelected = !string.IsNullOrEmpty(selectedFolderName);
        LoadButton.interactable = folderSelected;
        deleteButton.interactable = folderSelected;
    }

    private void LoadSaves()
    {
        string path = Application.persistentDataPath + "/SaveGame/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] dirs = dir.GetDirectories();

        for (int i = 0; i < dirs.Length; i++)
        {
            FileInfo[] file = dirs[i].GetFiles("Save.json");

            if (file.Length < 1)
            {
                Debug.Log("File not found!");
                continue;
            }

            string folderName = dirs[i].Name;
            int index = i;
            GameObject saveFileObject = Instantiate(Resources.Load<GameObject>("UI/SaveFileUI"), contentTransform);
            saveFileObject.name = folderName;
            saveFileObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = folderName;
            saveFileObject.GetComponent<Button>().onClick.AddListener(() => SaveFileSelectOnClick(folderName, index));
        }
    }

    private void SaveFileSelectOnClick(string folderName, int index)
    {
        contentTransform.GetChild(selectedIndex).GetComponent<Image>().color = Color.white;
        contentTransform.GetChild(index).GetComponent<Image>().color = Color.yellow;
        selectedIndex = index;
        selectedFolderName = folderName;
    }

    private void LoadGameOnClick()
    {
        PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        playerInfo.LoadGame(selectedFolderName);
        SceneManager.LoadScene("GameMenuScene");
    }

    private void DeleteSaveOnClick()
    {
        string path = Application.persistentDataPath + "/SaveGame/" + selectedFolderName;
        Directory.Delete(path, true);
        SceneManager.LoadScene("LoadGameScene");
    }

    private void BackMenuOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
