using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VladimirVAZ3.Scripts
{
    public class ShowMessageService
    {
        #region Singleton Implementation
        private static readonly Lazy<ShowMessageService> lazy = new(() => new ShowMessageService());

        public static ShowMessageService Instance => lazy.Value;
        #endregion

        #region Fields and Properties
        private MainWindow _mainWindow;
        private readonly List<ContentControl> _queueMessage = new List<ContentControl>();
        #endregion

        private ShowMessageService()
        {
            // Ничего не делаем, так как экземпляр должен быть уникальным
        }

        public void AttachMainWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }

        public async void Show(Color MainColorBackgroundMessage, KeyValuePair<PackIconKind, Color> InfoIcon, string TextMessage)
        {
            if (_mainWindow.ParentMessage.Children.Count < 2)
            {
                _queueMessage.Add(AddQueue(MainColorBackgroundMessage, InfoIcon, TextMessage));

                _mainWindow.ParentMessage.Children.Add(_queueMessage.Last());

                await _mainWindow.Dispatcher.InvokeAsync(async () =>
                {
                    ContentControl last = _queueMessage.Last();

                    await Animations.Instance.AnimationFade(TimeSpan.FromSeconds(2f), 0f, 1f, _mainWindow.Dispatcher, () => _queueMessage.RemoveAt(_queueMessage.Count - 1), last);
                    await Task.Delay(TimeSpan.FromSeconds(2f));

                    await Animations.Instance.AnimationFade(TimeSpan.FromSeconds(2f), 1f, 0.3f, _mainWindow.Dispatcher, default, last);
                    await Task.Delay(TimeSpan.FromSeconds(0.8f));

                    _mainWindow.ParentMessage?.Children.Remove(last);
                });
            }
        }

        private ContentControl AddQueue(Color MainColorBackgroundMessage, KeyValuePair<PackIconKind, Color> InfoIcon, string TextMessage)
        {
            return new ContentControl()
            {
                ContentTemplate = (DataTemplate)_mainWindow.FindResource("Message"),
                Content = new
                {
                    ColorBorder = new SolidColorBrush(MainColorBackgroundMessage),
                    Icon = InfoIcon.Key,
                    ColorIcon = new SolidColorBrush(InfoIcon.Value),
                    TextError = TextMessage
                }
            };
        }
    }
}
