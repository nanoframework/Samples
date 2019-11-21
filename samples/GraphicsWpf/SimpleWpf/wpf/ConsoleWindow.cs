using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.Runtime.Events;
using SimpleWpf;

namespace nanoFramework.UI.Console
{
    public class ConsoleWindow : Window
    {
        private Font Small =  Resource.GetFont(Resource.FontResources.small);
        private Font CourierRegular10 = Resource.GetFont(Resource.FontResources.courierregular10);
        private Font NinaB = Resource.GetFont(Resource.FontResources.NinaB);

        private TextFlow log;
        private Text timeText;
        private Brush solidBlack = new SolidColorBrush(Color.Black);

        public ConsoleWindow()
        {
            StackPanel panel = new StackPanel();

            timeText = new Text(CourierRegular10, "Embarrassing second of silence.");
            timeText.TextAlignment = TextAlignment.Right;
            timeText.VerticalAlignment = VerticalAlignment.Top;
            timeText.ForeColor = ColorUtility.ColorFromRGB(255, 255, 0);
            panel.Children.Add(timeText);

            ScrollViewer scroll = new ScrollViewer();
            scroll.Height = SystemMetrics.ScreenHeight - CourierRegular10.Height;
            scroll.Width = SystemMetrics.ScreenWidth;
            scroll.ScrollingStyle = ScrollingStyle.Last;
            scroll.Background = null;
            scroll.LineHeight = Small.Height;
            
            panel.Children.Add(scroll);                        

            log = new TextFlow();
            log.HorizontalAlignment = HorizontalAlignment.Left;
            log.VerticalAlignment = VerticalAlignment.Top;
            scroll.Child = log;            

            Background = solidBlack;
            Child = panel;

            //ExtendedTimer clock = new ExtendedTimer(
            //    new System.Threading.TimerCallback(
            //        delegate
            //        {
            //            timeText.TextContent = DateTime.Now.ToString("ddd d. MMMM yyyy HH:mm:ss");
            //            timeText.Invalidate();
            //        }),
            //    null, ExtendedTimer.TimeEvents.Second);
        }

        public void WriteLine(string s)
        {
            //Dispatcher.BeginInvoke( new EventHandler(InvokedWriteLine), new object[] { s, EventArgs.Empty });
        }

        private void InvokedWriteLine(object text, EventArgs e)
        {
            log.TextRuns.Add(new TextRun(text.ToString(), Small, Color.White));
            log.TextRuns.Add(TextRun.EndOfLine);
            ((ScrollViewer)log.Parent).LineDown();
        }

        protected virtual void OnStart() { }
    }
}
