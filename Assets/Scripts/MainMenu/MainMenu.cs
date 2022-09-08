using System.Collections;
using System.Collections.Generic;
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
        buttons[1].GetComponent<Button>().onClick.AddListener(ExitGameOnClick);
    }

    private void StartGameOnClick()
    {
        SceneManager.LoadScene("BattleScene");
    }

    private void ExitGameOnClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
