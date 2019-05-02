using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    

    public GameObject meteorPrefab;
    public GameObject AlfaMainMeteor;
    float distance;
    
    public float delayTime = 3f;
    
    
    
    void Start()
    {
        distance = AlfaMainMeteor.GetComponent<MeteorScript>().distance;
        StartCoroutine(SpawnMeteor());
        
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = Random.onUnitSphere * distance;
        GameObject meteorObject = ObjectPoolingManager.Instance.GetMeteor(meteorPrefab);
        meteorObject.transform.position = pos;
        meteorObject.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(delayTime);

        StartCoroutine(SpawnMeteor());
    }
}
