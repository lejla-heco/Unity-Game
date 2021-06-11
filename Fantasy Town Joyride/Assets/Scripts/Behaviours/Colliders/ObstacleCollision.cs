using System;
using System.Collections;
using Spacecraft.Core.LevelGenerator;
using System.Collections.Generic;
using Spacecraft.Core;
using Spacecraft.Core.Entities;
using UnityEditor.AssetImporters;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Spacecraft
{
    public class ObstacleCollision : TrackedEntity
    {
        [SerializeField] private GameObject CollisionEffect;
        [SerializeField] private GameObject DeathEffect;
        [SerializeField] private GameObject ProtectedEffect;
        [SerializeField] private RawImage FirstLife;
        [SerializeField] private RawImage SecondLife;
        [SerializeField] private RawImage ThirdLife;

        private GameObject PickUpEffect;
        private float Delay;

        private Renderer Renderer;
        private Color[] RegularColors;
        [SerializeField] private Material Mat1;


        private void Start()
        {
            Renderer = GetComponent<Renderer>();
            RegularColors = new Color[Renderer.materials.Length];

            for (int i = 0; i < Renderer.materials.Length; i++)
                RegularColors[i] = Renderer.materials[i].color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PowerUp"))
            {
                IsProtected = true;
                Delay = 10.0f;
                other.gameObject.SetActive(false);
                ActivateEffect(ProtectedEffect);
                StartCoroutine(ReactivatePowerUp(other.gameObject));
            }

            if (!IsProtected)
            {
                if (other.gameObject.CompareTag("Barrel") || other.gameObject.CompareTag("Hidrant") ||
                    other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("StreetSign") ||
                    other.gameObject.CompareTag("RoadBlock"))
                {
                    LoseLife();
                }
                else if (other.gameObject.CompareTag("GasTank"))
                {
                    FirstLife.color = Color.black;
                    SecondLife.color = Color.black;
                    ThirdLife.color = Color.black;
                    Die();
                }
            }
        }

        IEnumerator ReactivatePowerUp(GameObject other)
        {
            yield return new WaitForSeconds(1.0f);
            other.SetActive(true);
        }


        public void LoseLife()
        {
            if (IsProtected) return;
            Delay = 0.5f;
            LoseLife();
            if (Lives == 2)
            {
                FirstLife.color = Color.black;
                ActivateEffect(CollisionEffect);
                StartCoroutine(Flasher());
            }
            else if (Lives == 1)
            {
                SecondLife.color = Color.black;
                ActivateEffect(CollisionEffect);
                StartCoroutine(Flasher());
            }
            else if (Lives == 0)
            {
                ThirdLife.color = Color.black;
                Die();
            }
        }

        private void Die()
        {
            if (IsProtected) return;
            Delay = 5.0f;
            ActivateEffect(DeathEffect);
            Die();
        }

        private void ActivateEffect(GameObject effect)
        {
            effect.SetActive(true);
            StartCoroutine(Deactivate(effect));
        }

        IEnumerator Deactivate(GameObject effect)
        {
            yield return new WaitForSeconds(Delay);
            effect.SetActive(false);
            if (IsProtected) IsProtected = false;
        }

        IEnumerator Flasher()
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