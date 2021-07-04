using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform
{
    public int Index { get; set; }
    public Platform next { get; set; }
    public GameObject gameObject { get; set; }
}
