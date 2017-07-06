using System;
using System.Collections.Generic;
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

namespace ScopeMirror.Single
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        AppModel AppModel = AppModel.Instance;

        ScopeWindow ScopeWindow = new ScopeWindow();
        MirrorWindow MirrorWindow;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (o, e) => ScopeWindow.Show();
            Closing += (o, e) =>
            {
                ScopeWindow.Close();
                MirrorWindow?.Close();
            };

            AppModel.IsMirroring.Subscribe(b =>
            {
                if (b)
                {
                    AppModel.StartTrackingImage();
                    ShowMirrorWindow();
                }
                else
                {
                    CloseMirrorWindow();
                    AppModel.StopTrackingImage();
                }
            });
        }

        void ShowMirrorWindow()
        {
            var bounds = AppModel.SelectedScreen.Value.Rectangle;
            var scale = this.GetScreenScale();

            MirrorWindow = new MirrorWindow
            {
                Left = bounds.X / scale,
                Top = bounds.Y / scale,
            };
            MirrorWindow.Loaded += (o, e) => MirrorWindow.WindowState = WindowState.Maximized;
            MirrorWindow.Show();
        }

        void CloseMirrorWindow()
        {
            MirrorWindow.Close();
            MirrorWindow = null;
        }
    }

    public static class ControlsHelper
    {
        public static double GetScreenScale(this Window window)
        {
            if (!window.IsLoaded) throw new InvalidOperationException("The window has not been loaded.");

            var v = window.PointToScreen(new Point(100, 0)) - window.PointToScreen(new Point(0, 0));
            return v.X / 100;
        }
    }
}
