using UnityEngine;
using System.Collections.Generic;

namespace Spacecraft.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopItemsCollection", menuName = "Fantasy Town Joyride/Shop Items Collection")]
    public class ShopItemsCollection : ScriptableObject
    {
        public List<Speeder> Speeders;
    }
}