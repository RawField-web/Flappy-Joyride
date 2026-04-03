using UnityEngine;
public class ScoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // kus gecince skoru artir
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<ScoreManager>()?.AddScore();
        }
    }
}
