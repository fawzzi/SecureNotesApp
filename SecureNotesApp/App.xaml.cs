using System.Windows;

namespace SecureNotesApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Сначала вызываем базовый метод
            base.OnStartup(e);

            // Устанавливаем режим, чтобы приложение не закрывалось сразу после логина
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                // Если пароль верный, создаем главное окно
                MainWindow mainWindow = new MainWindow(loginWindow.Password);

                // Теперь переключаем режим закрытия на обычный (по последнему окну)
                this.ShutdownMode = ShutdownMode.OnLastWindowClose;

                this.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                // Если пользователь нажал отмену в окне логина
                this.Shutdown();
            }
        }
    }
}