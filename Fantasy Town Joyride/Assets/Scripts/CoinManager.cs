using System.Collections;
using Spacecraft.Controllers.Core.Entities;
using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;

namespace Spacecraft
{
public class CoinManager : TrackedEntity
{
    private int RotateSpeed;
    private int Delay;
    private static int Points;
    private void Start()
    {
        Points = PlayerPrefs.GetInt("CollectedMoney",0);
        RotateSpeed = 3;
        Delay = 2;
    }
    private void Update()
    {
        if (IsPaused) PlayerPrefs.SetInt("CollectedMoney", Points);
        transform.Rotate(0, RotateSpeed, 0, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("BigGem"))
        {
                Deactivate();
                Points += 2;
        }
        if (this.gameObject.CompareTag("SmallGem"))
        {
                Deactivate();
                Points += 1;
        }

        Debug.Log("CollectedCoins = " + Points);
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);
        Reactivate();
    }
    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(Delay);
        this.transform.gameObject.SetActive(true);
    }
}
}