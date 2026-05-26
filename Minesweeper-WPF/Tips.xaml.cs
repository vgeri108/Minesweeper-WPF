using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for Tips.xaml
    /// </summary>
    public partial class Tips : Window
    {
        private readonly double originalWidth;
        private readonly double originalHeight;
        private readonly double originalAspect;

        public Tips(string title = "Tipp", string description = "Ne lépj aknára.")
        {
            InitializeComponent();
            Title.Text = title;
            Description.Text = description;

            originalWidth = Width;
            originalHeight = Height;
            if (originalHeight <= 0) originalHeight = 1;
            originalAspect = originalWidth / originalHeight;

            Loaded += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(LeftBottom), System.Windows.Threading.DispatcherPriority.Render);

                var mw = Application.Current.MainWindow as Window;
                if (mw != null)
                {
                    mw.SizeChanged += (ms, me) =>
                    {
                        Dispatcher.BeginInvoke(new Action(LeftBottom), System.Windows.Threading.DispatcherPriority.Render);
                    };
                    mw.LocationChanged += (ms, me) =>
                    {
                        Dispatcher.BeginInvoke(new Action(LeftBottom), System.Windows.Threading.DispatcherPriority.Render);
                    };
                }
            };

            SizeChanged += Tips_SizeChanged;
        }

        private bool _isAdjustingSize = false;
        private void Tips_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (_isAdjustingSize) return;
            _isAdjustingSize = true;

            if (Math.Abs(e.NewSize.Width - e.PreviousSize.Width) > 0.1)
                Height = e.NewSize.Width / originalAspect;
            else if (Math.Abs(e.NewSize.Height - e.PreviousSize.Height) > 0.1)
                Width = e.NewSize.Height * originalAspect;

            _isAdjustingSize = false;
        }

        private void LeftBottom()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null || RootBorder == null)
                return;

            FrameworkElement mainContent = mainWindow.Content as FrameworkElement;
            double mainContentWidth, mainContentHeight;
            Point mainContentTopLeftScreen;

            if (mainContent != null && mainContent.ActualWidth > 0 && mainContent.ActualHeight > 0)
            {
                mainContentWidth = mainContent.ActualWidth;
                mainContentHeight = mainContent.ActualHeight;
                mainContentTopLeftScreen = mainContent.PointToScreen(new Point(0, 0));
            }
            else
            {
                mainContentWidth = mainWindow.ActualWidth;
                mainContentHeight = mainWindow.ActualHeight;
                mainContentTopLeftScreen = mainWindow.PointToScreen(new Point(0, 0));
            }

            var source = PresentationSource.FromVisual(this);
            Matrix fromDevice = source?.CompositionTarget.TransformFromDevice ?? Matrix.Identity;
            Point mainContentTopLeft = fromDevice.Transform(mainContentTopLeftScreen);

            double desiredWidth = Math.Max(1.0, Math.Min(mainContentWidth / 3.0, mainContentWidth));

            double desiredHeight = desiredWidth / originalAspect;
            if (desiredHeight > mainContentHeight)
            {
                desiredHeight = mainContentHeight;
                desiredWidth = desiredHeight * originalAspect;
                desiredWidth = Math.Max(1.0, Math.Min(desiredWidth, mainContentWidth));
            }

            double mainMinWidth = mainWindow.MinWidth;
            if (!double.IsNaN(mainMinWidth) && !double.IsInfinity(mainMinWidth))
            {
                desiredWidth = Math.Max(desiredWidth, mainMinWidth);
                if (desiredWidth > mainContentWidth)
                    desiredWidth = mainContentWidth;
                desiredHeight = desiredWidth / originalAspect;
            }

            MinWidth = mainMinWidth;
            Width = desiredWidth;
            Height = desiredHeight;

            double mainLeft = mainContentTopLeft.X;
            double mainTop = mainContentTopLeft.Y;
            double mainRight = mainLeft + mainContentWidth;
            double mainBottom = mainTop + mainContentHeight;

            RootBorder.UpdateLayout();

            double cornerRadius = Math.Min(RootBorder.CornerRadius.BottomLeft, 
                Math.Min(RootBorder.ActualHeight / 2.0, RootBorder.ActualWidth / 2.0));

            double targetLeft = mainLeft - cornerRadius;
            double targetTop = mainBottom - (RootBorder.ActualHeight - cornerRadius);

            if (targetLeft + Width > mainRight)
                targetLeft = mainRight - Width;
            if (targetLeft < mainLeft)
                targetLeft = mainLeft;

            if (targetTop < mainTop)
                targetTop = mainTop;
            if (targetTop + Height > mainBottom)
                targetTop = mainBottom - Height;

            Left = targetLeft;
            Top = targetTop;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            Activate();
            Topmost = true;
        }
    }
}
