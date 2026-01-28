using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace VladimirVAZ3.Scripts
{
    public class Animations
    {
        #region Instance
        private static readonly Lazy<Animations> lazy = new(() => new Animations());

        private Animations() { }

        public static Animations Instance => lazy.Value;
        #endregion

        public async Task AnimationHight(DependencyProperty Window, Dispatcher dispatcher, UIElement MyFrame, double From, double To, TimeSpan Timer)
        {
            DoubleAnimation _anim = new()
            {
                From = From,
                To = To,
                Duration = Timer,
                EasingFunction = new QuadraticEase()
            };

            await dispatcher.InvokeAsync(() => MyFrame.BeginAnimation(Window, _anim));
        }

        public async Task AnimationFade<T>(TimeSpan Timer, float From, float To, Dispatcher dispatcher, Action? action = default, params T[] Objects) where T : UIElement
        {
            DoubleAnimation _anim = new()
            {
                From = From,
                To = To,
                Duration = Timer,
                EasingFunction = new QuadraticEase()
            };

            action?.Invoke();
            await dispatcher.InvokeAsync(() => Array.ForEach(Objects, x => x.BeginAnimation(UIElement.OpacityProperty, _anim)));
        }
    }
}
