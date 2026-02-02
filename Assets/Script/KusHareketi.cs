using UnityEngine;
using UnityEngine.InputSystem;

public class KusHareketi : MonoBehaviour
{
    public Rigidbody2D kusRb; // Physics component / Fizik bileşeni
    public float ziplamaGucu = 10f; // Jump force / Zıplama gücü
    
    // NOTE: Gravity Scale must be positive in Unity (1 = Earth gravity). Negative makes it fly up.
    // NOT: Unity'de Gravity Scale pozitif olmalıdır (1 = Dünya yerçekimi). Eksi yaparsan uçar.
    public float cekimKuvveti = 1.5f; // Gravity Scale / Yer çekimi

    void Start()
    {
        // To assign the bird's Rigidbody component if not assigned
        // Kuşun Rigidbody componentini bizim atayabilmek için
        if (kusRb == null) kusRb = GetComponent<Rigidbody2D>();

        // Enable gravity for the bird
        // Kuşun yerçekimine göre düşmesini sağla
        kusRb.bodyType = RigidbodyType2D.Dynamic;
        
        // Set gravity scale from variable
        // Yer çekimi gücünü değişkenden al
        kusRb.gravityScale = cekimKuvveti;
    }

    void Update()
    {
        // Jump control with the new Input System
        // Yeni Input System ile zıplama kontrolü
        // Space key, Left Mouse Button, or Touch
        // Space tuşu, Fare sol tık veya Ekrana dokunma
        bool zipla = false;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            zipla = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            zipla = true;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            zipla = true;
        }

        if (zipla)
        {
            // Propel the bird upwards
            // Kuşu yukarı fırlat
            kusRb.linearVelocity = Vector2.up * ziplamaGucu;
        }

        // --- ROTATION ANIMATION / ROTASYON ANIMASYONU ---
        // Rotate head based on velocity
        // Hızına göre kafasını çevir
        
        // Max 30 degrees when going up, max -90 degrees when going down
        // Yukarı çıkarken max 30 derece, aşağı inerken max -90 derece
        float egim = Mathf.Clamp(kusRb.linearVelocity.y * 4f, -90f, 30f);
        
        // Applying directly instead of lerp makes it feel more "snappy" like the original
        // Yumuşak geçiş yerine direkt uyguluyoruz, daha "snappy" hissettirir
        transform.rotation = Quaternion.Euler(0, 0, egim);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If collided with anything (Pipe or Ground), game over
        // Herhangi bir şeye (Boru veya Zemin) çarpınca oyun biter
        GameManager.Instance.GameOver();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If passed through an object tagged "ScoreZone", get point
        // "ScoreZone" tag'li bir objenin içinden geçince puan al
        if (other.CompareTag("ScoreZone"))
        {
            GameManager.Instance.ScoreArtir();
        }
    }
}
