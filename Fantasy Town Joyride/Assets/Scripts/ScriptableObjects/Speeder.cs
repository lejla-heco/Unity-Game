using UnityEngine;

namespace Spacecraft.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "Fantasy Town Joyride/Speeder")]
    public class Speeder : ScriptableObject
    {
        public int Id;
        public GameObject ShipModel;
        public int Price;


        public bool DoIOwnThisItem()
        {
            var PurchasedItems = PlayerPrefs.GetString("PurchasedSpeeders", "1");

            return PurchasedItems.Contains(Id.ToString());
        }


        public void Purchase()
        {
            if (DoIOwnThisItem())
            {
                return;
            }

            var PurchasedItems = PlayerPrefs.GetString("PurchasedSpeeders", "1");
            PurchasedItems += ("-" + Id);
            PlayerPrefs.SetString("PurchasedSpeeders", PurchasedItems);
        }


        public bool IsThisSpeederActive()
        {
            return Id == PlayerPrefs.GetInt("ActiveSpeeder", 1);
        }

        public void Activate()
        {
            if (IsThisSpeederActive() || !DoIOwnThisItem())
            {
                return;
            }

            PlayerPrefs.SetInt("ActiveSpeeder", Id);
        }
    }
}