using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Character;
    public GameObject HOLD;
    // Start is called before the first frame update

    public void startTheGame()
        {
        Character.GetComponent<Rigidbody>().useGravity = true;
        Character.GetComponent<CharacterScript>().enabled = true;
        Character.GetComponent<Animator>().enabled = true;
        Character.GetComponent<LineRenderer>().enabled = true;
        Time.timeScale = 1.3f;
        HOLD.SetActive(true);
        GameObject.Find("TapToStartButton").SetActive(false);
        }
}
