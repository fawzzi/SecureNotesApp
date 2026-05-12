using System.Windows;

namespace SecureNotesApp
{
    public partial class LoginWindow
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
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите пароль!");
            }
        }
    }
}