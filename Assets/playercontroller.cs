using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;    // 飛機前進速度

    public AudioSource audioSource;     // 音效播放器
    public AudioClip hitSound;          // 撞到障礙物音效
    public AudioClip passSound;         // 順利通過音效

    private bool isGameOver = false;    // 遊戲結束標記
    private Vector3 startPos;           // 記錄遊戲的初始原點

    void Start()
    {
        // 記錄飛機一開始在世界座標中的位置作為原點
        startPos = transform.position;
    }

    void Update()
    {
        if (isGameOver) return;

        // 1. 讓飛機持續往前飛
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 2. 取得滑鼠在螢幕上的位置
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 3. 讓飛機平滑跟隨滑鼠位置
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(newPosition.x, targetWorldPos.x, Time.deltaTime * 5f);
        newPosition.y = Mathf.Lerp(newPosition.y, targetWorldPos.y, Time.deltaTime * 5f);

        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        // 撞到障礙物
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("撞到障礙物！回到原點重新開始！");

            // 1. 播放撞擊音效
            if (hitSound && audioSource)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // 2. 將飛機瞬間移動回一開始的記錄點 (startPos)
            transform.position = startPos;

            // （如果你希望扣命或做其他重置邏輯，也可以加在這裡）
        }
        // 通過得分區域
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("順利通過障礙物！");
            if (passSound && audioSource)
            {
                audioSource.PlayOneShot(passSound);
            }
        }
    }
}