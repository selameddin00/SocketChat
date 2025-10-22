using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        // Server'a baglanan TcpClient nesnesi
        private TcpClient? tcpClient;
        
        // Server ile iletisim kurulan network stream
        private NetworkStream? networkStream;
        
        // Istemcinin bagli olup olmadigini kontrol eden degisken
        private bool isConnected = false;
        
        // Kullanici adi ve server IP adresi
        private string userName = "";
        private string serverIP = "127.0.0.1";

        public ClientForm()
        {
            InitializeComponent();
        }

        // Form yuklendiginde calisir
        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Kullanici adini ve server IP adresini al
            using (var inputDialog = new InputDialog())
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    userName = inputDialog.UserName;
                    serverIP = inputDialog.ServerIP;
                    this.Text = $"Chat Client - {userName} ({serverIP})";
                }
                else
                {
                    // Kullanici isim girmezse uygulamayi kapat
                    Application.Exit();
                    return;
                }
            }

            // Server'a baglan
            ConnectToServer();
        }

        // Server'a baglanan metod
        private void ConnectToServer()
        {
            try
            {
                // Girilen IP adresi uzerinden 5000 portuna baglan
                tcpClient = new TcpClient();
                tcpClient.Connect(serverIP, 5000);
                networkStream = tcpClient.GetStream();
                isConnected = true;

                AddMessageToListBox("Server'a basariyla baglandi!");

                // Server'dan gelen mesajlari dinleyen thread'i basla
                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                // UI elementlerini etkinlestir
                txtMessage.Enabled = true;
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server'a baglanirken hata olustu: {ex.Message}\n\nLutfen once Server'i baslatin.", 
                    "Baglanti Hatasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        // "Gonder" butonuna tiklandiginda calisir
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // TextBox'ta Enter tusuna basildiginda calisir
        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tusunun "beep" sesini engelle
                SendMessage();
            }
        }

        // Mesaj gonderme metodu
        private void SendMessage()
        {
            if (!isConnected || networkStream == null)
            {
                MessageBox.Show("Server baglantisi yok!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string message = txtMessage.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                // Kullanici adi ile birlikte mesaji olustur
                string fullMessage = $"{userName}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(fullMessage);

                // Mesaji server'a gonder
                networkStream.Write(data, 0, data.Length);

                // Kendi mesajini ekrana yaz
                AddMessageToListBox($"Ben: {message}");

                // TextBox'i temizle
                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mesaj gonderilirken hata olustu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisconnectFromServer();
            }
        }

        // Server'dan gelen mesajlari dinleyen metod
        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];

            while (isConnected && tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    // Server'dan veri oku
                    int bytesRead = networkStream!.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Gelen veriyi string'e cevir
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        
                        // Mesaji ekrana yaz
                        AddMessageToListBox(message);
                    }
                    else
                    {
                        // Server baglanti kapatti
                        break;
                    }
                }
                catch (Exception)
                {
                    // Baglanti hatasi olustu
                    break;
                }
            }

            // Baglanti kesildi
            DisconnectFromServer();
        }

        // Server baglantiyi kesme metodu
        private void DisconnectFromServer()
        {
            if (!isConnected)
            {
                return;
            }

            isConnected = false;

            try
            {
                networkStream?.Close();
                tcpClient?.Close();
            }
            catch { }

            // UI thread'inde degisiklik yap
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    AddMessageToListBox("Server baglantisi kesildi.");
                    txtMessage.Enabled = false;
                    btnSend.Enabled = false;
                }));
            }
            else
            {
                AddMessageToListBox("Server baglantisi kesildi.");
                txtMessage.Enabled = false;
                btnSend.Enabled = false;
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
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Server baglantisini kapat
            DisconnectFromServer();
        }
    }

    // Kullanici adi ve server IP adresi almak icin basit bir dialog formu
    public class InputDialog : Form
    {
        private TextBox txtUserName;
        private TextBox txtServerIP;
        private Button btnOk;
        private Label lblUserName;
        private Label lblServerIP;

        public string UserName { get; private set; } = "";
        public string ServerIP { get; private set; } = "127.0.0.1";

        public InputDialog()
        {
            // Form ozellikleri
            this.Text = "Baglanti Bilgileri";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(320, 200);

            // Kullanici adi label
            lblUserName = new Label();
            lblUserName.Text = "Kullanici Adi:";
            lblUserName.Location = new System.Drawing.Point(20, 20);
            lblUserName.Size = new System.Drawing.Size(80, 20);
            this.Controls.Add(lblUserName);

            // Kullanici adi TextBox
            txtUserName = new TextBox();
            txtUserName.Location = new System.Drawing.Point(20, 45);
            txtUserName.Size = new System.Drawing.Size(260, 25);
            this.Controls.Add(txtUserName);

            // Server IP label
            lblServerIP = new Label();
            lblServerIP.Text = "Server IP Adresi:";
            lblServerIP.Location = new System.Drawing.Point(20, 80);
            lblServerIP.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(lblServerIP);

            // Server IP TextBox
            txtServerIP = new TextBox();
            txtServerIP.Text = "127.0.0.1";
            txtServerIP.Location = new System.Drawing.Point(20, 105);
            txtServerIP.Size = new System.Drawing.Size(260, 25);
            txtServerIP.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    btnOk.PerformClick();
                }
            };
            this.Controls.Add(txtServerIP);

            // Button
            btnOk = new Button();
            btnOk.Text = "Baglan";
            btnOk.Location = new System.Drawing.Point(105, 145);
            btnOk.Size = new System.Drawing.Size(90, 30);
            btnOk.Click += (s, e) =>
            {
                UserName = txtUserName.Text.Trim();
                ServerIP = txtServerIP.Text.Trim();
                
                if (string.IsNullOrEmpty(UserName))
                {
                    MessageBox.Show("Lutfen bir kullanici adi girin!", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrEmpty(ServerIP))
                {
                    MessageBox.Show("Lutfen server IP adresini girin!", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(btnOk);

            // Form yuklendikinde kullanici adi TextBox'a odaklan
            this.Load += (s, e) => txtUserName.Focus();
        }
    }
}

