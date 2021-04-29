using System.Collections;
using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;

namespace Spacecraft
{
public class CoinManager : MonoBehaviour
{
    private int RotateSpeed;
    private int Delay;
    private static int Points = 0;
    private void Start()
    {
        RotateSpeed = 3;
        Delay = 2;
    }
    private void Update()
    {
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