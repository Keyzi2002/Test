using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicBomb : MonoBehaviour
{
    public Transform rootPos;
    [SerializeField]
    float horizonAmp, verticalAmp;
    Vector3 target;
    public float hitDistance = 0.75f;
    float coolDown = 0f;
    public float moveSpeed = 1f;
    public float distance = 115f;
    float traveledDistance = 0f;
    public float minCD = 1f;
    public float maxCD = 3f;
    public GameObject explosion;
    bool explosing = false;

    public ExplosionPool explosionPool;

    private void Start()
    {
        ReChargeBomb();
    }
    void Update()
    {
        if(coolDown > 0)
        {
            return;
        }
        explosing = false;
        traveledDistance += moveSpeed;
        transform.position = transform.position + moveSpeed * transform.forward;

        if (traveledDistance >= distance)
        {
            ReChargeBomb();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(!explosing)
        {
            explosing = true;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, hitDistance);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<Character>() != null)
                {
                    hitCollider.gameObject.GetComponent<Character>().statePlayer = StatePlayer.Fall;
                }
            }
            //GameObject explo = Instantiate(explosion, transform.position, Quaternion.Euler(0, 180, 0));
            //Destroy(explo, 1.5f);
            explosionPool.SpawnExplosion(transform.position);
            ReChargeBomb();
        }
    }
    void ReChargeBomb()
    {
        coolDown = Random.Range(minCD, maxCD);
        StartCoroutine(WaitToRecharge());
    }

    IEnumerator WaitToRecharge()
    {
        yield return new WaitForSeconds(coolDown);
        coolDown = 0f;
        traveledDistance = 0;
        target = rootPos.position;
        target += new Vector3(Random.Range(-horizonAmp, horizonAmp), 0f, Random.Range(-verticalAmp, verticalAmp));
        Vector3 delta = new Vector3(Random.Range(-50f, 50f), 50f, Random.Range(-50f, 50f));
        transform.position = target + delta;
        transform.LookAt(target);
    }
}
