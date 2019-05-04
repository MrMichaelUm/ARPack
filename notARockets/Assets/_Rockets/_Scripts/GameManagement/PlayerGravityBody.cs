using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityBody : MonoBehaviour {

    public PlanetScript attractorPlanet;  //Тело к которому будет питягиваться наш объект

    private Transform playerTransform;

    void Start()
    {

        GetComponent<Rigidbody>().useGravity = false;   //Мы создаём свою гравитацию
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;  //Со своим вращением

        playerTransform = transform;
    }

    void FixedUpdate()
    {
        if (attractorPlanet)
        {
            attractorPlanet.Attract(playerTransform);
        }
    }
}
