using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Flappy Ayarlari")]
    public float flapForce = 5f; // zip zip gucu

    [Header("Jetpack Ayarlari")]
    public float jetpackThrust = 15f; // jetpack gucu
    public float maxUpSpeed = 8f; // max yukari cikma hizi
    public int jetpackDuration = 5; // kac engel sonra jetpack bitsin

    [Header("Gorsel Referanslar")]
    public GameObject jetpackObject; // roket buraya
    public GameObject fireObject; // ates gorselini buraya

    [Header("Ses")]
    public AudioSource jetpackMusic; // muzigi buraya

    private Rigidbody2D rb;
    private bool isJetpackMode = false;
    private bool isThrusting = false;
    private int jetpackObstacleCount = 0; // jetpack acikken kac engel gectigi

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // baslangicta roket ve ates kapali olcak
        if (jetpackObject != null) jetpackObject.SetActive(false);
        if (fireObject != null) fireObject.SetActive(false);
    }

    void Update()
    {
        if (!isJetpackMode)
        {
            // normal hali: space basinca zipla
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // mevcut Y hizini sifirla ki ziplama birden dursun
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            // jetpack hali: basili tutunca yukari cik
            isThrusting = Input.GetKey(KeyCode.Space);

            // ates gorselini space basiliyken goster
            if (fireObject != null)
                fireObject.SetActive(isThrusting);
        }
    }

    void FixedUpdate()
    {
        // jetpack itme kuvveti fiziki
        if (isJetpackMode && isThrusting)
        {
            rb.AddForce(Vector2.up * jetpackThrust);

            // max hizi gecmesin
            if (rb.linearVelocity.y > maxUpSpeed)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxUpSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Carpisme oldu: " + other.tag);

        if (other.CompareTag("Star"))
        {
            AktivaJetpack();
            other.gameObject.SetActive(false); // yildizi kapa
        }

        // borulara carpinca game over
        if (other.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }

    // jetpack modunu ac
    void AktivaJetpack()
    {
        isJetpackMode = true;
        jetpackObstacleCount = 0;

        if (jetpackObject != null) jetpackObject.SetActive(true);

        if (jetpackMusic != null && !jetpackMusic.isPlaying)
            jetpackMusic.Play();

        // jetpack alininca hizi artir
        ObstaclePooler pooler = FindFirstObjectByType<ObstaclePooler>();
        if (pooler != null) pooler.moveSpeed += 1.5f;
    }

    // bu fonksiyon ObstaclePooler tarafindan her engel spawn olunca cagrilacak
    public void OnObstacleSpawned()
    {
        if (!isJetpackMode) return;

        jetpackObstacleCount++;

        // 5 engel gectiyse jetpack bitti
        if (jetpackObstacleCount >= jetpackDuration)
        {
            DeaktifJetpack();
        }
    }

    // jetpack modunu kapat
    void DeaktifJetpack()
    {
        isJetpackMode = false;
        isThrusting = false;

        if (jetpackObject != null) jetpackObject.SetActive(false);
        if (fireObject != null) fireObject.SetActive(false);

        if (jetpackMusic != null) jetpackMusic.Stop();

        // hizi eski haline dondur
        ObstaclePooler pooler = FindFirstObjectByType<ObstaclePooler>();
        if (pooler != null) pooler.moveSpeed -= 1.5f;
    }
}