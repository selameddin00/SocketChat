using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class ServerForm : Form
    {
        // Server'in dinleme yapacagi TcpListener nesnesi
        private TcpListener? tcpListener;
        
        // Server'in calisip calismadigini kontrol eden degisken
        private bool serverRunning = false;
        
        // Bagli tum istemcileri tutan liste (thread-safe)
        private List<TcpClient> connectedClients = new List<TcpClient>();
        
        // Thread-safe islemler icin kullanilacak kilit nesnesi
        private object lockObject = new object();

        public ServerForm()
        {
            InitializeComponent();
        }

        // "Server'i Baslat" butonuna tiklandiginda calisir
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!serverRunning)
            {
                StartServer();
            }
            else
            {
                MessageBox.Show("Server zaten calisiyor!", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Server'i baslatan metod
        private void StartServer()
        {
            try
            {
                // TcpListener nesnesini olustur ve 5000 portunu dinlemeye basla
                tcpListener = new TcpListener(IPAddress.Any, 5000);
                tcpListener.Start();
                serverRunning = true;

                // Ekrana bilgi mesajlari yazdir
                AddMessageToListBox("Server 5000 portunda baslatildi...");
                AddMessageToListBox($"Bu bilgisayarin IP adresleri:");
                
                // Bu bilgisayarin tum IP adreslerini goster
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork) // Sadece IPv4 adresleri
                    {
                        AddMessageToListBox($"  - {address}");
                    }
                }
                
                AddMessageToListBox("Client'lar bu IP adreslerinden birini kullanarak baglanabilir.");
                btnStartServer.Enabled = false;

                // Istemci baglantilarini dinleyen thread'i basla
                Thread acceptThread = new Thread(AcceptClients);
                acceptThread.IsBackground = true;
                acceptThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server baslatilirken hata olustu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Yeni istemci baglantilarini kabul eden metod
        private void AcceptClients()
        {
            while (serverRunning)
            {
                try
                {
                    // Yeni bir istemci baglantisini bekle ve kabul et
                    TcpClient client = tcpListener!.AcceptTcpClient();

                    // Bagli istemciler listesine ekle
                    lock (lockObject)
                    {
                        connectedClients.Add(client);
                    }

                    AddMessageToListBox($"Yeni istemci baglandi. Toplam istemci: {connectedClients.Count}");

                    // Her istemci icin ayri bir thread baslatarak mesajlari dinle
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    if (serverRunning)
                    {
                        AddMessageToListBox($"Istemci baglanti hatasi: {ex.Message}");
                    }
                }
            }
        }

        // Istemciden gelen mesajlari dinleyen ve broadcast eden metod
        private void HandleClient(TcpClient client)
        {
            NetworkStream? stream = null;
            
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[1024];

                while (serverRunning && client.Connected)
                {
                    // Istemciden veri oku
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Gelen veriyi string'e cevir
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        
                        // Mesaji ekrana yazdir
                        AddMessageToListBox($"Gelen mesaj: {message}");

                        // Mesaji tum bagli istemcilere gonder (broadcast)
                        BroadcastMessage(message, client);
                    }
                    else
                    {
                        // Istemci baglanti kapatti
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                AddMessageToListBox($"Istemci iletisim hatasi: {ex.Message}");
            }
            finally
            {
                // Baglanti kopan istemciyi listeden kaldir
                RemoveClient(client);
                stream?.Close();
                client.Close();
            }
        }

        // Mesaji tum bagli istemcilere gonderen metod
        private void BroadcastMessage(string message, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            List<TcpClient> clientsToRemove = new List<TcpClient>();

            lock (lockObject)
            {
                foreach (TcpClient client in connectedClients)
                {
                    try
                    {
                        // Mesaji gonderen istemci haric tum istemcilere gonder
                        if (client != sender && client.Connected)
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    catch (Exception)
                    {
                        // Hata alan istemciyi kaldirilacaklar listesine ekle
                        clientsToRemove.Add(client);
                    }
                }

                // Hata alan istemcileri listeden kaldir
                foreach (TcpClient client in clientsToRemove)
                {
                    connectedClients.Remove(client);
                    client.Close();
                }
            }

            if (clientsToRemove.Count > 0)
            {
                AddMessageToListBox($"{clientsToRemove.Count} istemci baglantisi kesildi.");
            }
        }

        // Belirli bir istemciyi listeden kaldiran metod
        private void RemoveClient(TcpClient client)
        {
            lock (lockObject)
            {
                if (connectedClients.Contains(client))
                {
                    connectedClients.Remove(client);
                    AddMessageToListBox($"Bir istemci ayrildi. Kalan istemci: {connectedClients.Count}");
                }
            }
        }

        // ListBox'a mesaj ekleyen thread-safe metod
        private void AddMessageToListBox(string message)
        {
            if (lstMessages.InvokeRequired)
            {
                // UI thread'inde degilsek, Invoke ile UI thread'ine gec
                lstMessages.Invoke(new Action(() => AddMessageToListBox(message)));
            }
            else
            {
                // Zaman damgasi ile mesaji ekle
                string timeStampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
                lstMessages.Items.Add(timeStampedMessage);
                
                // En son eklenen mesaja otomatik scroll
                lstMessages.TopIndex = lstMessages.Items.Count - 1;
            }
        }

        // Form kapanirken calisir
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Server'i durdur
            serverRunning = false;

            // Tum istemci baglantilarini kapat
            lock (lockObject)
            {
                foreach (TcpClient client in connectedClients)
                {
                    try
                    {
                        client.Close();
                    }
                    catch { }
                }
                connectedClients.Clear();
            }

            // TcpListener'i durdur
            try
            {
                tcpListener?.Stop();
            }
            catch { }
        }
    }
}

