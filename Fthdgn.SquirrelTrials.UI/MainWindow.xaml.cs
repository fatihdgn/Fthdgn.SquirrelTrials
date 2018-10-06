using Squirrel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fthdgn.SquirrelTrials.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Task updateTask;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = AssemblyInfoProvider.Title;
            txtVersion.Text = AssemblyInfoProvider.Version;
            txtExePath.Text = Process.GetCurrentProcess().MainModule.FileName;
            updateTask = UpdateAsync();
        }

        async Task UpdateAsync()
        {
#if DEBUG
            using (var mgr = new UpdateManager("C:\\Users\\fthdg\\source\\repos\\Fthdgn.SquirrelTrials\\Releases"))
#else
            using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/fatihdgn/Fthdgn.SquirrelTrials"))
#endif
            {
                if ((await mgr.CheckForUpdate()).ReleasesToApply.Any())
                {
                    await mgr.UpdateApp(i =>
                    {
                        txtTitle.Dispatcher.BeginInvoke(new Action(() => txtTitle.Text = "Updating..."));
                        txtVersion.Dispatcher.BeginInvoke(new Action(() => txtVersion.Text = $"%{i}"));
                    });
                    var fi = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
                    fi = new FileInfo(System.IO.Path.Combine(fi.Directory.Parent.FullName, fi.Name));
                    if (fi.Exists)
                    {
                        Process.Start(fi.FullName);
                        Application.Current.Shutdown();
                    }
                }
            }
        }
    }
}
