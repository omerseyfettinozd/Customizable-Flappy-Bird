using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    // Dreamlo Private Code (WRITE ACCESS / YAZMA İZNİ)
    private const string privateCode = "write private code here";
    // Dreamlo Public Code (READ ACCESS / OKUMA İZNİ)
    // Since we have private code, we can read too. 
    // Eğer private code varsa okuma da yapabiliriz, public code şart değil ama güvenlik için iyidir.
    private const string publicCode = "write public code here"; // Derived or empty / URL'den tahmin edilen veya boş bırakılan
    // Note: Public code is usually short in "Public Code" on website.
    // However, can also read with Private code: http://dreamlo.com/lb/PRIVATE_CODE/json
    // Not: Normalde Public code web sitesinde "Public Code" diye kısa yazar. 
    // Ancak Private code ile de okuma yapılabilir: http://dreamlo.com/lb/PRIVATE_CODE/json
    
    private const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList; // Leaderboard list / Sıralama listesi

    // Event to notify when leaderboard is downloaded
    // Olay yönetimi için delegate (List yüklendiğinde haber ver)
    public delegate void OnHighscoresDownloaded(Highscore[] highscoreList);
    public event OnHighscoresDownloaded highscoresDownloaded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewHighscore(string username, int score)
    {
        // Send score / Skoru gönder
        StartCoroutine(UploadNewHighscore(username, score));
    }

    public void DownloadHighscores()
    {
        // Download list / Listeyi indir
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        // URL: http://dreamlo.com/lb/PRIVATE_CODE/add/NAME/SCORE
        // Replace spaces with + to avoid issues
        // İsimdeki boşlukları + yapalım, sorun çıkmasın
        string cleanName = username.Replace(" ", "+");
        
        // Create URL / URL oluştur
        string requestURL = webURL + privateCode + "/add/" + cleanName + "/" + score;
        
        using (UnityWebRequest www = UnityWebRequest.Get(requestURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Score Upload Error: " + www.error);
            }
            else
            {
                Debug.Log("Score Upload Successful! / Skor başarıyla yüklendi!");
                
                // Wait a bit for server to process data
                // Sunucunun veriyi işlemesi için çok kısa bekle
                yield return new WaitForSeconds(0.5f);
                
                // Update list after upload
                // Yükleme bitince listeyi güncelle
                DownloadHighscores();
            }
        }
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        // URL: http://dreamlo.com/lb/PRIVATE_CODE/pipe (We fetch in Pipe format)
        // Public code is better if available but private code works too.
        // URL: http://dreamlo.com/lb/PRIVATE_CODE/pipe (Pipe formatında veri çekiyoruz)
        // Public code varsa onu kullanmak daha iyidir ama private code da çalışır.
        string requestURL = webURL + privateCode + "/pipe";

        using (UnityWebRequest www = UnityWebRequest.Get(requestURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Leaderboard Download Error: " + www.error);
            }
            else
            {
                Debug.Log("Leaderboard Downloaded! / Liste indirildi!");
                FormatHighscores(www.downloadHandler.text);
            }
        }
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0].Replace("+", " ");
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
        }

        // Invoke event to update UI
        // Action varsa çalıştır (UI güncellemek için)
        if (highscoresDownloaded != null)
            highscoresDownloaded(highscoresList);
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}
