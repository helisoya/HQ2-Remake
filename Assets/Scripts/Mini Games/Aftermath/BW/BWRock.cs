using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Rock in the "Battle against Wizard" MiniGame
/// </summary>
public class BWRock : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = 10;
    private Vector3 direction;

    void Update()
    {
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        lifeTime -= Time.deltaTime;

        transform.position += direction * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ((BWMiniGame)MiniGame.instance).TakeDamage();
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 direction)
    {
        this.direction = direction;
    }
}
