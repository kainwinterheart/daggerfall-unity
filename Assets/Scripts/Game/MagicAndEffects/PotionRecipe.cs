﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2018 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    
// 
// Notes:
//

using System.Collections.Generic;
using DaggerfallConnect.FallExe;

namespace DaggerfallWorkshop.Game.MagicAndEffects
{
    /// <summary>
    /// Implements a potion recipe to support equality comparison, hashing, and string output.
    /// Internally the recipe is just an array of item template IDs abstracted to an Ingredient type.
    /// A recipe can be constructed from any Ingredient[] array or int[] array of item template IDs.
    /// </summary>
    public class PotionRecipe : IEqualityComparer<PotionRecipe.Ingredient[]>
    {
        #region Fields

        Ingredient[] ingredients = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets potion recipe ingredients.
        /// </summary>
        public Ingredient[] Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PotionRecipe()
        {
        }

        /// <summary>
        /// Ingredient[] array constructor.
        /// </summary>
        /// <param name="ingredients">Ingredient array.</param>
        public PotionRecipe(params Ingredient[] ingredients)
        {
            this.ingredients = ingredients;
        }

        /// <summary>
        /// int[] array of item template IDs constructor.
        /// </summary>
        /// <param name="ids">Array of item template IDs.</param>
        public PotionRecipe(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                Ingredient[] ingredients = new Ingredient[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    ingredients[i].id = ids[i];
                }
                this.ingredients = ingredients;
            }
        }

        #endregion

        #region Structures

        /// <summary>
        /// Abstracts an item ID into an ingredient.
        /// </summary>
        public struct Ingredient
        {
            public int id;

            public Ingredient(int id)
            {
                this.id = id;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Compare another PotionRecipe class with this one.
        /// </summary>
        /// <param name="other">Other potion recipe class.</param>
        /// <returns>True if both recipes are equal.</returns>
        public override bool Equals(object other)
        {
            if (other == null || !(other is PotionRecipe))
                return false;

            return Equals((other as PotionRecipe).Ingredients);
        }

        /// <summary>
        /// Gets hash code for this recipe.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return GetHashCode(ingredients);
        }

        /// <summary>
        /// Gets string listing all ingredients.
        /// </summary>
        /// <returns>Ingredient list.</returns>
        public override string ToString()
        {
            if (ingredients == null || ingredients.Length == 0)
                return string.Empty;

            string result = string.Empty;
            for (int i = 0; i < ingredients.Length; i++)
            {
                ItemTemplate template = DaggerfallUnity.Instance.ItemHelper.GetItemTemplate(ingredients[i].id);
                result += template.name;
                if (i < ingredients.Length - 1)
                    result += ", ";
                else
                    result += ".";
            }

            return result;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if recipe is defined.
        /// </summary>
        /// <returns>True if recipe has at least one ingredient.</returns>
        public bool HasRecipe()
        {
            return (ingredients != null && ingredients.Length > 0);
        }

        /// <summary>
        /// Compare a recipe with this one.
        /// </summary>
        /// <param name="recipe">Other recipe.</param>
        /// <returns>True if other recipe equal with this one.</returns>
        public bool Equals(Ingredient[] ingredients)
        {
            return Equals(this.ingredients, ingredients);
        }

        /// <summary>
        /// Compare two recipes for equality.
        /// </summary>
        /// <param name="ingredients1">First recipe.</param>
        /// <param name="ingredients2">Second recipe.</param>
        /// <returns>True if recipes are equal.</returns>
        public bool Equals(Ingredient[] ingredients1, Ingredient[] ingredients2)
        {
            if (ingredients1 == null || ingredients2 == null || ingredients1.Length != ingredients2.Length)
                return false;

            for (int i = 0; i < ingredients1.Length; i++)
            {
                if (ingredients1[i].id != ingredients2[i].id)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a hash code for recipe from ingredients.
        /// Note: Using hash code calculation from:
        /// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        /// </summary>
        /// <param name="recipe">Ingredients.</param>
        /// <returns>Hash code.</returns>
        public int GetHashCode(Ingredient[] ingredients)
        {
            if (ingredients == null || ingredients.Length == 0)
                return 0;

            int hash = 17;
            for (int i = 0; i < ingredients.Length; i++)
            {
                hash = hash * 23 + ingredients[i].id;
            }

            return hash;
        }

        #endregion
    }
}