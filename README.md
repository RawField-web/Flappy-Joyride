# Flappy JoyRide

Beykoz Universitesi - Dijital Oyun Tasarimi Bolumu  
Oyun Icin Kod Yazimi II - Vize Odevi  
Ogretim Gorevlisi: Ozgur Bulut Gumrukcü

---

## Oyunun Temel Fikri

Flappy JoyRide, klasik Flappy Bird mekanigini temel alan ama uzerine yeni bir oynanis katmani ekleyen 2D bir endless runner oyunudur.

Oyuncu, Space tusuna basarak kusun yukari ziplamassini saglar ve engellerden kacmaya calisir. Ancak belirli aralikta haritada beliren bir yildiz toplandiginda oyun tamamen degisir: kusun sirtina bir jetpack takilir, arkaplan muzigi Jetpack Joyride temasiyla degisir ve oyuncu artik Space tusunu basili tutarak surukleyebilir. Jetpack etkisi 5 engel sonra sona erer, ancak bir sonraki yildizi toplarsa devam eder.

---

## Oynanış

- **Normal Mod:** Space tusuna her basista kus yukari firlar, yerçekimi asagiya çeker.
- **Jetpack Modu:** Yildizi topla, Space'i basili tut ve yukari sur! Birakinca dus.
- Engellerden kac, skoru artir, yuksek skoru kirma!

---

## Kontroller

| Tus | Islem |
|-----|-------|
| Space (tek bas) | Kus yukari ziplar (Normal Mod) |
| Space (basili tut) | Jetpack ile yukari yuksel (Jetpack Modu) |

---

## Teknik Sistemler

### 1. Temel Oyun Dongusu
Oyun dort ana durumdan olusur:
- **Main Menu:** Oyna ve Cik butonlari
- **Oynanis:** Engellerden kacma, yildiz toplama, jetpack modu
- **Game Over:** Olum ekrani, skor gosterimi
- **Restart / Main Menu:** Yeniden baslama veya ana menuye donme

### 2. Coroutine Kullanimi
`ObstaclePooler` scriptinde engel uretimi bir `IEnumerator` fonksiyonu ile yonetilir. Her `spawnInterval` saniyede bir havuzdan bir engel alinip sahneye yerlestirilir. Bu sayede ana is parcacigi (Main Thread) bloklanmadan zamanlamali uretim yapilir.

```csharp
IEnumerator SpawnRoutine()
{
    while (true)
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnObstacle();
        if (spawnCount % starSpawnEvery == 0 && spawnCount != 0)
            SpawnStar();
    }
}
```

### 3. Object Pooling Sistemi
Engeller ve yildizlar icin ayri havuzlar olusturulmustur. Oyun basinda belirlenen sayida nesne bellege alinir ve `SetActive(false)` ile beklemeye alinir. Ekrandan cikan nesneler yok edilmez, havuza geri gonderilir. Bu yontem Garbage Collection baskisini minimize eder ve performansi stabilize eder.

- Engel havuzu: 6 nesne
- Yildiz havuzu: 3 nesne

### 4. ScriptableObject Kullanimi
Oyunun tum sayisal parametreleri (engel hizi, ziplama gucu, jetpack suresi vb.) `GameSettings` adli bir ScriptableObject uzerinden yonetilir. Bu sayede kod degistirmeden Inspector uzerinden oyun dengesi ayarlanabilir.

### 5. PlayerPrefs Kullanimi
Oyuncunun ulastigi en yuksek skor `PlayerPrefs` ile yerel diske kaydedilir. Oyun her kapanip acildiginda bu deger korunur ve ekranda gosterilir.

```csharp
PlayerPrefs.SetInt("HighScore", score);
PlayerPrefs.Save();
```

---

## Ekstra Mekanik: Jetpack Modu

Klasik Flappy Bird'den ayiran ana ozellik jetpack sistemidir:

- Her 5 engelde bir haritada bir **yildiz** belirir
- Yildizi toplayan oyuncu **Jetpack Moduna** gecer
- Jetpack modunda Space basili tutuldukca kus yukari yukselir, birakilinca duser
- Jetpack alininca **engel ve hareket hizi artar** (zorluk artar)
- **Jetpack Joyride muzigi** devreye girer
- **5 engel** sonra jetpack sona erer, hiz normale doner
- Bir sonraki yildizi toplarsa jetpack yeniden baslar

---

## Proje Yapisi

```
Assets/
  Scripts/
    PlayerController.cs    - Kus hareketi ve jetpack modu
    ObstaclePooler.cs      - Object pool ve engel/yildiz uretimi
    GameManager.cs         - Oyun dongusu ve Game Over
    ScoreManager.cs        - Skor sistemi ve PlayerPrefs
    ScoreTrigger.cs        - Engel gecince skor artirma
    MainMenu.cs            - Ana menu
  Prefabs/
    Obstacle.cs            - Engel prefabi (ust/alt boru + score trigger)
    Star.cs                - Yildiz prefabi
  Scenes/
    MainMenu               - Ana menu sahnesi
    Main                   - Oyun sahnesi
```

---

## Gelistirme Ortami

- **Engine:** Unity 6
- **Dil:** C#
- **Platform:** Windows
- **Kullanilan Paketler:** TextMeshPro

---

## Soruların Cevapları (Açıklama Dokümanı)

**Oyunun temel fikri nedir?**  
Klasik Flappy Bird mekaniğine, belirli aralıklarla çıkan bir yıldız power-up sistemi eklenerek jetpack modu sunulmuştur. Jetpack modunda oynanış tamamen değişir: sürekli basma yerine basılı tutma ile uçulur, hız artar ve müzik değişir.

**Coroutine hangi sistemde kullanıldı?**  
Engel ve yıldız üretim döngüsünde kullanıldı. `SpawnRoutine` adlı IEnumerator fonksiyonu, belirlenen aralıklarla havuzdan nesne çekerek sahneye yerleştirir.

**Object Pool neden tercih edildi?**  
Sürekli `Instantiate` ve `Destroy` çağrısı Garbage Collector üzerinde baskı yaratır ve FPS dalgalanmalarına neden olur. Object Pool ile nesneler önceden bellekte tutulur ve yeniden kullanılır.

**ScriptableObject neyi yönetmektedir?**  
Oyunun tüm sayısal parametrelerini (hız, kuvvet, süreler) merkezi bir veri noktasından yönetir. Kod değiştirmeden Inspector üzerinden ayar yapılabilir.

**PlayerPrefs hangi veriyi saklamaktadır?**  
Oyuncunun ulaştığı en yüksek skor (`HighScore`) değerini oturumlar arası kalıcı olarak saklar.
