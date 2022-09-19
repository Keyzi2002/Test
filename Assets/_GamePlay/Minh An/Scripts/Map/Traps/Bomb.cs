using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    bool positiveXMove = true;
    float startPosValue = -0.5835498f;
    float curPosValue = -0.5835498f;
    float moveSpeed = 0f;

    private void Start()
    {
        curPosValue = transform.position.x;
        moveSpeed = Random.Range(0.1f, 0.2f);
    }
    void Update()
    {
        if(positiveXMove)
        {
            if (curPosValue < startPosValue + 20f)
            {
                curPosValue += moveSpeed;
                transform.position = transform.position + moveSpeed * transform.right;
            }
            else
            {
                positiveXMove = false;
                moveSpeed = Random.Range(0.1f, 0.2f);
            }
        }
        else
        {
            if (curPosValue > startPosValue - 20f)
            {
                curPosValue -= moveSpeed;
                transform.position = transform.position + moveSpeed * -transform.right;
            }
            else
            {
                positiveXMove = true;
                moveSpeed = Random.Range(0.1f, 0.2f);
            }
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
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
