using UnityEngine;
using System.IO;

public class BackgroundSelector : MonoBehaviour
{
    // The SpriteRenderer component of the background object
    // Arkaplan objesinin SpriteRenderer bileşeni
    public SpriteRenderer backgroundRenderer;

    private Vector2 initialWorldSize; // Initial world dimensions / Başlangıç dünya boyutları
    private Sprite originalSprite; // Original sprite / Orijinal sprite
    private string SAVE_KEY = "SelectedBackground"; // Save key / Kayıt anahtarı

    void Start()
    {
        if (backgroundRenderer != null)
        {
            // Capture the initial bounds (world space size)
            // Başlangıç boyutlarını yakala (dünya uzayı boyutu)
            initialWorldSize = backgroundRenderer.bounds.size;
            // Save the original sprite to allow resetting
            // Sıfırlamaya izin vermek için orijinal sprite'ı kaydet
            originalSprite = backgroundRenderer.sprite;

            // --- LOAD SAVED BACKGROUND ---
            // --- KAYITLI ARKAPLANI YÜKLE ---
            string savedPath = PlayerPrefs.GetString(SAVE_KEY, "");
            if (!string.IsNullOrEmpty(savedPath))
            {
                if (File.Exists(savedPath))
                {
                    LoadBackgroundFromFile(savedPath);
                }
            }
        }
    }

    public void SelectBackground()
    {
        string[] fileTypes = new string[] { "image/*", "public.image" };

        #if UNITY_STANDALONE_WIN
        // Windows dedicated file picker
        // Windows için özel dosya seçici
        WindowsFilePicker.PickFile((path) =>
        {
            OnImagePicked(path);
        });
        #else
        // Mobile and Editor native file picker
        // Mobil ve Editör için yerel dosya seçici
        NativeFilePicker.PickFile((path) =>
        {
            OnImagePicked(path);
        }, fileTypes);
        #endif
    }

    private void OnImagePicked(string path)
    {
        if (path == null)
        {
            Debug.Log("Operation cancelled / İşlem iptal edildi");
        }
        else
        {
            Debug.Log("Selected file: " + path);
            LoadBackgroundFromFile(path);

            // Save the selection
            // Seçimi kaydet
            PlayerPrefs.SetString(SAVE_KEY, path);
            PlayerPrefs.Save();
        }
    }

    public void ResetBackground()
    {
        // Delete the saved preference
        // Kaydedilen tercihi sil
        PlayerPrefs.DeleteKey(SAVE_KEY);

        if (backgroundRenderer != null && originalSprite != null)
        {
            // Restore original sprite
            // Orijinal sprite'ı geri yükle
            backgroundRenderer.sprite = originalSprite;
            
            ApplyScaling(); 
        }
    }

    private void LoadBackgroundFromFile(string path)
    {
        Texture2D texture = null;
        byte[] fileData = File.ReadAllBytes(path);
        if (fileData != null && fileData.Length > 0)
        {
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Auto-resizes texture / Dokuyu otomatik boyutlandırır
        }

        if (texture == null)
        {
            Debug.LogError("Could not load image: " + path);
            return;
        }

        // Create Sprite
        // Pivot at center (0.5, 0.5)
        // Sprite oluştur (Pivot merkezde)
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        if (backgroundRenderer != null)
        {
            backgroundRenderer.sprite = newSprite;
            ApplyScaling();
        }
    }

    private void ApplyScaling()
    {
        if (backgroundRenderer == null) return;

        // Reset scale to 1 to measure the unscaled sprite size
        // Ölçeklendirilmemiş sprite boyutunu ölçmek için scale'i 1 yap
        backgroundRenderer.transform.localScale = Vector3.one;

        // Get the new unscaled size
        // Yeni ölçeksiz boyutu al
        Vector2 currentSize = backgroundRenderer.bounds.size;

        // Calculate ratios to stretch the image to the initial world size
        // Resmi başlangıç dünya boyutuna gerdirmek için oranları hesapla
        float widthRatio = initialWorldSize.x / currentSize.x;
        float heightRatio = initialWorldSize.y / currentSize.y;

        // Apply Stretch-to-Fill
        // Ekrana Sığdır (Stretch) uygula
        backgroundRenderer.transform.localScale = new Vector3(widthRatio, heightRatio, 1f);
    }
}
