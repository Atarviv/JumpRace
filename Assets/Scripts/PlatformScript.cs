using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlatformScript : MonoBehaviour
    {
    GameObject Character;
    public Platform Currentplatform = new Platform();
    public bool lastPlatform, closestPlatform;
    public float FarestFromCharacter;
    public GameObject Confetti, closestPlatformCircle;
    public Text IndexText;
    public List<Material> colors = new List<Material>();
    void Awake()
        {
        Character = GameObject.Find("Character");
        Currentplatform.gameObject = gameObject;
        }
    private void Update()
        {
        if (closestPlatform)
            {
            closestPlatformCircle.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -closestPlatformCircle.GetComponent<SpriteRenderer>().color.a + (1 - Vector3.Distance(transform.position, Character.transform.position) / FarestFromCharacter));
            }
        }
    public IEnumerator platformColorandMovement()
        {
        Material color = colors[Random.Range(0, colors.Count)];
        for (int j = 0; j < transform.GetChildCount(); j++)
            if (transform.GetChild(j).name == "Top")
                transform.GetChild(j).GetComponent<MeshRenderer>().material = color;
        int dir = 1;
        int i = 30;
        while (!lastPlatform && color.name == "YellowPlatformMaterial" && Currentplatform.Index > 0)
            {
            while (i != 60)
                {
                transform.position += transform.right * dir * 0.05f;
                yield return new WaitForSeconds(0.05f);
                i++;
                }
            i = 0;
            dir *= -1;
            }
        }
    public IEnumerator BottomPlatformDestroy()
        {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        }
    private void OnCollisionEnter(Collision collision)
        {
        if (lastPlatform)
            {
            Vector3 pos = transform.position + new Vector3(-transform.localScale.x / 2, transform.localScale.y / 2, -transform.localScale.z / 2);
            for (float i = 0; i <= transform.localScale.x; i += transform.localScale.x)
                for (float j = 0; j <= transform.localScale.z; j += transform.localScale.z)
                    Instantiate(Confetti, pos + new Vector3(i, 0, j), Confetti.transform.rotation);
            }

        }
    }
