using UnityEngine;

public class BoruHareketi : MonoBehaviour
{
    public float hiz = 5f; // Movement speed / Hareket hızı
    public float yokOlmaSiniri = -10f; // X coordinate to hide when off-screen / Ekranın solundan çıkınca gizleneceği X koordinatı

    void Update()
    {
        // Move left / Sola doğru hareket et
        // Vector3.left = (-1, 0, 0)
        transform.position += Vector3.left * hiz * Time.deltaTime;

        // Hide when off-screen (for performance)
        // Ekrandan çıkınca yok ol (performans için)
        if (transform.position.x < yokOlmaSiniri)
        {
            // OLD: Destroy completely (Not memory friendly)
            // ESKİ: Tamamen yok et (Bellek dostu değil)
            // Destroy(gameObject); 
            
            // NEW: Just hide it. So Spawner can reuse it.
            // YENİ: Sadece gizle. Böylece Spawner onu tekrar kullanabilir.
            gameObject.SetActive(false);
        }
    }
}
