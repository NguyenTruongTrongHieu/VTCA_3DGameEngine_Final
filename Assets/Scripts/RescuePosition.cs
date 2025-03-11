using UnityEngine;

public class RescuePosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hostage"))
        { 
            Destroy(other.gameObject);
        }
    }
}
