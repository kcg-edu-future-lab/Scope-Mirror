using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace ScopeMirror.Lightning.Host
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        public ReactiveProperty<byte[]> ScreenImage { get; } = new ReactiveProperty<byte[]>();
    }
}
