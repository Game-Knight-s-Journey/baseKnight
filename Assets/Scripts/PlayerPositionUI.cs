using UnityEngine;
using TMPro;

public class PlayerPositionUI : MonoBehaviour
{
    public Transform playerTransform; // Kéo thả player vào đây
    [SerializeField] private TextMeshProUGUI positionText; // Text hiển thị vị trí

    // --- PHẦN MỚI ---
    public GameObject pauseMenuPanel; // Kéo Panel Tạm Dừng vào đây trong Inspector

    void Start()
    {
        // Đảm bảo game luôn bắt đầu ở tốc độ bình thường
        Time.timeScale = 1f;

        // --- PHẦN MỚI ---
        // Đảm bảo panel tạm dừng luôn được ẩn khi game bắt đầu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Chưa gán Pause Menu Panel trong Inspector!");
        }

        if (playerTransform != null)
        {
            UpdatePositionText(playerTransform.position);
        }
        else
        {
            Debug.LogError("Chưa gán Player Transform trong Inspector!");
        }
    }

    // Hàm mới cho nút "Tạm dừng & Lưu"
    public void PauseAndSavePosition()
    {
        if (playerTransform == null) return;

        // 1. Lưu vị trí của player
        Vector2 pos = playerTransform.position;
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.Save();
        Debug.Log("Đã lưu vị trí player: " + pos);
        UpdatePositionText(pos);

        // 2. Tạm dừng game
        Time.timeScale = 0f;
        Debug.Log("GAME ĐÃ TẠM DỪNG.");

        // --- PHẦN MỚI ---
        // 3. Hiển thị panel tạm dừng
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    // Hàm mới cho nút "Tiếp tục & Tải"
    public void ResumeAndLoadPosition()
    {
        if (playerTransform == null) return;

        // --- PHẦN MỚI ---
        // 1. Ẩn panel tạm dừng
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // 2. Tiếp tục game
        Time.timeScale = 1f;
        Debug.Log("GAME ĐÃ TIẾP TỤC.");

        // 3. Tải vị trí và di chuyển player
        float x = PlayerPrefs.GetFloat("PlayerPosX", playerTransform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerPosY", playerTransform.position.y);
        Vector2 loadedPos = new Vector2(x, y);

        playerTransform.position = new Vector3(x, y, playerTransform.position.z);
        Debug.Log("Đã tải vị trí player: " + loadedPos);
        UpdatePositionText(loadedPos);
    }

    // Cập nhật text hiển thị vị trí (không thay đổi)
    void UpdatePositionText(Vector2 pos)
    {
        if (positionText != null)
        {
            positionText.text = $"Vị trí player: X = {pos.x:F2}, Y = {pos.y:F2}";
        }
    }
}