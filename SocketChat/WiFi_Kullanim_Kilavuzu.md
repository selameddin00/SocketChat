# WiFi Ağı Üzerinden Chat Uygulaması Kullanım Kılavuzu

## 🚀 Hızlı Başlangıç

### 1. Server Bilgisayarında (Ana Bilgisayar)
1. **ChatServer** uygulamasını başlatın
2. **"Server'i Başlat"** butonuna tıklayın
3. **IP adreslerini not edin** (örnek: 192.168.1.100)
4. Server formunda şu mesajları göreceksiniz:
   ```
   [17:05:15] Server 5000 portunda baslatildi...
   [17:05:15] Bu bilgisayarin IP adresleri:
   [17:05:15]   - 192.168.1.100
   [17:05:15]   - 127.0.0.1
   [17:05:15] Client'lar bu IP adreslerinden birini kullanarak baglanabilir.
   ```

### 2. Client Bilgisayarlarda (Diğer Bilgisayarlar)
1. **ChatClient** uygulamasını başlatın
2. **Kullanıcı adınızı** girin (örn: "Ahmet")
3. **Server IP adresini** girin (örn: "192.168.1.100")
4. **"Bağlan"** butonuna tıklayın
5. "Server'a başarıyla bağlandı!" mesajını göreceksiniz

### 3. Mesajlaşma
- Her bilgisayarda mesaj yazın ve gönderin
- Mesajlar tüm bağlı bilgisayarlara iletilecek
- Server'da tüm aktiviteler görüntülenecek

## 🔧 Sorun Giderme

### Bağlantı Sorunu
**Problem**: "Server'a bağlanırken hata oluştu" mesajı
**Çözümler**:
1. **IP adresini kontrol edin** - Server formundaki IP'yi doğru girdiğinizden emin olun
2. **Aynı WiFi ağında olduğunuzdan emin olun**
3. **Firewall ayarlarını kontrol edin** (aşağıya bakın)
4. **Server'ın çalıştığından emin olun**

### Firewall Sorunu
**Problem**: Bağlantı engelleniyor
**Çözüm**:
1. **Windows Defender Firewall** açın
2. **"Gelen Kurallar"** bölümüne gidin
3. **"Yeni Kural"** tıklayın
4. **"Port"** seçin → **"İleri"**
5. **"TCP"** seçin → **"Belirli yerel bağlantı noktaları"** → **"5000"** → **"İleri"**
6. **"Bağlantıya İzin Ver"** seçin → **"İleri"**
7. **Tüm profilleri seçin** → **"İleri"**
8. **İsim**: "Chat Server Port 5000" → **"Son"**

### IP Adresi Bulamama
**Problem**: Server'ın IP adresini bilmiyorum
**Çözümler**:
1. **Server formunda IP listesine bakın**
2. **CMD açın** ve `ipconfig` komutunu çalıştırın
3. **WiFi adapter'ındaki IPv4 adresini** kullanın

## 📱 Test Senaryoları

### Senaryo 1: İki Bilgisayar
- **Bilgisayar A**: Server + Client (kullanıcı: "Ali")
- **Bilgisayar B**: Client (kullanıcı: "Veli")
- **Test**: Ali ve Veli birbirleriyle mesajlaşır

### Senaryo 2: Üç Bilgisayar
- **Bilgisayar A**: Server (sadece server)
- **Bilgisayar B**: Client (kullanıcı: "Ahmet")
- **Bilgisayar C**: Client (kullanıcı: "Mehmet")
- **Test**: Ahmet ve Mehmet birbirleriyle mesajlaşır

### Senaryo 3: Karışık Kullanım
- **Bilgisayar A**: Server + Client (kullanıcı: "Admin")
- **Bilgisayar B**: Client (kullanıcı: "User1")
- **Bilgisayar C**: Client (kullanıcı: "User2")
- **Test**: Üç kullanıcı da birbirleriyle mesajlaşır

## 🎯 İpuçları

### Başarılı Kullanım İçin:
- ✅ **Aynı WiFi ağında** olduğunuzdan emin olun
- ✅ **Server'ı önce başlatın**
- ✅ **IP adresini doğru girin**
- ✅ **Firewall'u yapılandırın**
- ✅ **Her kullanıcı farklı isim kullansın**

### Performans İpuçları:
- 📶 **Güçlü WiFi sinyali** kullanın
- 🚫 **VPN kapatın** (bağlantı sorunlarına neden olabilir)
- 🔄 **Bağlantı koparsa** yeniden bağlanın

## 🆘 Acil Durum Çözümleri

### Server Çalışmıyor
1. Server'ı kapatın
2. Yeniden başlatın
3. "Server'i Başlat" butonuna tıklayın

### Client Bağlanamıyor
1. IP adresini kontrol edin
2. Server'ın çalıştığından emin olun
3. Firewall ayarlarını kontrol edin
4. WiFi bağlantısını kontrol edin

### Mesajlar Gelmiyor
1. Bağlantı durumunu kontrol edin
2. Server'da istemci sayısını kontrol edin
3. Yeniden bağlanmayı deneyin

## 📞 Destek

Sorun yaşarsanız:
1. **Server formundaki mesajları** kontrol edin
2. **Client'taki hata mesajlarını** not edin
3. **WiFi bağlantısını** test edin
4. **Firewall ayarlarını** kontrol edin

**Başarılı kullanım!** 🎉
