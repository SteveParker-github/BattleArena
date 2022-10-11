using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject border;
    // Start is called before the first frame update
    void Awake()
    {
        border = transform.GetChild(1).gameObject;
    }

    public void OnSelect(BaseEventData baseEventData)
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            border.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData baseEventData)
    {
        border.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            border.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        border.SetActive(false);
    }
}
