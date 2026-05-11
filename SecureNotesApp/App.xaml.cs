using System.Windows;

namespace SecureNotesApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                MainWindow mainWindow = new MainWindow(loginWindow.Password);

                this.ShutdownMode = ShutdownMode.OnLastWindowClose;

                this.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}