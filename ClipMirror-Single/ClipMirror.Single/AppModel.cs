using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Reactive.Bindings;

namespace ClipMirror.Single
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        public DisplayScreen[] Screens { get; }
        public ReactiveProperty<DisplayScreen> SelectedScreen { get; } = new ReactiveProperty<DisplayScreen>();

        public ReactiveProperty<bool> IsDisplaying { get; } = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public AppModel()
        {
            Screens = Screen.AllScreens
                .Select((s, i) => new DisplayScreen(i + 1, s))
                .ToArray();
            SelectedScreen.Value = Screens.FirstOrDefault(s => !s.Screen.Primary) ?? Screens.First();
        }
    }

    public class DisplayScreen
    {
        public int Id { get; }
        public Screen Screen { get; }

        public Int32Rect Rectangle { get; }
        public string Info => $"{Id}: {Rectangle}";

        public DisplayScreen(int id, Screen screen)
        {
            Id = id;
            Screen = screen;

            Rectangle = screen.Bounds.ToInt32Rect();
        }
    }
}
