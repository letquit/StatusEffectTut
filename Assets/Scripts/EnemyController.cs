using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Vector2 StartPosition = new Vector2(-5f, 0f); // 左侧起点
    [SerializeField] private Vector2 EndPosition = new Vector2(5f, 0f);   // 右侧终点
    [SerializeField] private float LerpSpeed = 2f;                         // 移动速度

    private float t = 0f;
    private bool movingForward = true;

    void Update()
    {
        if (movingForward)
        {
            t += Time.deltaTime * LerpSpeed;
            if (t >= 1f)
            {
                t = 1f;
                movingForward = false;
            }
        }
        else
        {
            t -= Time.deltaTime * LerpSpeed;
            if (t <= 0f)
            {
                t = 0f;
                movingForward = true;
            }
        }

        transform.position = Vector3.Lerp(
            new Vector3(StartPosition.x, StartPosition.y, transform.position.z),
            new Vector3(EndPosition.x, EndPosition.y, transform.position.z),
            t
        );
    }
}
