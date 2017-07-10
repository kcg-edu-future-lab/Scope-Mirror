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
using System.Windows.Shapes;

namespace ScopeMirror.Lightning.Guest
{
    /// <summary>
    /// ScopeWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ScopeWindow : Window
    {
        public static readonly Func<bool, SolidColorBrush> ToMovingBackground = x => x ? OpaqueBackground : Transparent;
        static readonly SolidColorBrush Transparent = new SolidColorBrush(Colors.Transparent);
        static readonly SolidColorBrush OpaqueBackground = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));

        AppModel AppModel = AppModel.Instance;
        double scale = 1.0;

        public ScopeWindow()
        {
            InitializeComponent();

            MouseLeftButtonDown += (o, e) => DragMove();

            Loaded += (o, e) => scale = this.GetScreenScale();

            BasePanel.Loaded += (o, e) => UpdateScopeBounds();
            LocationChanged += (o, e) => UpdateScopeBounds();
            SizeChanged += (o, e) => UpdateScopeBounds();
        }

        void UpdateScopeBounds()
        {
            var leftTop = BasePanel.PointToScreen(new Point(0, 0));
            AppModel.ScopeBounds = new Int32Rect((int)leftTop.X, (int)leftTop.Y, (int)(scale * BasePanel.ActualWidth), (int)(scale * BasePanel.ActualHeight));
        }
    }
}
