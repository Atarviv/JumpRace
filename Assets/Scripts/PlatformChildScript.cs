using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformChildScript : MonoBehaviour
{
    public GameObject PurpleExplosion;
    private void OnCollisionEnter(Collision collision)
        {
        if (GetComponent<MeshRenderer>().material.name == "PurplePlatformMaterial (Instance)")
            {
            Instantiate(PurpleExplosion, transform.position, transform.rotation);
            Destroy(transform.parent.gameObject);
            }
        }
    }
