using System.Windows;

namespace SecureNotesApp
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                var mainWindow = new MainWindow(loginWindow.Password);

                ShutdownMode = ShutdownMode.OnLastWindowClose;

                MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}