using System.Collections;
using Spacecraft.Controllers.Core.LevelGenerator;
using UnityEngine;

namespace Spacecraft
{
public class CoinManager : MonoBehaviour
{
    private int RotateSpeed;
    private int Delay;
    private void Start()
    {
        RotateSpeed = 4;
        Delay = 2;
    }
    private void Update()
    {
        transform.Rotate(0, RotateSpeed, 0, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        SpacecraftTag spacecraftTag = other.GetComponent<SpacecraftTag>();
        if (spacecraftTag != null)
        {
            this.gameObject.SetActive(false);
            Reactivate();
        }
    }
    
    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(Delay);
        this.transform.gameObject.SetActive(true);
    }
}
}