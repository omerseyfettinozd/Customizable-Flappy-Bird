# Customizable Flappy Bird / Özelleştirilebilir Flappy Bird

**[English]**
A customizable Flappy Bird clone made with Unity. This project allows players to upload their own images for the bird and the background, features a global leaderboard, and supports both PC and Mobile controls.

**[Türkçe]**
Unity ile yapılmış özelleştirilebilir bir Flappy Bird kopyası. Bu proje, oyuncuların kuş ve arka plan için kendi resimlerini yüklemelerine olanak tanır, küresel bir liderlik tablosu içerir ve hem Bilgisayar hem de Mobil kontrollerini destekler.

---

## Features / Özellikler

### English
- **Customization:**
  - **Custom Bird:** Upload any image from your device to use as the player character. The game automatically stretches and resizes it to fit the hitbox.
  - **Custom Background:** Change the game background to any image you like.
- **Leaderboard:** Integrated online leaderboard (Dreamlo) to compete with other players.
- **High Score:** Saves your best score locally.
- **Cross-Platform Controls:**
  - **PC:** Spacebar or Left Mouse Click to jump.
  - **Mobile:** Tap anywhere on the screen to jump.
- **Dynamic Physics:** The bird rotates based on its velocity (tilts up when jumping, down when falling).

### Türkçe
- **Özelleştirme:**
  - **Özel Kuş:** Oyuncu karakteri olarak kullanmak için cihazınızdan herhangi bir resim yükleyin. Oyun, vuruş kutusuna (hitbox) sığması için resmi otomatik olarak gerdirir ve boyutlandırır.
  - **Özel Arka Plan:** Oyun arka planını istediğiniz herhangi bir resimle değiştirin.
- **Liderlik Tablosu:** Diğer oyuncularla rekabet etmek için entegre çevrimiçi liderlik tablosu (Dreamlo).
- **Yüksek Skor:** En iyi skorunuzu yerel olarak kaydeder.
- **Çapraz Platform Kontrolleri:**
  - **PC:** Zıplamak için Boşluk tuşu veya Sol Fare Tıklaması.
  - **Mobil:** Zıplamak için ekranda herhangi bir yere dokunun.
- **Dinamik Fizik:** Kuş, hızına bağlı olarak döner (zıplarken yukarı, düşerken aşağı eğilir).

---

## How to Play / Nasıl Oynanır

**[EN]**
1. **Start:** Click "Start" or tap the screen to begin.
2. **Controls:** Press Space, Click, or Tap to flap your wings and jump. Avoid the pipes!
3. **Customize:**
   - In the menu, click the "Select Bird" button to choose a custom sprite.
   - Click "Select Background" to choose a custom background.
4. **Leaderboard:** Enter your name and submit your score to see where you rank globally.

**[TR]**
1. **Başlat:** Başlamak için "Start"a tıklayın veya ekrana dokunun.
2. **Kontroller:** Kanat çırpmak ve zıplamak için Boşluk tuşuna basın, Tıklayın veya Dokunun. Borulardan kaçının!
3. **Özelleştir:**
   - Menüde, özel bir karakter seçmek için "Kuş Seç" (Select Bird) butonuna tıklayın.
   - Özel bir arka plan seçmek için "Arka Plan Seç" (Select Background) butonuna tıklayın.
4. **Liderlik Tablosu:** İsminizi girin ve dünya çapındaki sıralamanızı görmek için skorunuzu gönderin.

---

## Project Structure / Proje Yapısı

The main scripts are located in `Assets/Script`:
Ana scriptler `Assets/Script` klasöründe bulunur:

- `GameManager.cs`: Core game logic (Singleton). / Temel oyun mantığı.
- `KusHareketi.cs`: Bird physics and controls. / Kuş fiziği ve kontrolleri.
- `BoruSpawner.cs`: Object pooling for pipes. / Borular için nesne havuzu.
- `LeaderboardManager.cs`: Network logic for high scores. / Yüksek skorlar için ağ mantığı.
- `ResimSecici.cs`: Logic for loading custom bird images. / Özel kuş resimlerini yükleme mantığı.
- `BackgroundSelector.cs`: Logic for loading custom backgrounds. / Özel arka planları yükleme mantığı.

---

## Installation / Kurulum

1. Clone this repository. / Bu depoyu klonlayın.
2. Open with Unity (Recommended version: 2022.3 or later). / Unity ile açın (Önerilen sürüm: 2022.3 veya üzeri).
3. Open the main scene in `Assets/Scenes`. / `Assets/Scenes` içindeki ana sahneyi açın.
4. Press Play! / Oynat'a basın!

---

*Developed by Ömer Seyfettin*
