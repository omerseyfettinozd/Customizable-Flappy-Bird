using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResimSecici : MonoBehaviour
{
	// SpriteRenderer component of the bird or object to change
	// Kuşun veya değiştirilecek objenin SpriteRenderer bileşeni
	public SpriteRenderer kuS_Renderer;

	private Vector2 baslangicBoyutu; // Initial size / İlk baştaki boyut
	private Sprite orjinalSprite; // To store original bird sprite / Orijinal kuşu saklamak için
	private string KAYIT_ANAHTARI = "SecilenKusResmi"; // PlayerPrefs key / PlayerPrefs anahtarı

	void Start()
	{
		if( kuS_Renderer != null )
		{
			// Save the initial bounds (width and height)
			// Başlangıçtaki kapladığı alanı (en ve boy) kaydediyoruz
			baslangicBoyutu = kuS_Renderer.bounds.size;
			// Save the original sprite
			// Orijinal sprite'ı kaydediyoruz
			orjinalSprite = kuS_Renderer.sprite;

			// --- LOAD SAVED IMAGE / KAYITLI RESMİ YÜKLE ---
			string kayitliYol = PlayerPrefs.GetString( KAYIT_ANAHTARI, "" );
			if( !string.IsNullOrEmpty( kayitliYol ) )
			{
				// Check if file still exists
				// Dosya hala orada mı kontrol et
				if( File.Exists( kayitliYol ) )
				{
					ResmiKusaAta( kayitliYol );
				}
			}
		}
	}

	public void ResimSec()
	{
		// Start image selection with NativeFilePicker
		// NativeFilePicker ile resim seçimi başlat
		// image/* for Android, public.image for iOS generic image type
		// image/* Android için, public.image iOS için genel resim türüdür
		string[] fileTypes = new string[] { "image/*", "public.image" };

		#if UNITY_STANDALONE_WIN
		// Special file picker for Windows Build
		// Windows Build için özel dosya seçici
		WindowsFilePicker.PickFile( ( path ) => 
		{
			OnImagePicked(path);
		});
		#else
		// Native File Picker for Mobile and Editor
		// Mobil ve Editor için Native File Picker
		NativeFilePicker.PickFile( ( path ) =>
		{
			OnImagePicked(path);
		}, fileTypes );
		#endif
	}

	private void OnImagePicked(string path)
	{
		if( path == null )
		{
			Debug.Log( "İşlem iptal edildi / Operation cancelled" );
		}
		else
		{
			Debug.Log( "Seçilen dosya: " + path );
			ResmiKusaAta( path );
			
			// Save selection
			// Seçimi kaydet
			PlayerPrefs.SetString( KAYIT_ANAHTARI, path );
			PlayerPrefs.Save();
		}
	}

	public void ResmiSifirla()
	{
		// Delete record / Kaydı sil
		PlayerPrefs.DeleteKey( KAYIT_ANAHTARI );
		
		if( kuS_Renderer != null && orjinalSprite != null )
		{
			// Restore original sprite / Orijinal sprite'ı geri yükle
			kuS_Renderer.sprite = orjinalSprite;
			
			// Reset scale (assuming 1,1,1 default)
			// Boyutu (Scale) sıfırla (1,1,1 varsayılan kabul ediyoruz)
			kuS_Renderer.transform.localScale = Vector3.one;

			// Update collider (return to original state)
			// Collider'ı da güncelle (eski haline dönsün)
			ColliderGuclle();
		}
	}

	// Combined image loading and scaling in one function
	// Resmi yükleme ve scale işlemini tek fonksiyonda topladık
	private void ResmiKusaAta( string path )
	{
		// Load image and make Sprite / Resmi yükle ve Sprite yap
		Texture2D texture = null;
		byte[] fileData = File.ReadAllBytes( path );
		if( fileData != null && fileData.Length > 0 )
		{
			texture = new Texture2D( 2, 2 );
			texture.LoadImage( fileData ); // Auto resizes / Otomatik boyutlandırır
		}
		
		if( texture == null )
		{
			Debug.LogError( "Resim yüklenemedi: " + path );
			return;
		}

		// Create Sprite / Sprite oluştur
		// Set pivot to center (0.5, 0.5) / Pivotu (0.5, 0.5) yaparak merkeze alıyoruz
		Sprite yeniSprite = Sprite.Create( texture, new Rect( 0, 0, texture.width, texture.height ), new Vector2( 0.5f, 0.5f ) );
		
		if( kuS_Renderer != null )
		{
			kuS_Renderer.sprite = yeniSprite;
			
			// --- SIZE ADJUSTMENT / BOYUT AYARLAMA ---
			// Reset scale first to measure original size
			// Önce objenin scale'ini sıfırla ki orjinal boyutunu ölçelim
			kuS_Renderer.transform.localScale = Vector3.one;

			// Get world space size of the new sprite
			// Yeni sprite'ın world space boyutunu al
			Vector2 yeniBoyut = kuS_Renderer.bounds.size;

			// Scale based on initial size (Calculate X and Y separately to Stretch)
			// Başlangıç boyutuna göre oranla (Gerdirmek için X ve Y'yi ayrı ayrı hesapla)
			float genislikOrani = baslangicBoyutu.x / yeniBoyut.x;
			float yukseklikOrani = baslangicBoyutu.y / yeniBoyut.y;
			
			// NEW METHOD (Stretch): Fit image exactly inside the box
			// YENİ YÖNTEM (Stretch - Gerdirme)
			// Resmi tam olarak kutunun içine sığdırıyoruz
			kuS_Renderer.transform.localScale = new Vector3( genislikOrani, yukseklikOrani, 1f );

			// --- UPDATE COLLIDER / COLLIDER GÜNCELLEME ---
			// Since sprite changed and scale reduced, we must update collider to new sprite's
			// original dimensions so it stays normal size when multiplied by scale.
			// Sprite değiştiği ve scale küçüldüğü için collider'ı da yeni sprite'ın
			// orjinal boyutlarına (büyük haline) güncellemeliyiz ki
			// scale ile çarpılınca normal boyutta kalsın.
			ColliderGuclle();
		}
	}

	private void ColliderGuclle()
	{
		// 1. If PolygonCollider2D exists (safest, gets exact shape)
		// 1. PolygonCollider2D varsa (en garantisi bu, şekli tam alır)
		PolygonCollider2D polyCol = kuS_Renderer.GetComponent<PolygonCollider2D>();
		if (polyCol != null)
		{
			Destroy(polyCol); // Delete old / Eskisini sil
			kuS_Renderer.gameObject.AddComponent<PolygonCollider2D>(); // Add new (Auto shape) / Yenisini ekle (Otomatik şekil alır)
			return; // Exit / Çıksın
		}

		// 2. If BoxCollider2D exists / 2. BoxCollider2D varsa
		BoxCollider2D boxCol = kuS_Renderer.GetComponent<BoxCollider2D>();
		if (boxCol != null)
		{
			boxCol.size = kuS_Renderer.sprite.bounds.size;
			boxCol.offset = kuS_Renderer.sprite.bounds.center;
		}

		// 3. If CircleCollider2D exists / 3. CircleCollider2D varsa
		CircleCollider2D circleCol = kuS_Renderer.GetComponent<CircleCollider2D>();
		if (circleCol != null)
		{
			// Set radius to half of the largest dimension
			// En büyük kenarın yarısını yarıçap yapalım
			float maxBoyut = Mathf.Max(kuS_Renderer.sprite.bounds.size.x, kuS_Renderer.sprite.bounds.size.y);
			circleCol.radius = maxBoyut * 0.5f;
			circleCol.offset = kuS_Renderer.sprite.bounds.center;
		}
	}
}
