using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an ennemy in the 
/// </summary>
public class HREnnemy : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float speed = 2;
    [SerializeField] private float sideSpeed;
    [SerializeField] private float winAfter;


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime + transform.right * sideSpeed * Time.deltaTime;
        if (transform.position.x <= -15 || transform.position.x >= 16) sideSpeed *= -1f;

        if (transform.position.z >= winAfter) MiniGame.instance.EndMiniGame(false);
    }
}
