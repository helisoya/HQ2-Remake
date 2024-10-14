using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a pizza for the BC MiniGame
/// </summary>
public class BCPizza : MonoBehaviour
{
    [SerializeField] private Animator pizzaAnimator;
    [SerializeField] private Transform elementsRoot;
    private BCElements elements;

    /// <summary>
    /// Gets the elements root
    /// </summary>
    /// <returns>The elements root</returns>
    public Transform GetElementsRoot()
    {
        return elementsRoot;
    }

    /// <summary>
    /// Adds elements to the pizza
    /// </summary>
    /// <param name="elementsAdded">The elements to add</param>
    public void AddElements(BCElements elementsAdded)
    {
        if (elementsAdded.hasSalad) elements.hasSalad = true;
        if (elementsAdded.hasHam) elements.hasHam = true;
        if (elementsAdded.hasTomato) elements.hasTomato = true;
        if (elementsAdded.hasCheese) elements.hasCheese = true;
        if (elementsAdded.hasChocolate) elements.hasChocolate = true;
    }

    /// <summary>
    /// Resets the pizza's elements
    /// </summary>
    public void Reset()
    {
        elements = new BCElements();
        foreach (Transform child in elementsRoot)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Returns true if an element can be added
    /// </summary>
    /// <param name="elements">The elements to add</param>
    /// <returns>Can at least one element be added ?</returns>
    public bool CanAdd(BCElements check)
    {
        if (check.hasSalad && !elements.hasSalad) return true;
        if (check.hasChocolate && !elements.hasChocolate) return true;
        if (check.hasCheese && !elements.hasCheese) return true;
        if (check.hasTomato && !elements.hasTomato) return true;
        if (check.hasHam && !elements.hasHam) return true;
        return false;
    }

    /// <summary>
    /// Checks if the pizza is conform to the target
    /// </summary>
    /// <param name="target">The target</param>
    /// <returns>Is the pizza conform ?</returns>
    public bool IsConform(BCElements target)
    {
        return elements.hasHam == target.hasHam
            && elements.hasTomato == target.hasTomato
            && elements.hasSalad == target.hasSalad
            && elements.hasChocolate == target.hasChocolate
            && elements.hasCheese == target.hasCheese;
    }

    /// <summary>
    /// Trigger an animation
    /// </summary>
    /// <param name="triggerName">The trigger's name</param>
    public void TriggerAnimation(string triggerName)
    {
        pizzaAnimator.SetTrigger(triggerName);
    }
}
