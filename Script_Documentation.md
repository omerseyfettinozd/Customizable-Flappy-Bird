# Script Documentation / Script Dokümantasyonu

Below is the explanation of how each script in the `Assets/Script` folder works, both in English and Turkish.
Aşağıda, `Assets/Script` klasöründeki her bir scriptin nasıl çalıştığı hem İngilizce hem de Türkçe olarak açıklanmıştır.

---

## 1. BackgroundSelector.cs

**English:**
This script manages the game background. It allows players to select a custom image from their device to use as a background.
- It finds the `SpriteRenderer` of the background.
- It uses a file picker to let the user choose an image (`.png`, `.jpg`).
- It loads the selected image, creates a `Sprite` from it, and applies it to the background.
- It calculates the necessary scaling to stretch the image to fill the initial world size.
- It saves the selected image path using `PlayerPrefs` so it persists between sessions.
- It also has a "Reset" function to restore the original background.

**Türkçe:**
Bu script oyunun arkaplanını yönetir. Oyuncuların cihazlarından özel bir resim seçip arkaplan olarak kullanmasına olanak tanır.
- Arkaplanın `SpriteRenderer` bileşenini bulur.
- Kullanıcının bir resim (`.png`, `.jpg`) seçmesi için dosya seçici kullanır.
- Seçilen resmi yükler, ondan bir `Sprite` oluşturur ve arkaplana uygular.
- Resmi başlangıçtaki dünya boyutuna sığdırmak (stretch) için gerekli ölçeklemeyi hesaplar.
- Seçilen resim yolunu `PlayerPrefs` kullanarak kaydeder, böylece oyun kapatılıp açıldığında hatırlanır.
- Ayrıca orijinal arkaplana dönmek için bir "Sıfırla" fonksiyonu vardır.

---

## 2. BoruHareketi.cs

**English:**
This script handles the movement of pipes.
- It moves the pipe object to the left at a constant speed defined by `hiz` (speed).
- Check if the pipe has gone off-screen (past `yokOlmaSiniri`).
- If it is off-screen, it deactivates the object (`SetActive(false)`) instead of destroying it, allowing it to be reused by the Object Pool system.

**Türkçe:**
Bu script boruların hareketini yönetir.
- Boru nesnesini `hiz` değişkeniyle belirlenen sabit bir hızla sola doğru hareket ettirir.
- Borunun ekrandan çıkıp çıkmadığını (`yokOlmaSiniri`) kontrol eder.
- Eğer ekrandan çıktıysa, nesneyi yok etmek yerine pasif hale getirir (`SetActive(false)`), böylece Object Pool sistemi tarafından tekrar kullanılabilir.

---

## 3. BoruSpawner.cs

**English:**
This script spawns pipes at regular intervals using an Object Pooling system for performance.
- creates a pool of pipe objects at the start (`borular` array).
- It tracks time with a timer. When the time exceeds `sure` (interval), it spawns a pipe.
- Instead of `Instantiate`, it finds an inactive pipe from the pool, resets its position to the right side with a random Y height, and activates it.
- This prevents memory garbage collection spikes associated with frequent creating and destroying of objects.

**Türkçe:**
Bu script, performans için Object Pooling (Nesne Havuzu) sistemi kullanarak düzenli aralıklarla boru oluşturur.
- Başlangıçta bir boru havuzu (`borular` dizisi) oluşturur.
- Bir zamanlayıcı ile süreyi takip eder. Süre dolduğunda (`sure`) bir boru çağırır.
- `Instantiate` (yeni yaratma) yapmak yerine, havuzdaki pasif bir boruyu bulur, pozisyonunu rastgele bir Y yüksekliği ile sağ tarafa ayarlar ve aktif hale getirir.
- Bu yöntem, sürekli nesne yaratıp yok etmenin neden olduğu performans düşüşlerini (Garbage Collection) engeller.

---

## 4. GameManager.cs

**English:**
This is the core manager of the game. It handles game state, UI, scoring, and high scores.
- **Singleton:** Implements the Singleton pattern (`Instance`) so other scripts can access it easily.
- **Game Flow:** Manages Starting, Pausing (`Time.timeScale`), Restarting, and Game Over states.
- **Scoring:** Increases score when passing pipes and checks for high scores (`PlayerPrefs`).
- **UI:** Updates the score text, Game Over panel, and Start panel.
- **Leaderboard Integration:** Connects with `LeaderboardManager` to submit scores and display the leaderboard UI.

**Türkçe:**
Oyunun çekirdek yöneticisidir. Oyun durumunu, arayüzü (UI), puanlamayı ve yüksek skorları yönetir.
- **Singleton:** Diğer scriptlerin kolayca erişebilmesi için Singleton yapısını (`Instance`) uygular.
- **Oyun Akışı:** Başlatma, Duraklatma (`Time.timeScale`), Yeniden Başlatma ve Oyun Bitişi durumlarını yönetir.
- **Puanlama:** Borulardan geçince puanı artırır ve yüksek skoru kontrol eder (`PlayerPrefs`).
- **UI:** Skor yazısını, Oyun Sonu panelini ve Başlangıç panelini günceller.
- **Liderlik Tablosu:** Skorları göndermek ve sıralamayı göstermek için `LeaderboardManager` ile bağlantı kurar.

---

## 5. KusHareketi.cs

**English:**
Controls the bird's physics and input.
- **Physics:** Uses `Rigidbody2D` for gravity and physics.
- **Input:** Listens for Space key, Mouse Click, or Touch to make the bird jump (`ziplamaGucu`).
- **Rotation:** Rotates the bird's head up when flying up and down when falling, for a natural feel.
- **Collision:** Detects collisions with pipes/ground (triggers Game Over) and Score Zones (triggers Score Increase).

**Türkçe:**
Kuşun fiziğini ve kontrollerini yönetir.
- **Fizik:** Yerçekimi ve fizik işlemleri için `Rigidbody2D` kullanır.
- **Girdi:** Kuşu zıplatmak için (`ziplamaGucu`) Boşluk tuşunu, Fare Tıklamasını veya Dokunmayı dinler.
- **Dönüş:** Doğal bir his vermek için yükselirken kuşun kafasını yukarı, düşerken aşağı eğer.
- **Çarpışma:** Borulara/Zemine çarpışmayı (Oyun Biter) ve Skor Bölgelerinden geçişi (Puan Artar) algılar.

---

## 6. LeaderboardManager.cs

**English:**
Handles online high scores using the Dreamlo service.
- **Networking:** Uses `UnityWebRequest` to communicate with the Dreamlo web server.
- **Upload:** Uploads the player's name and score to the server.
- **Download:** Downloads the current high score list.
- **Events:** Triggers an event when data is downloaded so the UI can update.

**Türkçe:**
Dreamlo servisini kullanarak çevrimiçi yüksek skorları yönetir.
- **Ağ:** Dreamlo web sunucusuyla iletişim kurmak için `UnityWebRequest` kullanır.
- **Yükleme:** Oyuncunun ismini ve skorunu sunucuya yükler.
- **İndirme:** Mevcut yüksek skor listesini indirir.
- **Olaylar:** Veri indirildiğinde arayüzün (UI) güncellenebilmesi için bir olay (event) tetikler.

---

## 7. ResimSecici.cs

**English:**
Similar to BackgroundSelector, but for the Player (Bird).
- Handles selecting a custom image for the bird character.
- Loads the image, creates a sprite, and **stretches** it to fit the bird's original size exactly.
- Updates the `Collider` (hitbox) to match the new shape or size of the bird.
- Saves the selection so the custom bird persists.

**Türkçe:**
BackgroundSelector'a benzer, ancak Oyuncu (Kuş) içindir.
- Kuş karakteri için özel bir resim seçilmesini yönetir.
- Resmi yükler, bir sprite oluşturur ve kuşun orijinal boyutuna tam sığacak şekilde **gerdirir (stretch)**.
- `Collider`'ı (vuruş kutusunu) kuşun yeni şekline veya boyutuna uyacak şekilde günceller.
- Seçimi kaydeder, böylece özel kuş kalıcı olur.

---

## 8. WindowsFilePicker.cs

**English:**
A helper script to provide a native file selection dialog on Windows Standalone builds.
- Uses `DllImport` to call the Windows `comdlg32.dll` system library.
- displaying the standard "Open File" dialog window of Windows.
- It is only used when the game is running on Windows; mobile platforms use a different plugin.

**Türkçe:**
Windows Standalone sürümlerinde yerel dosya seçim penceresi sağlamak için yardımcı bir scripttir.
- Windows `comdlg32.dll` sistem kütüphanesini çağırmak için `DllImport` kullanır.
- Bu, Windows'un standart "Dosya Aç" penceresini görüntüler.
- Sadece oyun Windows üzerinde çalışırken kullanılır; mobil platformlar farklı bir eklenti kullanır.
