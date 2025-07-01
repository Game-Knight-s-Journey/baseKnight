using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Thêm vào
using System;                   // Thêm vào
using System.Linq;              // Thêm vào
using System.Text;
public class GameManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    //[SerializeField] private TextMeshProUGUI highScoresListText; // Dùng cho cả Win và Lose
    [SerializeField] private TextMeshProUGUI finalScoreText; // Hiển thị điểm khi kết thúc game
    [SerializeField] private TextMeshProUGUI highScoreText;  // Hiển thị điểm cao nhất
    [SerializeField] private TextMeshProUGUI finalScoreTextW; // Hiển thị điểm khi kết thúc game
    [SerializeField] private TextMeshProUGUI highScoreTextW;  // Hiển thị điểm cao nhất
                                                              // BIẾN MỚI
    [SerializeField] private TextMeshProUGUI highScoresListTextGO; // UI hiển thị danh sách 10 điểm (Game Over)
    [SerializeField] private TextMeshProUGUI highScoresListTextW;  // UI hiển thị danh sách 10 điểm (Game Win)
    private bool isGameOver = false;
    private bool isGameWin = false;
    // Hằng số để quản lý việc lưu trữ
    private const string HighScoresKey = "HighScoresData"; // Đổi tên key để tránh xung đột với dữ liệu cũ
    private const int MaxScoresToKeep = 10; // Giới hạn số lượng điểm lưu trữ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Khởi tạo lại trạng thái khi bắt đầu scene
        Time.timeScale = 1;
        score = 0;
        isGameOver = false;
        isGameWin = false;

        UpdateScore();
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);

        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            Debug.Log("Điểm cao nhất đã lưu: " + highScore);
        }
        else
        {
            Debug.Log("Chưa có điểm cao nhất được lưu.");
        }

        LoadHighScore();
        UpdateHighScoreUI();
    }


    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UpdateScore();
        }

    }
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
        Debug.Log("Hien thi + 1 diem");
    }
    // *** HÀM MỚI: Tải dữ liệu từ PlayerPrefs ***
    private HighScoreData LoadHighScoresData()
    {
        string json = PlayerPrefs.GetString(HighScoresKey, "{}");
        HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
        if (data.allScores == null)
        {
            data.allScores = new List<ScoreEntry>();
        }
        return data;
    }
    // *** HÀM MỚI: Thêm điểm mới và lưu lại ***
    private void AddAndSaveScore(int newScore)
    {
        HighScoreData data = LoadHighScoresData();

        ScoreEntry newEntry = new ScoreEntry
        {
            score = newScore,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // Thêm giờ:phút:giây để sắp xếp chính xác hơn
        };

        data.allScores.Add(newEntry);

        // Điều này đảm bảo 10 mục đầu tiên luôn là 10 lần chơi gần nhất
        data.allScores = data.allScores.OrderByDescending(entry => entry.date).ToList();

        // Giới hạn số lượng điểm lưu trong danh sách (giữ lại 50 lần chơi gần nhất)
        if (data.allScores.Count > MaxScoresToKeep)
        {
            data.allScores.RemoveRange(MaxScoresToKeep, data.allScores.Count - MaxScoresToKeep);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(HighScoresKey, json);
        PlayerPrefs.Save();
    }
    // *** HÀM MỚI: Hiển thị điểm cao nhất lên UI ***
    private void DisplayHighScoreInfo()
    {
        // 1. Hiển thị điểm của lần chơi hiện tại
        if (finalScoreText != null) finalScoreText.text = "Điểm của bạn: " + score;
        if (finalScoreTextW != null) finalScoreTextW.text = "Điểm của bạn: " + score;

        // 2. Tìm và hiển thị điểm cao nhất từ dữ liệu đã lưu
        HighScoreData data = LoadHighScoresData();
        string highScoreInfo = "Chưa có điểm cao";

        if (data.allScores.Count > 0)
        {
            // Tìm mục có điểm số lớn nhất
            ScoreEntry highestScoreEntry = data.allScores.OrderByDescending(entry => entry.score).FirstOrDefault();

            if (highestScoreEntry != null)
            {
                // Chuyển đổi ngày sang định dạng dd/MM/yyyy để hiển thị
                DateTime entryDate = DateTime.Parse(highestScoreEntry.date);
                string displayDate = entryDate.ToString("dd/MM/yyyy");
                highScoreInfo = $"Điểm cao nhất: {highestScoreEntry.score} ({displayDate})";
            }
        }

        // Cập nhật lên các Text UI
        if (highScoreText != null) highScoreText.text = highScoreInfo;
        if (highScoreTextW != null) highScoreTextW.text = highScoreInfo;

        // PHẦN 2: LOGIC MỚI - HIỂN THỊ DANH SÁCH 10 LẦN CHƠI GẦN NHẤT
        // ===================================================================

        StringBuilder builder = new StringBuilder("5 Lần Chơi Gần Nhất\n\n");

        if (data.allScores.Count == 0)
        {
            builder.Append("Chưa có lịch sử chơi.");
        }
        else
        {
            // Vì danh sách đã được sắp xếp theo ngày, chỉ cần lấy 10 mục đầu tiên
            int displayCount = Mathf.Min(data.allScores.Count, 5);
            for (int i = 0; i < displayCount; i++)
            {
                ScoreEntry entry = data.allScores[i];
                DateTime entryDate = DateTime.Parse(entry.date);
                string displayDate = entryDate.ToString("dd/MM/yyyy");

                builder.AppendLine($"{entry.score} điểm - {displayDate}");
            }
        }

        // Cập nhật lên các Text UI mới
        if (highScoresListTextGO != null) highScoresListTextGO.text = builder.ToString();
        if (highScoresListTextW != null) highScoresListTextW.text = builder.ToString();
    }
    private void LoadHighScore()
    {
        // Lấy điểm cao nhất đã lưu, mặc định 0 nếu chưa có
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Điểm cao nhất: " + highScore;

        highScoreTextW.text = "Điểm cao nhất: " + highScore;
    }
    private void UpdateHighScoreUI()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            highScoreText.text = "Điểm cao nhất: " + score;
            highScoreTextW.text = "Điểm cao nhất: " + score;
        }
    }

    private void ShowFinalScore()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Điểm của bạn: " + score;
        if (finalScoreTextW != null)
            finalScoreTextW.text = "Điểm của bạn: " + score;

        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "Điểm cao nhất: " + highScore;
        }
        if (highScoreTextW != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreTextW.text = "Điểm cao nhất: " + highScore;
        }
    }
    public void GameOver()
    {
        isGameOver = true;
        UpdateHighScoreUI();
        ShowFinalScore();
        Time.timeScale = 0;
        AddAndSaveScore(score);    // Lưu điểm hiện tại vào danh sách
        DisplayHighScoreInfo();    // Hiển thị thông tin điểm cao nhất
        gameOverUi.SetActive(true);
        gameWinUi.SetActive(false);

    }
    public void GameWin()
    {
        isGameWin = true;
        UpdateHighScoreUI();
        ShowFinalScore();
        Time.timeScale = 0;
        AddAndSaveScore(score);    // Lưu điểm hiện tại vào danh sách
        DisplayHighScoreInfo();    // Hiển thị thông tin điểm cao nhất
        gameWinUi.SetActive(true);
        gameOverUi.SetActive(false);

    }
    public void RestarGame()
    {
        // Chỉ cần load lại scene là đủ, Start() sẽ xử lý việc reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GotoMenu()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public bool IsGameOver() => isGameOver;

    public bool IsGameWin() => isGameWin;
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HighScoresKey); // Xóa key dữ liệu mới
        PlayerPrefs.Save();

        // Cập nhật lại UI để hiển thị trạng thái rỗng
        if (highScoreText != null) highScoreText.text = "Chưa có điểm cao";
        if (highScoreTextW != null) highScoreTextW.text = "Chưa có điểm cao";

        Debug.Log("Đã xóa toàn bộ dữ liệu điểm cao.");
    }

}