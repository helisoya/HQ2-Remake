using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a bowl in the BC MiniGame
/// </summary>
public class BCBowl : MonoBehaviour
{
    [SerializeField] private BCElements element;
    [SerializeField] private GameObject linkedPrefab;
    private Transform elementTransform;
    private bool movingElement;

    void OnMouseDown()
    {
        BCMiniGame miniGame = (BCMiniGame)MiniGame.instance;

        if (miniGame.canInterract && miniGame.currentPizza.CanAdd(element))
        {
            miniGame.currentPizza.AddElements(element);
            elementTransform = Instantiate(linkedPrefab, transform.position, Quaternion.identity, miniGame.currentPizza.GetElementsRoot()).transform;
            movingElement = true;
            miniGame.canInterract = false;
        }
    }

    void Update()
    {
        if (movingElement)
        {
            if (elementTransform == null)
            {
                movingElement = false;
            }
            else
            {
                elementTransform.localPosition = Vector3.MoveTowards(elementTransform.localPosition, Vector3.zero, 5 * Time.deltaTime);
                if (elementTransform.localPosition == Vector3.zero)
                {
                    movingElement = false;
                    ((BCMiniGame)MiniGame.instance).canInterract = true;
                }
            }
        }
    }
}
