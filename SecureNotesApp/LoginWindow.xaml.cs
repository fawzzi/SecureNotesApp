using System.Windows;

namespace SecureNotesApp
{
    public partial class LoginWindow : Window
    {
        public string Password { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MasterPasswordBox.Password))
            {
                Password = MasterPasswordBox.Password;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите мастер-пароль для расшифровки данных!");
            }
        }
    }
}