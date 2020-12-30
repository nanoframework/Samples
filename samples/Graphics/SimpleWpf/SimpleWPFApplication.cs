////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using nanoFramework.UI.Input;
using nanoFramework.Presentation;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation.Shapes;
using nanoFramework.UI;
using nanoFramework.UI.Threading;
using nanoFramework.Runtime.Events;
using SimpleWpf;

namespace SimpleWPF
{
    /// <summary>
    /// This is the base class of all the windows.  It makes every window visible, 
    /// sets the window's size to the full size of the LCD, and give the window 
    /// focus.
    /// </summary>
    internal class PresentationWindow : Window
    {
        protected MySimpleWPFApplication _program;

        /// <summary>
        /// Constructs a PresentationWindow for the specified program.
        /// </summary>
        /// <param name="program"></param>
        protected PresentationWindow(MySimpleWPFApplication program)
        {
            _program = program;

            // Make the window visible and the size of the LCD
            this.Visibility = Visibility.Visible;
            this.Width = DisplayControl.ScreenWidth;
            this.Height = DisplayControl.ScreenHeight;
            Buttons.Focus(this); // Set focus to this window
        }

        /// <summary>
        /// Removes this window from the Window Manager.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            // Removes this window form the Window Manager.
            this.Close();

            // When any button is pressed, go back to the Home page.
            _program.GoHome();
        }
    }

    /// <summary>
    /// This is the main menu window; it shows the animated, sliding menu icons and 
    /// instructions to the user. It also handles button presses to move the menu 
    /// icons and handle selection of the menu items.
    /// </summary>
    internal sealed class MainMenuWindow : PresentationWindow
    {
        // This member keeps the menu item panel around.
        private MenuItemPanel m_MenuItemPanel;

        /// <summary>
        /// Constructs a MainMenuWindow for the specified program.
        /// </summary>
        /// <param name="program"></param>
        public MainMenuWindow(MySimpleWPFApplication program)
            : base(program)
        {
            // Create some colors for the text items.
            Color instructionTextColor = ColorUtility.ColorFromRGB(255, 255, 255);
            Color backgroundColor = ColorUtility.ColorFromRGB(0, 0, 0);
            Color upperBackgroundColor = ColorUtility.ColorFromRGB(69, 69, 69);
            Color unselectedItemColor = ColorUtility.ColorFromRGB(192, 192, 192);
            Color selectedItemColor = ColorUtility.ColorFromRGB(128, 128, 128);

            // The Main window contains a vertical StackPanel.
            StackPanel panel = new StackPanel(Orientation.Vertical);
            this.Child = panel;

            // The top child contains a horizontal StackPanel.
            m_MenuItemPanel = new MenuItemPanel(this.Width, this.Height);

            // The top child contains the menu items.  We pass in the small bitmap, 
            // large bitmap a description and then a large bitmap to use as a common 
            // sized bitmap for calculating the width and height of a MenuItem.
            MenuItem menuItem1 = new MenuItem(Resource.BitmapResources.Vertical_Stack_Panel_Icon_Small, Resource.BitmapResources.Vertical_Stack_Panel_Icon, "Vertical Stack Panel", Resource.BitmapResources.Canvas_Panel_Icon);
            MenuItem menuItem2 = new MenuItem(Resource.BitmapResources.Horizontal_Stack_Panel_Icon_Small, Resource.BitmapResources.Horizontal_Stack_Panel_Icon, "Horizontal Stack Panel", Resource.BitmapResources.Canvas_Panel_Icon);
            MenuItem menuItem3 = new MenuItem(Resource.BitmapResources.Canvas_Panel_Icon_Small, Resource.BitmapResources.Canvas_Panel_Icon, "Canvas Panel", Resource.BitmapResources.Canvas_Panel_Icon);
            MenuItem menuItem4 = new MenuItem(Resource.BitmapResources.Scrollable_Panel_Icon_Small, Resource.BitmapResources.Scrollable_Panel_Icon, "Scrollable Panel", Resource.BitmapResources.Canvas_Panel_Icon);
            MenuItem menuItem5 = new MenuItem(Resource.BitmapResources.Free_Drawing_Panel_Icon_Small, Resource.BitmapResources.Free_Drawing_Panel_Icon, "Free Drawing Panel", Resource.BitmapResources.Canvas_Panel_Icon);

            // Add each of the menu items to the menu item panel
            m_MenuItemPanel.AddMenuItem(menuItem1);
            m_MenuItemPanel.AddMenuItem(menuItem2);
            m_MenuItemPanel.AddMenuItem(menuItem3);
            m_MenuItemPanel.AddMenuItem(menuItem4);
            m_MenuItemPanel.AddMenuItem(menuItem5);

            // Add the menu item panel to the main window panel
            panel.Children.Add(m_MenuItemPanel);
        }

        /// <summary>
        /// Handles button click events.
        /// </summary>
        /// <param name="e">The button event arguments.</param>
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            switch (e.Button)
            {
                // If <Enter> button is pressed, go into the selected demo
                case Button.VK_SELECT:
                    {
                        switch (m_MenuItemPanel.CurrentChild)
                        {
                            case 0:  // Vertical Stack Panel Demo
                                new StackPanelDemo(_program, Orientation.Vertical);
                                break;
                            case 1:  // Horizontal Stack Panel Demo
                                new StackPanelDemo(_program, Orientation.Horizontal);
                                break;
                            case 2:  // Canvas Panel Demo
                                new CanvasPanelDemo(_program);
                                break;
                            case 3:  // Scrollable Panel Demo
                                new ScrollPanelDemo(_program);
                                break;
                            case 4:  // Free Drawing Panel Demo
                                new FreeDrawingDemo(_program);
                                break;
                        }
                    }
                    break;

                // If <Left> button is pressed, change the menu item left one.
                case Button.VK_LEFT:
                    {
                        if (m_MenuItemPanel != null)
                            m_MenuItemPanel.CurrentChild--;
                    }
                    break;

                // If <Right> button is pressed, change the menu item right one.
                case Button.VK_RIGHT:
                    {
                        if (m_MenuItemPanel != null)
                            m_MenuItemPanel.CurrentChild++;
                    }
                    break;
            }

            // Don't call base implementation (base.OnButtonDown) or we'll go back 
            // Home.
        }
    }

    /// <summary>
    /// This is the stack panel demo class.  This class shows how to stack items 
    /// (shapes, in this case) either vertically or horizontally, depending on the 
    /// orientation passed into the constructor.
    /// </summary>
    internal sealed class StackPanelDemo : PresentationWindow
    {
        /// <summary>
        /// This class shows how to build your own shape drawing in a 
        /// DrawingContext.
        /// </summary>
        private sealed class Cross : Shape
        {
            /// <summary>
            /// The default constructor.
            /// </summary>
            public Cross() { }

            /// <summary>
            /// To manual draw, override the OnRender method and draw using 
            /// standard drawing type functions.
            /// </summary>
            /// <param name="dc"></param>
            public override void OnRender(DrawingContext dc)
            {
                // Draw a line from top, left to bottom, right
                dc.DrawLine(base.Stroke, 0, 0, Width, Height);

                // Draw a line from top, right to bottom, left
                dc.DrawLine(base.Stroke, Width, 0, 0, Height);
            }
        }

        /// <summary>
        /// Constructs a StackPanelDemo for the specified program and orientation.
        /// </summary>
        /// <param name="program">The program for which to construct a demo.</param>
        /// <param name="orientation">The orientation of the graphical layout.
        /// </param>
        public StackPanelDemo(MySimpleWPFApplication program, Orientation orientation)
            : base(program)
        {
            StackPanel panel = new StackPanel(orientation);
            this.Child = panel;
            panel.Visibility = Visibility.Visible;

            // This is an array of different shapes to be drawn.
            Shape[] shapes = new Shape[] { new Ellipse(0, 0),
                                           new Line(),         // A square.
                                           new Polygon(new Int32[] { 0, 0,    50, 0,    50, 50,    0, 50 }),
                                           new Rectangle(),
                                            
                                           new Cross()  // A custom shape.
                                         };

            // Set up the needed member values for each shape.
            for (Int32 x = 0; x < shapes.Length; x++)
            {
                Shape s = shapes[x];
                s.Fill = new SolidColorBrush(ColorUtility.ColorFromRGB(0, 255, 0));
                s.Stroke = new Pen(Color.Black, 2);
                s.Visibility = Visibility.Visible;
                s.HorizontalAlignment = HorizontalAlignment.Center;
                s.VerticalAlignment = VerticalAlignment.Center;
                s.Height = Height - 1;
                s.Width = Width - 1;

                if (panel.Orientation == Orientation.Horizontal)
                    s.Width /= shapes.Length;
                else
                    s.Height /= shapes.Length;

                panel.Children.Add(s);
            }
        }
    }

    /// <summary>
    /// This class demonstrates positioning text objects directly on a canvas 
    /// object.
    /// </summary>
    internal sealed class CanvasPanelDemo : PresentationWindow
    {
        /// <summary>
        /// Constructs a CanvasPanelDemo for the given program.
        /// </summary>
        /// <param name="program"></param>
        public CanvasPanelDemo(MySimpleWPFApplication program)
            : base(program)
        {
            Canvas canvas = new Canvas();
            this.Child = canvas;
            this.Background = new SolidColorBrush(ColorUtility.ColorFromRGB(0, 255, 255));

            for (Int32 x = 0; x < Width; x += Width / 4)
            {
                for (Int32 y = 0; y < Height; y += Height / 4)
                {
                    Text text = new Text(MySimpleWPFApplication.SmallFont, " (" + x + "," + y + ")");
                    Canvas.SetLeft(text, x);
                    Canvas.SetTop(text, y);
                    canvas.Children.Add(text);
                }
            }
        }
    }

    /// <summary>
    /// This class is a helper class for the scrolling text example below; it 
    /// derives from the TextFlow class and adds functionality that makes it easier 
    /// to simply pass in a string of text that contains CRLF pairs.  This class 
    /// breaks up the lines into proper TextRuns for the TextFlow class it is based 
    /// on.
    /// </summary>
    internal sealed class ScrollerText : TextFlow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        public ScrollerText(string text, Font font, Color color)
            : base()
        {
            int pos = 0;

            // Break the text up if it contains CR/LF pairs.
            while ((pos = text.IndexOf("\r\n")) > -1)
            {
                TextRuns.Add(new TextRun(text.Substring(0, pos), font, color));
                TextRuns.Add(TextRun.EndOfLine);
                pos += 2;
                text = text.Substring(pos, text.Length - pos);
            }

            TextRuns.Add(new TextRun(text, font, color));
        }
    }

    /// <summary>
    /// This is the TextScrollView class; it derives from a Panel and wraps a 
    /// ScrollViewer object and a ScrollerText object (defined above).  With those 2 
    /// objects it demonstrates how to create a scrollable text object.
    /// </summary>
    internal sealed class TextScrollViewer : Panel
    {
        // These private values and public member give easy access to controlling
        // how large the horizontal scroll bar is.
        private int _hScrollHeight = 10;
        private int _hScrollWidth = 0;
        private double _hScrollRatio = 1;
        public int HScrollHeight
        {
            get { return _hScrollHeight; }
            set
            {
                _hScrollHeight = value;
                _hScrollWidth = Width - _vScrollWidth;
                _hScrollRatio = _hScrollWidth - _vScrollWidth - _vScrollWidth;
                _hScrollRatio /= Width;
            }
        }

        // These private values and public member give easy access to controlling
        // how large the vertical scroll bar is.
        private int _vScrollWidth = 10;
        private int _vScrollHeight = 0;
        private double _vScrollRatio = 1;
        public int VScrollWidth
        {
            get { return _vScrollWidth; }
            set
            {
                _vScrollWidth = value;
                _vScrollHeight = Height - _hScrollHeight;
                _vScrollRatio = _vScrollHeight - _hScrollHeight - _hScrollHeight;
                _vScrollRatio /= Height;
            }
        }

        // This private member is a standard Micro Framework Presentation object
        // The important member functions are the ones that control the scrolling
        // The TextScrollViewer class provides easy access to those scrolling
        // functions with the 4 following member functions.
        private ScrollViewer _viewer;

        /// <summary>
        /// Scroll one line up.
        /// </summary>
        public void LineUp()
        {
            _viewer.LineUp();
        }

        /// <summary>
        /// Scroll one line down.
        /// </summary>
        public void LineDown()
        {
            _viewer.LineDown();
        }

        /// <summary>
        /// Scroll to the left.
        /// </summary>
        public void LineLeft()
        {
            _viewer.LineLeft();
        }

        /// <summary>
        /// Scroll to the right.
        /// </summary>
        public void LineRight()
        {
            _viewer.LineRight();
        }

        // This public member is the text object that is based on the TextFlow 
        // class.
        public ScrollerText ScrollText;

        /// <summary>
        /// Constructs a TextScrollViewer using the specified text, font, and color.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        public TextScrollViewer(string text, Font font, Color color)
        {
            // Create the ScrollViewer object.
            _viewer = new ScrollViewer();

            // Create the ScrollText object using the parameters passed into the 
            // constructor and then set other important member values.
            ScrollText = new ScrollerText(text, font, color);
            ScrollText.HorizontalAlignment = HorizontalAlignment.Left;
            ScrollText.VerticalAlignment = VerticalAlignment.Top;

            // Set the child of the viewer to be the ScrollText object.
            this._viewer.Child = ScrollText;

            // Hard code a line with and height based on the character 'A'.
            _viewer.LineWidth = font.CharWidth('A');
            _viewer.LineHeight = font.Height;

            // Set the child of our class to be the ScrollViewer object.
            this.Children.Add(_viewer);
        }

        /// <summary>
        /// Normally the presentation framework would arrange our child objects 
        /// based on their visible size.  Since we are tyring to show scrolling 
        /// beyond that size, we override the ArrangeOverride method to layout 
        /// elements in an arrangement that is larger than the screen.
        /// </summary>
        /// <param name="arrangeWidth"></param>
        /// <param name="arrangeHeight"></param>
        protected override void ArrangeOverride(int arrangeWidth, int arrangeHeight)
        {
            base.ArrangeOverride(arrangeWidth, arrangeHeight);

            // Set the ScrollViewer's width and height to allow room for
            // the scroll bars.
            _viewer.Width = arrangeWidth - _vScrollWidth;
            _viewer.Height = arrangeHeight - _hScrollHeight;
            _viewer.Arrange(0, 0, _viewer.Width, _viewer.Height);

            // Set the ScrollText's width and height to be twice as big as the 
            // allowed area.
            ScrollText.Width = arrangeWidth * 2;
            ScrollText.Height = arrangeHeight * 2;
            ScrollText.UpdateLayout();
        }

        /// <summary>
        /// Override the OnRender to enable manually drawing the scroll bars.
        /// </summary>
        /// <param name="dc"></param>
        public override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // Make a brush and pen for drawing scroll bars.
            SolidColorBrush brush =
                new SolidColorBrush(ColorUtility.ColorFromRGB(224, 224, 224));
            SolidColorBrush sliderBrush = new SolidColorBrush(ColorUtility.ColorFromRGB(64, 64, 64));
            Pen pen = new Pen(ColorUtility.ColorFromRGB(64, 64, 64));

            // Draw the horizontal scroll bar.
            int hOffset = (int)(_viewer.HorizontalOffset * _hScrollRatio);
            dc.DrawRectangle(brush, pen, 0, Height - _hScrollHeight, _hScrollWidth, _hScrollHeight);
            dc.DrawRectangle(sliderBrush, pen, (int)(_viewer.HorizontalOffset * _hScrollRatio), Height - _hScrollHeight, _vScrollWidth, _hScrollHeight);

            // Draw the vertical scroll bar.
            int vOffset = (int)(_viewer.VerticalOffset * _vScrollRatio);
            dc.DrawRectangle(brush, pen, Width - _vScrollWidth, 0, _vScrollWidth, _vScrollHeight);
            dc.DrawRectangle(sliderBrush, pen, Width - _vScrollWidth, vOffset, _vScrollWidth, _hScrollHeight);
        }
    }

    /// <summary>
    /// This class demonstrates scrolling a field of text.
    /// </summary>
    internal sealed class ScrollPanelDemo : PresentationWindow
    {
        // This member is the text scroller helper class defined above.
        TextScrollViewer _viewer;

        /// <summary>
        /// Constructs a ScrollPanelDemo for the specified program.
        /// </summary>
        /// <param name="program"></param>
        public ScrollPanelDemo(MySimpleWPFApplication program)
            : base(program)
        {
            // Create a stack panel for the title and scroll view.
            StackPanel panel = new StackPanel(Orientation.Vertical);

            // Create the text scroll view and set all of its properties.
            _viewer = new TextScrollViewer(Resource.GetString(Resource.StringResources.ScrollableText), MySimpleWPFApplication.NinaBFont, Color.Black);
            _viewer.Width = this.Width;
            // Make room for the title bar.
            _viewer.Height = this.Height - 25;
            _viewer.HScrollHeight = 10;
            _viewer.VScrollWidth = 10;
            _viewer.HorizontalAlignment = HorizontalAlignment.Left;
            _viewer.VerticalAlignment = VerticalAlignment.Top;

            // Create the title text
            Text title = new Text(MySimpleWPFApplication.NinaBFont, Resource.GetString(Resource.StringResources.ScrollableTextTitle));
            title.ForeColor = Color.White;

            // Add the elements to the stack panel.
            panel.Children.Add(title);
            panel.Children.Add(_viewer);

            // Add the stack panel to this window.
            this.Child = panel;

            // Set the background color.
            this.Background = new SolidColorBrush(ColorUtility.ColorFromRGB(64, 64, 255));
        }

        /// <summary>
        /// A button handler.
        /// </summary>
        /// <param name="e">The button-press event arguments.</param>
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Button.VK_SELECT:

                    // Remove this window from the Window Manager.
                    this.Close();

                    // When the <Select> button is pressed, go back to the Home 
                    // page.
                    _program.GoHome();
                    break;

                case Button.VK_UP:
                    // Scroll the viewer up one line.
                    _viewer.LineUp();
                    break;

                case Button.VK_DOWN:
                    // Scroll the viewer down one line.
                    _viewer.LineDown();
                    break;

                case Button.VK_LEFT:
                    // Scroll the viewer left one line.
                    _viewer.LineLeft();
                    break;

                case Button.VK_RIGHT:
                    // Scroll the viewer right one line.
                    _viewer.LineRight();
                    break;
            }
        }
    }

    /// <summary>
    /// This class demonstrates free drawing on a surface.
    /// </summary>
    internal sealed class FreeDrawingDemo : PresentationWindow
    {
        private Random _random;

        /// <summary>
        /// Constructs a FreeDrawingDemo, using the specified program.
        /// </summary>
        /// <param name="program">The program to construct a demo for.</param>
        public FreeDrawingDemo(MySimpleWPFApplication program)
            : base(program)
        {
            _random = new Random();
        }

        /// <summary>
        /// Override the OnRender and draw using the DrawingContext that is passed 
        /// in.
        /// </summary>
        /// <param name="dc"></param>
        public override void OnRender(DrawingContext dc)
        {
            // Paint the background.
            dc.DrawRectangle(new SolidColorBrush(ColorUtility.ColorFromRGB(128, 0, 255)), new Pen(ColorUtility.ColorFromRGB(128, 0, 255)), 0, 0, Width, Height);

            // Draw some circles.
            for (int i = 0; i < 3; i++)
                dc.DrawEllipse(new SolidColorBrush(ColorUtility.ColorFromRGB((byte)(64 * i), 128, 128)), new Pen(ColorUtility.ColorFromRGB(255, (byte)(64 * i), 255)), Width / 5, Height / 5, i * (Width / 10 - (i * 2)), i * (Height / 10 - (i * 2)));

            // Draw some lines.
            for (int i = 0; i < 20; i++)
                dc.DrawLine(new Pen(ColorUtility.ColorFromRGB((byte)(16 * i),
                    (byte)(16 * (20 - i)), (byte)(16 * (2 * i)))),
                    _random.Next(Width / 2) + Width / 3,
                    _random.Next(Height / 3) + Height / 3,
                    _random.Next(Width / 4) + Width / 2,
                    _random.Next(Height / 4) + Height / 2);

            // Draw a rectangle.
            dc.DrawRectangle(new SolidColorBrush(ColorUtility.ColorFromRGB(255, 0, 0)), new Pen(ColorUtility.ColorFromRGB(0, 0, 255)), Width - 40, 0, 30, 100);

            // Draw a polyline.
            int[] points = { 10, 230, 30, 210, 0, 180, 30, 130, 50, 130, 80, 180, 50, 210, 70, 230 };
            if (Width > Height)
                for (int i = 1; i < points.Length; i += 2)
                    points[i] -= 20;
            dc.DrawPolygon(new SolidColorBrush(ColorUtility.ColorFromRGB(32, 0, 128)), new Pen(ColorUtility.ColorFromRGB(0, 0, 0)), points);

            // Draw some text.
            dc.DrawText(Resource.GetString(Resource.StringResources.DrawTextSample), Resource.GetFont(Resource.FontResources.NinaB), ColorUtility.ColorFromRGB(255, 255, 255), _random.Next(Width / 4), Height - 20);

        }
    }

    /// <summary>
    /// This control class is a container for the MenuItem controls.  It handles 
    /// which MenuItem is current and moving them back and forth.
    /// </summary>
    internal sealed class MenuItemPanel : Control
    {
        // Private members.
        private int _currentChild = 0;
        private int _width;
        private int _height;
        private int _animationStep;

        private int _largeX;

        // This array holds the MenuItems.
        public ArrayList MenuItemList;

        /// <summary>
        /// Constructs a MenuItemPanel of the specified size.
        /// </summary>
        /// <param name="width">The width of the panel.</param>
        /// <param name="height">The height of the panel.</param>
        public MenuItemPanel(int width, int height)
        {
            // Default background color is black.
            Background = new SolidColorBrush(Color.Black);

            // Width and height are passed in and set to our local members.
            _width = width;
            _height = height;

            // Create the MenuItem array.
            MenuItemList = new ArrayList();


            // Set the width of each MenuItem.
            _largeX = Resource.GetBitmap(Resource.BitmapResources.Canvas_Panel_Icon).Width + xOffsetSeparation;
        }

        /// <summary>
        /// Adds a menu item, by wrapping the ArrayList Add method.
        /// </summary>
        /// <param name="menuItem"></param>
        public void AddMenuItem(MenuItem menuItem)
        {
            MenuItemList.Add(menuItem);
        }

        /// <summary>
        /// This property handles getting and setting the current child MenuItem 
        /// index.
        /// </summary>
        public int CurrentChild
        {
            // Return the current child index.
            get { return _currentChild; }

            // Setting the current child also kicks off the animation sequence.
            set
            {
                if (value > _currentChild)
                    _animationStep = maxStep;        // Moving right.
                else if (value < _currentChild)
                    _animationStep = -maxStep;       // Moving left.
                else
                    _animationStep = 0;              // Same child, no movement.

                if (value >= MenuItemList.Count)    // Handle wrapping around right.
                    value = 0;

                if (value < 0)                      // Handle wrapping around left.
                    value = MenuItemList.Count - 1;

                // Set the child and redraw to start the animation.
                if (_animationStep != 0)
                {
                    _currentChild = value;
                    Invalidate();
                }
            }
        }

        // Using static and constant members allows us to easily change these
        // in one location and affect the many places that these numbers are needed.

        // The number of frames in the animation.
        static public int maxStep = 5;

        // The distance between each MenuItem.
        const int xOffsetSeparation = 4;

        // The number of mSec between each frame.
        const int timerInterval = 20;

        /// <summary>
        /// Override the OnRender method to do the actual drawing of the menu.
        /// </summary>
        /// <param name="dc">The drawing context to render.</param>
        public override void OnRender(DrawingContext dc)
        {
            // Call the base class in case this control contains other controls.  
            // Depending on where those controls are placed, this call might not 
            // be optimal.
            base.OnRender(dc);

            // Calculate some initial values for positioning and drawing the 
            // MenuItems.

            // Set the starting x position.
            int x = (_width / 2) - ((_largeX * 2) + (_largeX / 2));

            // Set the starting y position.
            int y = 6;

            // Set the scaling of the current MenuItem.
            int scale = 0;

            // Set the scaling offset based on the animation step.
            int scaleOffset = Mathematics.Abs(_animationStep);

            // Adjust the x based on the animation step.
            x += _animationStep * 5;

            // Iterate through the children, limiting them to 2 in front and 2 
            // behind the current child.  This places the current MenuItem in the 
            // middle of the menu.
            for (int i = _currentChild - 2; i < _currentChild + 3; i++)
            {
                // If we are on the current child...
                if (i == _currentChild)
                {
                    // Scale the current child based on the current animation step 
                    // value.  The current child is getting smaller, so take the 
                    // largest value (maxStep) and subtract the current scaling 
                    // offset.
                    scale = maxStep - scaleOffset;
                }
                else
                {
                    // If we are moving left and are drawing the child to the left, 
                    // or we are moving right and are drawing the child to the 
                    // right, then that child needs to be growing in size.  Else the 
                    // child is drawn without any scaling.
                    if ((_animationStep < 0 && i == _currentChild + 1) || (_animationStep > 0 && i == _currentChild - 1))
                        scale = scaleOffset;
                    else
                        scale = 0;
                }

                // Variable to point to the current MenuItem we want to draw.
                MenuItem menuItem = null;

                // Get the correct MenuItem from the array based on the value of i.
                // Because we are looking 2 left and 2 right, if the current child
                // is near the beginning or end of the array, we have to watch for
                // wrapping around the ends.
                if (i < 0)
                    menuItem = (MenuItem)MenuItemList[MenuItemList.Count + i];
                else if (i > MenuItemList.Count - 1)
                    menuItem = (MenuItem)MenuItemList[i - MenuItemList.Count];
                else
                    menuItem = (MenuItem)MenuItemList[i];

                // Have the MenuItem render itself based on the position and scaling 
                // calculated.
                menuItem.Render(dc, x, y, scale);

                // Increment the x position by the size of the MenuItems
                x += _largeX;
            }

            // Draw the current menuItem's text.
            if (_width > 0)
            {
                // Check the window size for displaying instructions.
                int step = 20;
                int row = 120;

                // Check for portrait display.
                if (_width < _height)
                    step = 40;

                // Draw the description of the current MenuItem.
                string text = ((MenuItem)MenuItemList[_currentChild]).Description;
                dc.DrawText(ref text, Resource.GetFont(Resource.FontResources.NinaB), Color.White, 10, row, _width - 20, step, TextAlignment.Center, TextTrimming.None);

                // Draw the basic instructions for the menu.
                text = Resource.GetString(Resource.StringResources.MenuScrolling);
                row += (step * 2);
                dc.DrawText(ref text, Resource.GetFont(Resource.FontResources.NinaB), Color.White, 10, row, _width - 20, step, TextAlignment.Center, TextTrimming.None);
                text = Resource.GetString(Resource.StringResources.MenuSelection);
                row += step;
                dc.DrawText(ref text, Resource.GetFont(Resource.FontResources.NinaB), Color.White, 10, row, _width - 20, step, TextAlignment.Center, TextTrimming.None);
                text = Resource.GetString(Resource.StringResources.ReturnToMenu);
                row += step;
                dc.DrawText(ref text, Resource.GetFont(Resource.FontResources.NinaB), Color.White, 10, row, _width - 20, step, TextAlignment.Center, TextTrimming.None);
            }

            // Start the animation timer.  The animation timer is called every time 
            // the menu is rendered.  The animation timer will handle decrementing 
            // the _animationStep member and stopping the timer when _animationStep 
            // reaches 0.
            StartAnimationTimer();
        }

        // Private timer member.
        private DispatcherTimer _animationTimer;

        /// <summary>
        /// Accessor function for the private timer member.
        /// </summary>
        private void StartAnimationTimer()
        {
            // Only start the timer if _animationStep is not 0.
            if (_animationStep != 0)
            {
                // The first time through, create the timer.
                if (_animationTimer == null)
                {
                    _animationTimer = new DispatcherTimer(this.Dispatcher);
                    _animationTimer.Interval = new TimeSpan(0, 0, 0, 0, timerInterval);
                    _animationTimer.Tick += new EventHandler(OnAnimationTimer);
                }

                // Keep track of when the timer was started, to deal with missing
                // frames because of a slow processor or being in the emulator.
                _lastTick = DateTime.UtcNow.Ticks;

                // Start the timer
                _animationTimer.Start();
            }
        }

        // Private member to keep track of when the timer was started so we can 
        // detect missing frames on slow processors and the emulator.
        long _lastTick = 0;

        /// <summary>
        /// Handles the timer ticks.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnAnimationTimer(object o, EventArgs e)
        {
            // Stop the timer while this frame is being processed.
            _animationTimer.Stop();

            // Figure out how much time has gone by since the timer was started.
            long ms = ((DateTime.UtcNow.Ticks - _lastTick) / 10000);

            // Set the last tick to now.
            _lastTick = DateTime.UtcNow.Ticks;

            // Figure out how many frames should have been displayed by now.
            int increment = (int)(ms / timerInterval);

            // If the timer is being serviced in less time than the minimum, just 
            // process the frame.  Otherwise, if we have gone beyond the maxStep,
            // just move the frame to maxStep.
            if (increment < 1)
                increment = 1;
            else if (increment > maxStep)
                increment = maxStep;

            // Increment _animationStep based on which direction we are going.
            if (_animationStep < 0)
                _animationStep += increment;
            else if (_animationStep > 0)
                _animationStep -= increment;

            // This will trigger another OnRender and kick off the timer again to 
            // take the next step in the animation.
            Invalidate();
        }

        /// <summary>
        /// Override this method if you want to hard-code the size of your control.
        /// </summary>
        /// <param name="availableWidth"></param>
        /// <param name="availableHeight"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight)
        {
            desiredWidth = _width;
            desiredHeight = _height;
        }
    }

    /// <summary>
    /// This control class is the MenuItem that handles drawing a menu item.
    /// </summary>
    internal sealed class MenuItem : Control
    {
        // Private members

        // Small image because you can't stretch an image smaller, only larger.
        private Bitmap _imageSmall;

        // Larger version so it looks good instead of stretching the small one.
        private Bitmap _image;

        // Description of this MenuItem.
        private string _description;

        // Array of widths to save time by pre-calculating them.
        private int[] _widthSteps;

        // Array of heights to save time by pre-calculating them.
        private int[] _heightSteps;

        // Width of large image.
        private int _largeWidth;

        // Height of large image.
        private int _largeHeight;

        /// <summary>
        /// The default constructor.
        /// </summary>
        public MenuItem()
        {
        }

        /// <summary>
        /// This constructor gets all the pieces at once.
        /// </summary>
        /// <param name="rBitmapSmall"></param>
        /// <param name="rBitmap"></param>
        /// <param name="description"></param>
        /// <param name="rLargeSizeBitmap"></param>
        public MenuItem(Resource.BitmapResources rBitmapSmall, Resource.BitmapResources rBitmap, string description, Resource.BitmapResources rLargeSizeBitmap)
        {
            // Get the images from the resource manager.
            _imageSmall = Resource.GetBitmap(rBitmapSmall);
            _image = Resource.GetBitmap(rBitmap);

            // Set the description.
            _description = description;

            // Create the step arrays for zooming in and out.
            _widthSteps = new int[MenuItemPanel.maxStep];
            _heightSteps = new int[MenuItemPanel.maxStep];

            // Get the difference in size between the large and small images.
            int wDiff = _image.Width - _imageSmall.Width;
            int hDiff = _image.Height - _imageSmall.Height;

            // Pre-calculate the width and height values for scaling the image.
            for (int i = 1; i < MenuItemPanel.maxStep; i++)
            {
                _widthSteps[i] = (wDiff / MenuItemPanel.maxStep) * i;
                _heightSteps[i] = (hDiff / MenuItemPanel.maxStep) * i;
            }

            // Set the large width and height based on one of the main icons.
            Bitmap bmp = Resource.GetBitmap(rLargeSizeBitmap);
            _largeWidth = bmp.Width;
            _largeHeight = bmp.Height;
        }

        /// <summary>
        /// Public accessor method for the description.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// We are not overriding the OnRender method of the class, but are simply 
        /// creating a render method that can be called by the menu container.
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        public void Render(DrawingContext dc, int x, int y, int scale)
        {
            // Make sure we have all of the proper images.
            if (_image != null && _imageSmall != null)
            {
                // If the scale is at maxStep, use the larger image.  Otherwise, use 
                // the scale value to scale from the smaller image to something 
                // bigger.
                if (scale == MenuItemPanel.maxStep)
                {
                    Width = _image.Width;
                    Height = _image.Height;
                    dc.DrawImage(_image, x, y);
                }
                else
                {
                    // If the scale is 0, draw the small bitmap.  Otherwise, 
                    // calculate the difference between the small and large bitmaps
                    // and stretch the small bitmap.
                    if (scale == 0)
                    {
                        Width = _imageSmall.Width;
                        Height = _imageSmall.Height;
                        x += ((_largeWidth - Width) / 2);
                        y += ((_largeHeight - Height) / 2);
                        dc.DrawImage(_imageSmall, x, y);
                    }
                    else
                    {
                        int wDiff = _image.Width - _imageSmall.Width;
                        int hDiff = _image.Height - _imageSmall.Height;

                        Width = _imageSmall.Width + _widthSteps[scale];
                        Height = _imageSmall.Height + _heightSteps[scale];
                        x += ((_largeWidth - Width) / 2);
                        y += ((_largeHeight - Height) / 2);
                        dc.Bitmap.StretchImage(x, y, _imageSmall, Width, Height, 255);
                    }
                }
            }
        }
    }
}