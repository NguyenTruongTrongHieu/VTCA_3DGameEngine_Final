using UnityEngine;

public class duongdan : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("duongdan"))
            {
                // Xóa vật phẩm khỏi scene
                Destroy(gameObject);
            }
        }
    }
}
