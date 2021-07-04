using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
    {
    public float Bottom_Platform_Distance_Min;
    public GameObject Platform, bottomPlatform, GreenBoulderPrefab;
    public float Platform_distance, BottomPlatformDistance;
    Vector3 lastpos;
    GameObject Bottom;
    Quaternion lastrotation;
    void Start()
        {
        lastrotation = transform.rotation;
        Bottom = GameObject.Find("Bottom");
        lastpos = new Vector3(0, -Platform_distance / 2, 0);
        FloatingSpawn(30);
        }
    public void FloatingSpawn(int amount)
        {
        GameObject.Find("Character").GetComponent<CharacterScript>().numOfPlatforms = amount;
        Platform[] platforms = new Platform[amount];
        for (int i = 0; i < amount; i++)
            {
            if (i < (amount - 1))
                {
                GameObject LastPlatform = Instantiate(Platform, lastpos, lastrotation);
                if (i == 0)
                    lastpos += new Vector3(0, -Platform_distance, Platform_distance * 1.5f);
                else
                    {
                    int deg = 0;
                    if (Random.Range(1, 11) <= 7)
                        deg = Random.Range(5, 45);
                    else
                        deg = Random.Range(-15, 5);
                    LastPlatform.transform.Rotate(0,deg,0);
                    lastpos += LastPlatform.transform.forward * Platform_distance - new Vector3(0, Platform_distance*0.2f, 0);
                    }
                LastPlatform.GetComponent<PlatformScript>().Currentplatform.Index = i;
                platforms[i] = LastPlatform.GetComponent<PlatformScript>().Currentplatform;
                if (i > 0)
                    platforms[i - 1].next = platforms[i];
                lastrotation = LastPlatform.transform.rotation;
                }
            else
                {
                GameObject finalPlatform = Instantiate(GreenBoulderPrefab, lastpos, transform.rotation);
                finalPlatform.GetComponent<PlatformScript>().lastPlatform = true;
                platforms[i] = finalPlatform.GetComponent<PlatformScript>().Currentplatform;
                if (i > 0)
                    platforms[i - 1].next = platforms[i];
                }
            }
        for(int i = 0; i < platforms.Length - 1; i++)
            {
            platforms[i].gameObject.GetComponent<PlatformScript>().IndexText.text = (amount - (platforms[i].Index + 1)).ToString();
            platforms[i].gameObject.GetComponent<PlatformScript>().StartCoroutine("platformColorandMovement");
            }
        Bottom.transform.position = new Vector3(0, platforms[amount - 1].gameObject.transform.position.y - 1, 0);
        GameObject.Find("Character").GetComponent<CharacterScript>().BottomPos = Bottom.transform.position.y;
        BottomSpawn(platforms, Bottom.transform.position.y + 1, amount * 2);
        drawLines(platforms);
        }
    public void drawLines(Platform[] platforms)
        {
        LineRenderer line = GetComponent<LineRenderer>();
        line.positionCount = platforms.Length - 1;
                line.startColor = Color.white;
                line.endColor = Color.white;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
        for (int i = 0; i < platforms.Length-1; i++)
            {
                line.SetPosition(i, platforms[i].gameObject.transform.position - new Vector3(0, 1, 0));
            }
        }
    public void BottomSpawn(Platform[] platforms, float Ypos, int amount)
        {
        Vector3 middlePlatformPos = platforms[(platforms.Length - platforms.Length % 2) / 2].gameObject.transform.position;
        for (float i = -platforms.Length * BottomPlatformDistance; i <= BottomPlatformDistance * platforms.Length; i += Random.Range(Bottom_Platform_Distance_Min, Bottom_Platform_Distance_Min * 1.5f))
            for (float j = -platforms.Length * BottomPlatformDistance; j <= BottomPlatformDistance * platforms.Length; j += Random.Range(Bottom_Platform_Distance_Min, Bottom_Platform_Distance_Min * 1.5f))
                {
                GameObject CurrentBottomPlatform = Instantiate(bottomPlatform, middlePlatformPos + new Vector3(i, -middlePlatformPos.y + Ypos + 1, j), transform.rotation);
                if (Vector3.Distance(CurrentBottomPlatform.transform.position, platforms[platforms.Length - 1].gameObject.transform.position )<10)
                    Destroy(CurrentBottomPlatform);
                }
        }
    }
