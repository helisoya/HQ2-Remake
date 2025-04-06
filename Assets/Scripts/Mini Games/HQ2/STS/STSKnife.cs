using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a knife in the
/// </summary>
public class STSKnife : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifeSpan = 10f;
    private bool move = true;
    private float lifeStart;

    void Start()
    {
        lifeStart = Time.time;
        move = true;
    }

    void Update()
    {
        if (!move) return;

        transform.position += transform.up * speed * Time.deltaTime;

        if (Time.time - lifeStart >= lifeSpan)
        {
            move = false;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!move) return;

        if (other.attachedRigidbody.name == "Bg")
        {
            move = false;
            STSMiniGame miniGame = (STSMiniGame)MiniGame.instance;
            miniGame.AddPoints(50);
            miniGame.ParentToTarget(transform);
        }
        else if (other.attachedRigidbody.name == "Salesman")
        {
            move = false;
            STSMiniGame miniGame = (STSMiniGame)MiniGame.instance;
            miniGame.AddPoints(-25);
            miniGame.ParentToTarget(transform);
        }
    }
}
