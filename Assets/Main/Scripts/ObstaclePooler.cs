using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooler : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject obstaclePrefab;
    public int poolSize = 6;// Havuzda kac engel bekleyecek

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;// Kac saniyede bir engel cikacak
    public float spawnX = 10f;// Engellin cikacagi X ekseni
    public float minY = -2f;
    public float maxY = 2f;

    [Header("Movement Settings")]
    public float moveSpeed = 2.5f; // Engellerin sola kayma hizi
    public float despawnX = -12f; // Bu X'in soluna gecince havuza geri doner

    [Header("Star Settings")]
    public GameObject starPrefab; // yildiz prefabi buraya
    public int starPoolSize = 3;// Havuzda kac yildiz bekleyecek
    public int starSpawnEvery = 5;// kac engelde bir yildiz cikacak
    public float starMinY = -1.5f;
    public float starMaxY = 1.5f;

    private List<GameObject> pool = new List<GameObject>();

    // yildiz havuzu
    private List<GameObject> starPool = new List<GameObject>();

    // kac engel spawn ettik sayaci
    private int spawnCount = 0;

    void Start()
    {
        // oyun baslarken 6 engeli onceden al
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        // yildiz havuzunu olusturduk
        for (int i = 0; i < starPoolSize; i++)
        {
            GameObject star = Instantiate(starPrefab);
            star.SetActive(false);
            starPool.Add(star);
        }

        // Coroutine :)
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        // aktif engelleri sola kaydır
        foreach (GameObject obstacle in pool)
        {
            if (!obstacle.activeSelf) continue;

            // sola dogru hareket
            obstacle.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            // havuza geri gonderme
            if (obstacle.transform.position.x < despawnX)
            {
                obstacle.SetActive(false); // kapatiyoz
            }
        }

        // aktif yildizlari sola kaydır
        foreach (GameObject star in starPool)
        {
            if (!star.activeSelf) continue;

            star.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if (star.transform.position.x < despawnX)
            {
                star.SetActive(false); // kapatiyoruz
            }
        }
    }

    // hocam IEnumeratoru unutmustum youtube dan baktim
    IEnumerator SpawnRoutine()
    {
        // oyun devam ettigi surece dongu calisir
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // Bekle
            SpawnObstacle();// Engel cikar

            // her 5 engelde bir yildiz cikar
            if (spawnCount % starSpawnEvery == 0 && spawnCount != 0)
            {
                SpawnStar();
            }
        }
    }

    // Havuzdan bos bir engel al ve konumlandır ve aktif et
    void SpawnObstacle()
    {
        // havuzda aktif olmayan engel ara
        foreach (GameObject obstacle in pool)
        {
            if (!obstacle.activeSelf)
            {
                // Rastgele Y belirleme
                float randomY = Random.Range(minY, maxY);
                obstacle.transform.position = new Vector3(spawnX, randomY, 0f);

                obstacle.SetActive(true); // Uyandır
                spawnCount++; // sayaci artir
                FindFirstObjectByType<PlayerController>()?.OnObstacleSpawned(); // Playeri bul, engel spawn oldugunu bildir
                return; // donguden cik
            }
        }

        Debug.LogWarning("ObstaclePooler: Biyerde hata var (engel)");
    }

    // yildiz havuzundan alma
    void SpawnStar()
    {
        foreach (GameObject star in starPool)
        {
            if (!star.activeSelf)
            {
                float randomY = Random.Range(starMinY, starMaxY);
                star.transform.position = new Vector3(spawnX + 3f, randomY, 0f);

                star.SetActive(true);
                return;
            }
        }

        Debug.LogWarning("ObstaclePooler: Biyerde hata var (yildiz)");
    }
}