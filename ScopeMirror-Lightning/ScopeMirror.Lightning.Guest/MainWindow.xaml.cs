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

namespace ScopeMirror.Lightning.Guest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ScopeWindow ScopeWindow = new ScopeWindow();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (o, e) => ScopeWindow.Show();
            Closing += (o, e) => ScopeWindow.Close();
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
