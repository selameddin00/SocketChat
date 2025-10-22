# Chat Uygulamasi - Windows Forms
öğrenci no: 240541035
isim/soyisim : selameddin tirit
fakülte/bölüm : teknoloji fakültesi / yazılım mühendisliği

Bu proje, C# dilinde Windows Forms kullanilarak gelistirilmis, TCP soketleri uzerinden cok kullanicili bir chat uygulamasidir.

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

## Kullanim

### 1. Server'i Baslatma
1. Visual Studio'da `ChatApp.sln` dosyasini acin
2. Solution Explorer'da `ChatServer` projesine sag tiklayip "Set as Startup Project" secin
3. F5 veya Ctrl+F5 ile calistirin
4. Acilan formda "Server'i Baslat" butonuna tiklayin
5. Server formunda bu bilgisayarin IP adreslerini goruntuleyin

### 2. Client'leri Baslatma (Ayni Bilgisayar)
1. Server calisir durumdayken, Visual Studio'da tekrar Debug menusunden "Start New Instance" secin
2. Acilan pencerede kullanici adinizi girin
3. Server IP adresi olarak "127.0.0.1" girin (localhost icin)
4. "Baglan" butonuna tiklayin

### 3. Client'leri Baslatma (Farkli Bilgisayarlar - WiFi Agi)
1. Server calisan bilgisayarda server formunda gosterilen IP adresini not edin
2. Diger bilgisayarlarda ChatClient uygulamasini calistirin
3. Kullanici adinizi girin
4. Server IP adresi olarak server bilgisayarinin IP adresini girin (ornek: 192.168.1.100)
5. "Baglan" butonuna tiklayin

### 4. Mesajlasma
- Client formunda mesajinizi yazin ve "Gonder" butonuna tiklayin veya Enter tusuna basin
- Mesajiniz tum diger bagli client'lara iletilecektir
- Server formunda tum aktiviteler goruntulenir

### 5. WiFi Agi Kullanimi
- **Ayni WiFi agina bagli** tum bilgisayarlar birbirleriyle mesajlasabilir
- Server bilgisayarinin IP adresini ogrenmek icin server formundaki IP listesine bakin
- **Firewall ayarlari** gerekebilir (Windows Defender Firewall'da 5000 portunu acmaniz gerekebilir)

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
- Turk karakter kullanilmamistir (Turkce yorum satirlari haric)
- WiFi agi kullanimi icin tum bilgisayarlar ayni agda olmali
- Firewall ayarlari gerekebilir (5000 portu acik olmali)
- Server bilgisayarinin IP adresini ogrenmek icin server formuna bakin

## WiFi Agi Kurulumu

### Gerekli Adimlar:
1. **Server bilgisayarinda** ChatServer uygulamasini baslatin
2. **IP adresini not edin** (server formunda gosterilir)
3. **Diger bilgisayarlarda** ChatClient uygulamasini baslatin
4. **Server IP adresini girin** ve baglanin
5. **Mesajlasmaya baslayin!**

### Firewall Ayarlari (Gerekirse):
- Windows Defender Firewall'da "Gelen Kurallar" bölümüne gidin
- "Yeni Kural" -> "Port" -> "TCP" -> "5000" -> "Bağlantıya İzin Ver"

## Lisans

Bu proje egitim amacli olusturulmustur.

