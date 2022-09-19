using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public List<ParticleSystem> explosions;

    private void Start()
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            explosions[i].gameObject.SetActive(false);
        }
    }
    public void SpawnExplosion(Vector3 spawnPos)
    {
        for(int i = 0; i < explosions.Count; i++)
        {
            if(!explosions[i].gameObject.activeSelf)
            {
                explosions[i].gameObject.transform.position = spawnPos;
                explosions[i].gameObject.SetActive(true);
                explosions[i].Play();
                StartCoroutine(WaitToUnActive(explosions[i].gameObject));
                return;
            }
        }
    }

    IEnumerator WaitToUnActive(GameObject explo)
    {
        yield return new WaitForSeconds(1.5f);
        explo.SetActive(false);
    }
}
