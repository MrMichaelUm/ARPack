using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    

    public GameObject meteorPrefab; //Цельный префаб готового метеорита(С "тенью", эффектами и т.д.)

    public GameObject AlfaMainMeteor; //Префаб самого камня метеорита внутри цельного префаба

    float distance;  // Радиус спавна
    
    public float delayTime = 3f; //Частота спавна
    
    
    
    void Start()
    {
        distance = AlfaMainMeteor.GetComponent<MeteorScript>().distance;

        StartCoroutine(SpawnMeteor());
        
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = Random.onUnitSphere * distance;       // Позиция на поверхности "сферы" заданного радиуса

        GameObject meteorObject = ObjectPoolingManager.Instance.GetMeteor(meteorPrefab);  // Вызываем метеорит из пула

        meteorObject.transform.position = pos;                       //Ставим в позицию
        meteorObject.transform.rotation = Quaternion.identity;       //Не куртим

        yield return new WaitForSeconds(delayTime);

        StartCoroutine(SpawnMeteor());
    }
}
