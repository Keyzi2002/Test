using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProperty : MonoBehaviour
{
    public Transform playerTransform;
    void Start()
    {
        playerTransform = Object.FindObjectOfType<Player>().gameObject.transform;
    }
    
}
