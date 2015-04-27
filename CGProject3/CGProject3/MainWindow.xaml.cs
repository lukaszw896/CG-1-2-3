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

namespace CGProject3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isSecondClick;
        public bool drawLine;
        public Point firstPoint;
        public int lineThickness = 1;
        public MainWindow()
        {
            isSecondClick = false;
            drawLine = true;
            InitializeComponent();
        }
        void MidpointLine(int x1, int y1, int x2, int y2)
        {
            bool thickenX;
            int longerAxis;
            int shorterAxis;
            int dx = x2 - x1;
            int dy = y2 - y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (dx < 0) dx1 = -1; else if (dx > 0) dx1 = 1;
            if (dy < 0) dy1 = -1; else if (dy > 0) dy1 = 1;         
            if (Math.Abs(dx) <= Math.Abs(dy))
            {
                thickenX = false;
                longerAxis = Math.Abs(dy);
                shorterAxis = Math.Abs(dx);
                if (dy < 0) dy2 = -1; else if (dy > 0) dy2 = 1;
            }
            else
            {
                thickenX = true;
                longerAxis = Math.Abs(dx);
                shorterAxis = Math.Abs(dy);
                if (dx < 0) dx2 = -1; else if (dx > 0) dx2 = 1; 
            }

            int numerator = longerAxis;
            #region line thickness
            int loopStartValue = lineThickness / 2 * (-1);
            int loopEndValue;
            if (lineThickness == 1)
            {
                loopEndValue = 1;
            }
            else
                loopEndValue = lineThickness / 2;
            #endregion 
            for (int i = 0; i <= longerAxis; i++)
            {       

                if (thickenX)
                {
                    for(int j=loopStartValue;j<loopEndValue;j++){
                        putPixel(x1, y1+j);
                    }
                }
                else
                {
                    for (int j = loopStartValue; j < loopEndValue; j++)
                    {
                        putPixel(x1+j, y1);
                    }
                }
               
                numerator += shorterAxis;
                if (numerator >= longerAxis)
                {
                    numerator -= longerAxis;
                    x1 += dx1;
                    y1 += dy1;
                }
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }

        public void DrawCircle(int x1, int y1, int x2, int y2)
        {
            int xDiff = Math.Abs(x1 - x2); 
            int yDiff = Math.Abs(y1 - y2);
            int radius = (int)Math.Sqrt(xDiff*xDiff + yDiff*yDiff); 

            int d = (5 - radius * 4) / 4;
            int x = 0;
            int y = radius;

            #region line thickness
            int loopStartValue = lineThickness / 2 * (-1);
            int loopEndValue;
            if (lineThickness == 1)
            {
                loopEndValue = 1;
            }
            else
                loopEndValue = lineThickness / 2;
            #endregion 

            do
            {

                for (int i = loopStartValue; i < loopEndValue;i++ ){
                    putPixel(x1 + x + i, y1 + y);
                    putPixel(x1 + x + i, y1 - y);
                    putPixel(x1 - x + i, y1 + y);
                    putPixel(x1 - x + i, y1 - y);
                    putPixel(x1 + y, y1 + x + i);
                    putPixel(x1 + y, y1 - x + i);
                    putPixel(x1 - y, y1 + x + i);
                    putPixel(x1 - y, y1 - x + i);
                }
                    if (d < 0)
                    {
                        d += 2 * x + 1;
                    }
                    else
                    {
                        d += 2 * (x - y) + 1;
                        y--;
                    }
                x++;
            } while (y>=x);
        }

        private void putPixel(int x, int y)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Height = 1;
            rect.Width = 1;
            rect.StrokeThickness = 1;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            myCanvas.Children.Add(rect);
        }

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (drawLine == true)
            {
                Point point = e.GetPosition(myCanvas);
                if (isSecondClick == false)
                {
                    putMarker(point);
                    firstPoint = point;
                    isSecondClick = true;
                }
                else
                {
                    putMarker(point);
                    isSecondClick = false;
                    MidpointLine((int)firstPoint.X, (int)firstPoint.Y, (int)point.X, (int)point.Y);
                }
            }
            else
            {
                Point point = e.GetPosition(myCanvas);
                if (isSecondClick == false)
                {
                    putMarker(point);
                    firstPoint = point;
                    isSecondClick = true;
                }
                else
                {
                    putMarker(point);
                    isSecondClick = false;
                    DrawCircle((int)firstPoint.X, (int)firstPoint.Y, (int)point.X, (int)point.Y);
                }
            }
           
        }

        private void putMarker(Point p)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Red);
            rect.Fill = new SolidColorBrush(Colors.Red);
            rect.Height = 7;
            rect.Width = 7;
            rect.StrokeThickness = 1;
            Canvas.SetLeft(rect, p.X-3);
            Canvas.SetTop(rect, p.Y-3);
            myCanvas.Children.Add(rect);
        }

        private void drawLineButton_Click(object sender, RoutedEventArgs e)
        {
            drawLine = true;
            isSecondClick = false;
            myCanvas.Children.Clear();
        }

        private void drawCircleButton_Click(object sender, RoutedEventArgs e)
        {
            drawLine = false;
            isSecondClick = false;
            myCanvas.Children.Clear();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)myComboBox.SelectedItem;
            int value =int.Parse( typeItem.Content.ToString());

            switch (value)
            {
                case 1:
                    lineThickness = 1;
                    break;
                case 3:
                    lineThickness = 3;
                    break;
                case 5:
                    lineThickness = 5;
                    break;
                case 7:
                    lineThickness = 7;
                    break;
            }
        }
    }
}
