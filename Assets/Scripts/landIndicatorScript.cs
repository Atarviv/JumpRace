using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landIndicatorScript : MonoBehaviour
{
    public int Seconds;

    public IEnumerator Land(float PlatformScale)
        {
        transform.localScale = new Vector3(PlatformScale * transform.localScale.x, PlatformScale * transform.localScale.y, transform.localScale.z);
        for(int i =0; i < Seconds * 20; i++)
            {
            transform.localScale = new Vector3(1.05f*transform.localScale.x, 1.05f*transform.localScale.y, transform.localScale.z);
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.05f);
            yield return new WaitForSeconds(0.02f);
            }
        Destroy(gameObject);
        }
}
