using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AnimatedSplash
{
    partial class ExtendedSplash : Page
    {
        private readonly Frame _rootFrame;
        private readonly SplashScreen _splash; 
        private Rect _splashImageRect;

        public ExtendedSplash(SplashScreen splashscreen)
        {
            InitializeComponent();
            
            _rootFrame = Window.Current.Content as Frame;
            _splash = splashscreen;

            Window.Current.SizeChanged += ExtendedSplash_OnResize;

            if (_splash != null)
            {
                _splash.Dismissed += DismissedEventHandler;
                _splashImageRect = _splash.ImageLocation;

                PositionImage();
                PositionRing();
            }
        }
        
        private void PositionImage()
        {
            Canvas.SetLeft(ExtendedSplashImage, _splashImageRect.X);
            Canvas.SetTop(ExtendedSplashImage, _splashImageRect.Y);
            
            ExtendedSplashImage.Height = _splashImageRect.Height;
            ExtendedSplashImage.Width = _splashImageRect.Width;
        }

        private void PositionRing()
        {
            double leftValue = _splashImageRect.X + (_splashImageRect.Width * 0.5) - (SplashProgressRing.Width * 0.5);
            double topValue = (_splashImageRect.Y + _splashImageRect.Height + _splashImageRect.Height * 0.1);

            Canvas.SetLeft(SplashProgressRing, leftValue);
            Canvas.SetTop(SplashProgressRing, topValue);
        }

        private void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            if (_splash != null)
            {
                _splashImageRect = _splash.ImageLocation;
                PositionImage();
                PositionRing();
            }
        }

        private async void DismissedEventHandler(SplashScreen sender, object e)
        {
            await SetupApp();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, DismissExtendedSplash);
        }

        private async Task SetupApp()
        {
            await Task.Delay(2000);
        }

        private void DismissExtendedSplash()
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", ExtendedSplashImage);
            _rootFrame.Navigate(typeof(MainPage));
        }
    }
}