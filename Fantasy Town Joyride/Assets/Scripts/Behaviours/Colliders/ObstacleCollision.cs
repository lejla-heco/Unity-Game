using System.Collections;
using Spacecraft.Core.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Spacecraft.Behaviours.Colliders
{
    public class ObstacleCollision : TrackedEntity
    {
        [SerializeField] private GameObject CollisionEffect;
        [SerializeField] private GameObject DeathEffect;
        [SerializeField] private GameObject ProtectedEffect;
        [SerializeField] private RawImage FirstLife;
        [SerializeField] private RawImage SecondLife;
        [SerializeField] private RawImage ThirdLife;
        [SerializeField] private Material Mat1;
        [SerializeField] private GameObject GameOverMenu;

        private Renderer Renderer;
        private Color[] RegularColors;


        private void Start()
        {
            Renderer = gameObject.GetComponentInChildren<Renderer>();
            RegularColors = new Color[Renderer.materials.Length];

            for (int i = 0; i < Renderer.materials.Length; i++)
                RegularColors[i] = Renderer.materials[i].color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PowerUp"))
            {
                IsProtected = true;
                other.gameObject.SetActive(false);
                ActivateEffect(ProtectedEffect, 10f);
                StartCoroutine(ReactivatePowerUp(other.gameObject));
            }

            if (!IsProtected)
            {
                if (other.gameObject.CompareTag("Barrel") || other.gameObject.CompareTag("Hidrant") ||
                    other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("StreetSign") ||
                    other.gameObject.CompareTag("RoadBlock"))
                {
                    HitObstacle();
                }
                else if (other.gameObject.CompareTag("GasTank"))
                {
                    FirstLife.color = Color.black;
                    SecondLife.color = Color.black;
                    ThirdLife.color = Color.black;
                    HitObstacle(3);
                }
            }
        }

        IEnumerator ReactivatePowerUp(GameObject other)
        {
            yield return new WaitForSeconds(1.0f);
            other.SetActive(true);
        }

        private void HitObstacle(int severity = 1)
        {
            if (IsProtected) return;

            var LivesLeft = LoseLife(severity);
            if (LivesLeft == 2)
            {
                FirstLife.color = Color.black;
                ActivateEffect(CollisionEffect, 0.5f);
                StartCoroutine(Flasher());
            }
            else if (LivesLeft == 1)
            {
                SecondLife.color = Color.black;
                ActivateEffect(CollisionEffect, 0.5f);
                StartCoroutine(Flasher());
            }
            else if (LivesLeft <= 0)
            {
                ThirdLife.color = Color.black;
                ActivateEffect(DeathEffect, 5f);

                GameOverMenu.SetActive(true);
            }
        }

        private void ActivateEffect(GameObject effect, float delay)
        {
            effect.SetActive(true);
            StartCoroutine(Deactivate(effect, delay));
        }

        private IEnumerator Deactivate(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            effect.SetActive(false);
            if (IsProtected) IsProtected = false;
        }

        private IEnumerator Flasher()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Renderer.materials.Length; j++)
                {
                    Renderer.materials[j].color = Mat1.color;
                }

                yield return new WaitForSeconds(.1f);
                for (int j = 0; j < Renderer.materials.Length; j++)
                {
                    Renderer.materials[j].color = RegularColors[j];
                }

                yield return new WaitForSeconds(.1f);
            }
        }
    }
}