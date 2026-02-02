using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için
using TMPro; // TextMeshPro kullanacağız

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance / Singleton örneği

    public TextMeshProUGUI scoreText; // Text to display score / Puanı gösteren text
    public GameObject gameOverPanel; // Panel to open when game over / Oyun bitince açılacak panel
    public GameObject startPanel; // Panel to show on game start / Oyun açılınca gelecek panel

    // Scores on Game Over Panel / Game Over Panelindeki Skorlar
    public TextMeshProUGUI gameOverScoreText; // Game over score / Oyun sonu skoru
    public TextMeshProUGUI bestScoreText; // Best score / En yüksek skor

    // --- LEADERBOARD UI ---
    public GameObject leaderboardUI; // Panel with name input and list / İsim girme ve listenin olduğu panel
    public TMP_InputField nameInput; // Name input field / İsim girme kutusu
    public TextMeshProUGUI leaderboardListText; // Leaderboard list text / Sıralama listesi yazısı
    public TextMeshProUGUI playerRankText; // Text showing player's own rank / Oyuncunun kendi sırasını gösteren yazı
    public GameObject submitButtonObj; // Submit button (to hide) / Gönder butonu (gizlemek için)

    private int score = 0; // Current score / Mevcut puan
    
    // Should the game start immediately or show menu when restarted?
    // Oyunun tekrar başladığında direkt mi başlasın yoksa menü mü gelsin?
    private static bool autoStart = false;

    void Awake()
    {
        // Singleton pattern: To access GameManager.Instance from anywhere
        // Singleton yapısı: Her yerden GameManager.Instance diye ulaşabilmek için
        if (Instance == null)
        {
            Instance = this;
        }

        // Performance Setting: Lock FPS to 300
        // This allows the game to run at max fluidity supported by device (60Hz, 90Hz, 120Hz).
        // Performans Ayarı: FPS Kilidi 300
        // Bu sayede oyun 60Hz, 90Hz veya 120Hz ekranlarda cihazın desteklediği maksimum akıcılıkta çalışır.
        Application.targetFrameRate = 300;
    }

    void Start()
    {
        // Hide panel / Paneli gizle
        if(gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // If restart was requested, start game immediately
        // Eğer yeniden başlat dendiyse direkt oyuna gir
        if (autoStart)
        {
            StartGame();
        }
        else
        {
            // Otherwise, show start panel and pause game
            // Değilse, start panelini göster ve oyunu durdur
            if (startPanel != null)
            {
                startPanel.SetActive(true);
                Time.timeScale = 0; // Pause game / Oyunu durdur
            }
            else
            {
                // Start immediately if no start panel
                // Start panel yoksa direkt başla
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        // Hide start panel / Start panelini gizle
        if (startPanel != null)
            startPanel.SetActive(false);

        // Start game / Oyunu başlat
        Time.timeScale = 1;
        
        // No need to reset score etc since scene is freshly loaded
        // Puanı vs sıfırlamaya gerek yok zaten sahne yeni yüklendi
    }

    public void ScoreArtir()
    {
        score++; // Increase score / Puanı artır
        // Write score to screen
        // Skoru ekrana yaz
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void GameOver()
    {
        // Pause game / Oyunu durdur
        Time.timeScale = 0;

        // --- BEST SCORE LOGIC / EN YÜKSEK SKOR MANTIĞI ---
        // Get saved best score (Default 0)
        // Kayıtlı en yüksek skoru al (Yoksa 0)
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // If current score beats the record
        // Eğer şu anki skor rekoru geçtiyse
        if (score > bestScore)
        {
            bestScore = score; // New record / Yeni rekor
            PlayerPrefs.SetInt("BestScore", bestScore); // Save / Kaydet
            PlayerPrefs.Save();
        }

        // --- UI UPDATE / UI GÜNCELLEME ---
        // If text fields in Game Over panel are assigned, update them
        // Eğer Game Over panelindeki textler atanmışsa yaz
        if (gameOverScoreText != null)
            gameOverScoreText.text = "SCORE: " + score.ToString();
            
        if (bestScoreText != null)
            bestScoreText.text = "BEST: " + bestScore.ToString();

        // Open panel / Paneli aç
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Quick restart (Start panel will not show)
        // Hızlı yeniden başlatma (Start paneli gelmez)
        autoStart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        // Return to main menu (Start panel will show)
        // Ana menüye dön (Start paneli gelir)
        autoStart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- LEADERBOARD FUNCTIONS ---

    public void OpenLeaderboard()
    {
        if (leaderboardUI != null)
        {
            leaderboardUI.SetActive(true);
            
            // Get saved name and fill the input field
            // Kayıtlı ismi getir ve kutucuğa yaz
            if(nameInput != null)
            {
                nameInput.text = PlayerPrefs.GetString("PlayerName", "");
            }

            // Download current list / Mevcut listeyi indir
            LeaderboardManager.Instance.DownloadHighscores();
            // Subscribe to event / Event'e abone ol
            LeaderboardManager.Instance.highscoresDownloaded += OnLeaderboardDownloaded;
        }
    }

    public void SubmitScoreToLeaderboard()
    {
        if (nameInput != null && !string.IsNullOrEmpty(nameInput.text))
        {
            // Save name so user doesn't have to type again
            // İsmi kaydet ki sonra tekrar yazmasın
            PlayerPrefs.SetString("PlayerName", nameInput.text);
            PlayerPrefs.Save();

            // SUBMIT BEST SCORE / EN YÜKSEK SKORU GÖNDER
            // Compare saved record and current score, send the bigger one just in case
            // Garanti olsun diye: Hem kayıtlı rekoru hem şu anki puanı kıyasla, büyüğü gönder
            int savedBest = PlayerPrefs.GetInt("BestScore", 0);
            int scoreToSend = Mathf.Max(score, savedBest);
            
            Debug.Log("Submitting Score: " + scoreToSend + " (Local: " + score + ", Saved: " + savedBest + ")");
            LeaderboardManager.Instance.AddNewHighscore(nameInput.text, scoreToSend);
            
            // Hide button to prevent double submit / Butonu kapat ki tekrar basamasın
            if(submitButtonObj != null) submitButtonObj.SetActive(false);
        }
    }

    void OnLeaderboardDownloaded(Highscore[] highscoreList)
    {
        if (leaderboardListText != null)
        {
            leaderboardListText.text = "";
            // Show top 10 / İlk 10 kişiyi göster
            for (int i = 0; i < highscoreList.Length; i++)
            {
                if (i >= 10) break;
                
                leaderboardListText.text += (i + 1) + ". " + highscoreList[i].username + " - " + highscoreList[i].score + "\n";
            }
        }

        // --- PLAYER'S OWN RANK / OYUNCUNUN KENDİ SIRASI ---
        if (playerRankText != null)
        {
            string currentPlayerName = PlayerPrefs.GetString("PlayerName", "");
            bool found = false;

            if (!string.IsNullOrEmpty(currentPlayerName))
            {
                for (int i = 0; i < highscoreList.Length; i++)
                {
                    if (highscoreList[i].username == currentPlayerName)
                    {
                        // Found the rank / Sıralamayı bulduk
                        playerRankText.text = "Your Rank: " + (i + 1) + ". (" + highscoreList[i].score + " Pts)";
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                playerRankText.text = "Not Ranked Yet...";
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe / Event aboneliğini kaldır
        if (LeaderboardManager.Instance != null)
            LeaderboardManager.Instance.highscoresDownloaded -= OnLeaderboardDownloaded;
    }
}
