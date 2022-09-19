using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    float moveSpeed = 0f;
    public bool rightLane;
    public float minSpeed = 0.25f;
    public float maxSpeed = 0.45f;

    private void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        if(rightLane)
        {
            if (transform.position.x > endPos.position.x)
            {
                transform.position = transform.position + moveSpeed * -transform.right;
            }
            else
            {
                moveSpeed = Random.Range(minSpeed, maxSpeed);
                transform.position = new Vector3(startPos.position.x, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (transform.position.x < startPos.position.x)
            {
                transform.position = transform.position + moveSpeed * -transform.right;
            }
            else
            {
                moveSpeed = Random.Range(0.25f, 0.45f);
                transform.position = new Vector3(endPos.position.x, transform.position.y, transform.position.z);
            }
        }
        

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + -transform.right * 1.5f, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Character>() != null)
            {
                hitCollider.gameObject.GetComponent<Character>().statePlayer = StatePlayer.Fall;
                //gameObject.SetActive(false);
            }
        }
    }
}
