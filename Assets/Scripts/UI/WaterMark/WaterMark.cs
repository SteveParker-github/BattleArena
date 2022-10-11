using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMark : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
