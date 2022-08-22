using System;
using UnityEngine;

namespace Prince
{
    public class PickableInteractions : MonoBehaviour
    {
        /// <summary>
        /// Used by small potions to restore one health point.
        /// </summary>
        public void RestoreOneHealthPoint()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used by poison potions to take away one health point.
        /// </summary>
        public void LoseOneHealthPoint()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used by big potions to restore all health points and add an extra one.
        /// </summary>
        public void EnhanceHealth()
        {
            throw new NotImplementedException();
        }
    }
}