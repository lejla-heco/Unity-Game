using Spacecraft.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spacecraft.UI
{
    public class Shop : MonoBehaviour
    {
        public ShopItemsCollection Collection;

        [SerializeField] private TextMeshProUGUI CoinsValueText;
        [SerializeField] private TextMeshProUGUI SpeederPriceText;
        [SerializeField] private Button BuyBtn;
        [SerializeField] private Button ActivateBtn;

        private int SpeederIndex;
        private int Coins;
        private GameObject SpeederInScene;
        private bool IsDirty;

        public void Next()
        {
            NavigateSpeederPreview(1);
        }

        public void Prev()
        {
            NavigateSpeederPreview(-1);
        }

        public void PurchaseSpeeder()
        {
            var DesiredSpeederPrice = Collection.ShopItems[SpeederIndex].Price;
            if (Coins < DesiredSpeederPrice)
            {
                return;
            }

            Collection.ShopItems[SpeederIndex].Purchase();
            // deduct money
            Coins -= DesiredSpeederPrice;
            PlayerPrefs.SetInt("CollectedMoney", Coins);

            IsDirty = true;
        }

        public void ActivateSpeeder()
        {
            Collection.ShopItems[SpeederIndex].Activate();

            IsDirty = true;
        }

        public void ExitGarage()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void Start()
        {
            SpeederIndex = 0;

            // read coins
            Coins = PlayerPrefs.GetInt("CollectedMoney", 0);

            IsDirty = true;
        }

        private void NavigateSpeederPreview(int direction)
        {
            var Temp = SpeederIndex;
            Temp += direction;
            if (Temp >= Collection.ShopItems.Count)
            {
                Temp = 0;
            }

            if (Temp < 0)
            {
                Temp = Collection.ShopItems.Count - 1;
            }

            SpeederIndex = Temp;
            IsDirty = true;
        }

        private void FixedUpdate()
        {
            if (!IsDirty)
            {
                return;
            }


            CoinsValueText.text = Coins.ToString();

            Destroy(SpeederInScene);

            // display current speeder
            SpeederInScene = Instantiate(
                Collection.ShopItems[SpeederIndex].ShipModel,
                new Vector3(0, 0, 0),
                Quaternion.Euler(new Vector3(11, -200, 0))
            );

            var Price = Collection.ShopItems[SpeederIndex].Price;
            var IsPurchased = Collection.ShopItems[SpeederIndex].DoIOwnThisItem();
            var IsActive = Collection.ShopItems[SpeederIndex].IsThisSpeederActive();

            if (Price > Coins || IsPurchased)
            {
                BuyBtn.interactable = false;
            }
            else
            {
                BuyBtn.interactable = true;
            }

            if (IsPurchased && !IsActive)
            {
                ActivateBtn.interactable = true;
            }
            else
            {
                ActivateBtn.interactable = false;
            }

            SpeederPriceText.text = "Price: " + Price;


            IsDirty = false;
        }
    }
}