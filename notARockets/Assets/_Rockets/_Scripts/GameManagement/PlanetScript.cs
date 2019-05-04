﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour {

    public float gravity = -12;  //Сила, с которой планета будет притягивать объекты с весом

    public void Attract(Transform playerTransform)
    { 
        Vector3 gravityUp = (playerTransform.position - transform.position).normalized;  //Вектор направления силы
        Vector3 localUp = playerTransform.up;

        playerTransform.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);  //Применение силы в заданном направлении

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * playerTransform.rotation; //Поворот по шару

        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, 50f * Time.deltaTime); //Плавность поворота
    }
}