using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookScript : InteractableObjectScript
{
    [SerializeField] private RecipeSO[] recipes;

    public override void Interaction()
    {
        player.LearnRecipes(recipes);

        Destroy(gameObject);
    }
}
