using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a bullet in the "Running From Marcel" Minigame
/// </summary>
public class RFMBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5;
    [SerializeField] private float speed = 15;


    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
