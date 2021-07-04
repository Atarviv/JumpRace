using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TapToStartTextScript : MonoBehaviour
{
    bool grow,_gameRunning;
    float counter;
    // Start is called before the first frame update
    void Start()
    {
        grow = true;
        _gameRunning = true;
        StartCoroutine(nameof(GrowText));
    }

    public IEnumerator GrowText()
        {
        while(_gameRunning)
            {
            if (grow)
                GetComponent<Text>().fontSize = Mathf.RoundToInt(GetComponent<Text>().fontSize*1.01f);
            else    
            GetComponent<Text>().fontSize = Mathf.RoundToInt(GetComponent<Text>().fontSize/1.01f);
            counter += 0.02f;
            if(counter>=0.7f)
                {
                counter = 0;
                grow = !grow;
                }
            yield return new WaitForSeconds(0.02f);
            }
        }
}
