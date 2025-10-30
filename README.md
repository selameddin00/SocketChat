# SocketChat (Windows Forms, TCP)

Windows uzerinde C# ve Windows Forms ile gelistirilmis, TCP soketleri uzerinden cok kullanicili anlik mesajlasma uygulamasi.

## Ogrenci Bilgileri

| Alan | Deger |
|---|---|
| Ogrenci No | 240541035 |
| Ad Soyad | Selameddin Tirit |
| Fakulte/Bolum | Teknoloji Fakultesi / Yazilim Muhendisligi |

---

## Proje Yapisi

Proje iki ayri Windows Forms uygulamasindan olusur:

### 1. ChatServer
- **ServerForm.cs**: Server formu ve tum server mantigi
- TcpListener ile 5000 portunda baglanti dinleme
- Her istemci icin ayri thread olusturma
- Gelen mesajlari tum bagli istemcilere broadcast etme
- Baglanti kopan istemcileri otomatik temizleme

### 2. ChatClient
- **ClientForm.cs**: Client formu ve tum client mantigi
- TcpClient ile localhost:5000'e baglanti
- Kullanici adi girisi
- Mesaj gonderme ve alma
- Baglanti kesildiginde otomatik temizlik

## Kurulum ve Calistirma

### 1) Server'i Baslatma
1. Visual Studio'da `ChatApp.sln` dosyasini acin
2. Solution Explorer'da `ChatServer` projesine sag tiklayip "Set as Startup Project" secin
3. F5 veya Ctrl+F5 ile calistirin
4. Acilan formda "Server'i Baslat" butonuna tiklayin
5. Server formunda bu bilgisayarin IP adreslerini goruntuleyin

### 2) Client'leri Baslatma (Ayni Bilgisayar)
1. Server calisir durumdayken, Visual Studio'da tekrar Debug menusunden "Start New Instance" secin
2. Acilan pencerede kullanici adinizi girin
3. Server IP adresi olarak "127.0.0.1" girin (localhost icin)
4. "Baglan" butonuna tiklayin

### 3) Client'leri Baslatma (Farkli Bilgisayarlar - WiFi Agi)
1. Server calisan bilgisayarda server formunda gosterilen IP adresini not edin
2. Diger bilgisayarlarda ChatClient uygulamasini calistirin
3. Kullanici adinizi girin
4. Server IP adresi olarak server bilgisayarinin IP adresini girin (ornek: 192.168.1.100)
5. "Baglan" butonuna tiklayin

### 4) Mesajlasma
- Client formunda mesajinizi yazin ve "Gonder" butonuna tiklayin veya Enter tusuna basin
- Mesajiniz tum diger bagli client'lara iletilecektir
- Server formunda tum aktiviteler goruntulenir

## WiFi Agi Uzerinden Kullanim Kilavuzu

### 🚀 Hizli Baslangic

#### 1) Server Bilgisayarinda (Ana Bilgisayar)
1. ChatServer uygulamasini baslatin
2. "Server'i Baslat" butonuna tiklayin
3. IP adreslerini not edin (ornek: 192.168.1.100)
4. Formda su tur mesajlari gorursunuz:
```
[17:05:15] Server 5000 portunda baslatildi...
[17:05:15] Bu bilgisayarin IP adresleri:
[17:05:15]   - 192.168.1.100
[17:05:15]   - 127.0.0.1
[17:05:15] Client'lar bu IP adreslerinden birini kullanarak baglanabilir.
```

#### 2) Client Bilgisayarlarda (Diger Bilgisayarlar)
1. ChatClient uygulamasini baslatin
2. Kullanici adinizi girin (ornek: "Ahmet")
3. Server IP adresini girin (ornek: "192.168.1.100")
4. "Baglan" butonuna tiklayin
5. "Server'a basariyla baglandi!" mesajini gorursunuz

#### 3) Mesajlasma
- Her bilgisayarda mesaj yazip gonderin
- Mesajlar tum bagli kullanicilara dagitilir (broadcast)
- Server tarafinda tum aktiviteler listelenir

### 🔧 Sorun Giderme

#### Baglanti Sorunu
Problem: "Server'a baglanirken hata olustu" veya baglanamama.
Cözümler:
1. IP adresini dogru girdiginizden emin olun (Server formundaki IP listesine bakin)
2. Tum cihazlarin AYNI WiFi aginda oldugunu dogrulayin
3. Server'in calistigini ve 5000 portunu dinledigini dogrulayin
4. Firewall ayarlarini kontrol edin (asagidaki adimlar)

#### Firewall (Windows Defender) Ayarlari
1. Windows Defender Firewall'i acin
2. "Gelen Kurallar" > "Yeni Kural" deyin
3. "Port" secin > TCP > "Belirli yerel baglanti noktalari" > 5000
4. "Baglantiya Izin Ver" > Tum profiller
5. Isim: "Chat Server Port 5000" > Son

#### IP Adresi Bulamama
1. Server formundaki IP listesine bakin
2. Komut Istemi'nde `ipconfig` calistirin
3. WiFi adaptorundeki IPv4 adresini kullanin

### 📱 Test Senaryolari
- Senaryo 1: Iki bilgisayar (A: Server + Client, B: Client) — mesajlasma calisir
- Senaryo 2: Uc bilgisayar (A: Server, B ve C: Client) — tum kullanicilar mesajlasir
- Senaryo 3: Karisik (A: Server + Client, B: Client, C: Client) — tum mesajlar dagitilir

### 🎯 Ipuçlari
- ✅ Aynı WiFi aginda olun
- ✅ Once Server'i baslatin
- ✅ IP adresini dogru girin
- ✅ Firewall'u dogru yapilandirin
- ✅ Her kullanici farkli isim kullansin
- 📶 Guclu WiFi sinyali kullanin, VPN aciksa kapatin

---

## Ozellikler

- ✅ Cok kullanicili chat destegi
- ✅ Thread-safe mesaj iletimi
- ✅ Otomatik zaman damgasi
- ✅ Baglanti kopan kullanicilarin otomatik temizlenmesi
- ✅ UI donmadan calisma (async thread yapisi)
- ✅ Kullanici dostu arayuz
- ✅ Enter tusu ile hizli mesaj gonderme
- ✅ Localhost uzerinde sorunsuz calisan yapisik
- ✅ WiFi agi uzerinden coklu bilgisayar desteği
- ✅ Manuel IP adresi girisi
- ✅ Otomatik IP adresi gosterimi

## Teknik Detaylar

### Server
- **Port**: 5000
- **Protocol**: TCP
- **Encoding**: UTF-8
- **Threading**: Her istemci icin ayri thread
- **Broadcast**: Tum bagli istemcilere mesaj iletimi

### Client
- **Baglanti**: localhost (127.0.0.1):5000
- **Threading**: Mesaj dinleme icin ayri thread
- **UI Updates**: Invoke kullanarak thread-safe UI guncellemeleri

## Gereksinimler

- .NET 8.0 SDK veya uzeri
- Visual Studio 2022 veya uzeri (Windows Forms destegi ile)
- Windows isletim sistemi

## Notlar

- Server'i kapatmadan once client'lari kapatmaniz onerilir
- Ayni bilgisayarda birden fazla client acabilirsiniz
- Her client kendine ozgu bir kullanici adi olmali
- Turk karakter C# kodunda kullanilmamistir (Turkce yorum satirlari haric)
- WiFi icin tum bilgisayarlar ayni agda olmali
- Firewall'da 5000 TCP portu acik olmalidir
- Server bilgisayarinin IP adresini ogrenirken server formundaki listeyi baz alin

## Lisans

Bu proje egitim amacli olusturulmustur.

