using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasScript : MonoBehaviour
    {
    // Start is called before the first frame update
    void Awake()
        {
        GameObject Character = GameObject.Find("Character");
        for (int i = 0; i < Character.transform.GetChildCount(); i++)
            {
            if (Character.transform.GetChild(i).name == "Main Camera")
                GetComponent<Canvas>().worldCamera = Character.transform.GetChild(i).GetComponent<Camera>();

            }

        }

    // Update is called once per frame
    void Update()
        {

        }
    }
