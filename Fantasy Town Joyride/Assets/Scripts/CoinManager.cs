using UnityEngine;

namespace Spacecraft
{
public class CoinManager : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
}