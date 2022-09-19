using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float hitDistance = 0.75f;
    float coolDown = 0f;
    public float moveSpeed = 1f;
    public float distance = 115f;
    float traveledDistance = 0f;
    public float minCD = 1f;
    public float maxCD = 3f;

    private void Start()
    {
        coolDown = Random.Range(minCD, maxCD);
    }
    void Update()
    {
        
        if(coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            return;
        }
        traveledDistance += moveSpeed;
        transform.position = transform.position + moveSpeed * transform.forward;
        
        if(traveledDistance >= distance)
        {
            traveledDistance = 0;
            transform.position = transform.position + distance * -transform.forward;
            coolDown = Random.Range(minCD, maxCD);
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, hitDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Character>() != null)
            {
                hitCollider.gameObject.GetComponent<Character>().statePlayer = StatePlayer.Fall;
                //gameObject.SetActive(false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitDistance);
    }
}
