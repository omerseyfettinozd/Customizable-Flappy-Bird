# Nasıl GitHub'a Oyun Yüklenir (Release Oluşturma)

Android ve Windows sürümlerini oluşturup (Build alıp) GitHub'a yüklemek için aşağıdaki adımları takip et.

## 1. Adım: Build Alma (Unity)

### Windows İçin (Bilgisayar)
1.  Unity'de sol üstten **File > Build Settings** menüsüne git.
2.  Platform olarak **Windows, Mac, Linux** seçili olsun.
3.  **Build** butonuna bas.
4.  Masaüstünde veya proje içinde yeni bir klasör aç (Adı `WindowsBuild` olabilir) ve orayı seç.
5.  **ÖNEMLİ:** Build işlemi bitince klasörde bir `.exe` dosyası (oyun simgesi) ve yanında bir klasör (`_Data` ile biten) göreceksin.
6.  **Bu .exe dosyasını tek başına atarsan oyun ÇALIŞMAZ!** Yanındaki klasörle birlikte olması gerekir.
7.  Oluşan dosyaların hepsini seç -> Sağ Tıkla -> **Sıkıştır (ZIP) / Arşive Ekle**.
8.  Oluşan `.zip` dosyasına isim ver: `FlappyBird_Windows.zip`.
    *   *GitHub'a yükleyeceğin dosya bu ZIP dosyası olacak.*

### Android İçin (Tele)
1.  **File > Build Settings** menüsüne git.
2.  **Android** platformunu seç ve **Switch Platform** de (Eğer seçili değilse).
3.  **Build** butonuna bas.
4.  Dosya adı olarak örneğin `FlappyBird.apk` yaz ve kaydet.
5.  Eline tek bir `.apk` dosyası geçecek.

---

## 2. Adım: GitHub'a Yükleme (Release)

1.  GitHub proje sayfana git: [https://github.com/omerseyfettinozd/Customizable-Flappy-Bird](https://github.com/omerseyfettinozd/Customizable-Flappy-Bird)
2.  Sağ taraftaki menüde **Releases** başlığını bul (yoksa `Create a new release` yazar). Tıkla.
3.  **Draft a new release** (veya Create new release) butonuna bas.
4.  **Choose a tag** (Etiket seç): Buraya bir versiyon numarası yaz (örneğin: `v1.0`) ve çıkan "Create new tag" yazısına tıkla.
5.  **Release title** (Başlık): "v1.0 - İlk Sürüm" gibi bir başlık at.
6.  **Describe this release** (Açıklama): İstersen "Windows ve Android sürümleri eklendi." yazabilirsin.
7.  **Attach binaries by dropping them here** (Dosyaları buraya sürükle):
    *   Hazırladığın `FlappyBird_Windows.zip` dosyasını sürükleyip buraya bırak.
    *   Hazırladığın `FlappyBird.apk` dosyasını sürükleyip buraya bırak.
    *   Yükleme çubuklarının dolmasını bekle.
8.  En alttaki yeşil **Publish release** butonuna bas.

Tebrikler! Artık o sayfaya giren herkes oyununun `.exe` (zip içinde) ve `.apk` dosyalarını indirip oynayabilir.
