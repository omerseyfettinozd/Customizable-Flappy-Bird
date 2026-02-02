using UnityEngine;

public class BoruSpawner : MonoBehaviour
{
    public GameObject boruPrefab; // Boru prefab'ını buraya sürükleyeceğiz / Drag the pipe prefab here
    public float sure = 2f; // Kaç saniyede bir boru gelecek / Time interval between spawns
    public float yukseklikAraligi = 1.5f; // Y ekseninde ne kadar yukarı/aşağı oynayabilir / Y-axis range for random height
    
    // Object Pooling Değişkenleri / Object Pooling Variables
    public int havuzBoyutu = 5; // Havuzda kaç boru olacak / Number of pipes in the pool
    private GameObject[] borular; // Boruları tutacak dizi / Array to hold the pipes
    private int siradakiBoru = 0; // Sıradaki borunun indeksi / Index of the next pipe
    private float zamanSayaci = 0; // Zamanlayıcı / Timer

    void Start()
    {
        // --- OBJECT POOLING KURULUMU / SETUP OBJECT POOLING ---
        // Bellekte yer açmak için baştan boruları oluşturup gizliyoruz.
        // We instantiate pipes upfront and hide them to save memory performance.
        
        borular = new GameObject[havuzBoyutu];
        
        for (int i = 0; i < havuzBoyutu; i++)
        {
            borular[i] = Instantiate(boruPrefab, Vector2.zero, Quaternion.identity);
            borular[i].SetActive(false); // Başlangıçta gizle / Hide initially
        }
    }

    void Update()
    {
        // Süre sayacı / Time counter
        if (zamanSayaci > sure)
        {
            HavuzdanBoruGetir();
            zamanSayaci = 0;
        }

        zamanSayaci += Time.deltaTime;
    }

    void HavuzdanBoruGetir()
    {
        // Spawner'ın kendi yüksekliğini baz alarak rastgele bir Y belirle
        // Determine a random Y position based on Spawner's height
        float rastgeleY = Random.Range(-yukseklikAraligi, yukseklikAraligi);
        Vector3 yeniPozisyon = transform.position + new Vector3(0, rastgeleY, 0);

        // --- POOLING MANTIĞI / POOLING LOGIC ---
        // Yeni oluşturmak yerine sıradaki boruyu kullan
        // Use the next pipe instead of creating a new one
        
        borular[siradakiBoru].SetActive(false); // Önce kapat (reset için) / Disable first (for reset)
        borular[siradakiBoru].transform.position = yeniPozisyon; // Pozisyonunu ayarla / Set position
        borular[siradakiBoru].SetActive(true); // Tekrar aç / Enable again

        // Bir sonraki boruya geç, eğer sona geldiysek başa dön (Döngü)
        // Move to next pipe, loop back to start if at the end
        siradakiBoru++;
        if (siradakiBoru >= havuzBoyutu)
        {
            siradakiBoru = 0;
        }
    }
}
