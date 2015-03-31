﻿using Microsoft.Win32;
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
using System.Drawing;

namespace ComputerGraphicsProject1
{
    /// <summary>
    /// Interaction logic for BasicFilters.xaml
    /// </summary>
    public partial class BasicFilters : Page
    {
        /*************Seconnd project variables*****************/
        public List<Octree> nonLeavesDepth1 = new List<Octree>();
        public List<Octree> nonLeavesDepth2 = new List<Octree>();
        public List<Octree> nonLeavesDepth3 = new List<Octree>();
        public List<Octree> nonLeavesDepth4 = new List<Octree>();
        public List<Octree> nonLeavesDepth5 = new List<Octree>();
        public List<Octree> nonLeavesDepth6 = new List<Octree>();
        public List<Octree> nonLeavesDepth7 = new List<Octree>();
        public int numberOfLeaves = 0;
        /*******************************************************/
        #region first project
        int prevSliderValue = 0;
        public static WriteableBitmap bitmap;
        public BasicFilters()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush { Color = Colors.Coral };
            beginningLabel.Background = brush;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush { Color = Colors.Aqua };
            beginningLabel.Background = brush;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();

            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            // op.Multiselect = true;
            if (op.ShowDialog() == true)
            {

                string path = op.FileName;
                string name = op.SafeFileName;
                {
                    try
                    {
                        Image myimage = new Image();

                        myimage.Source = new BitmapImage(new Uri(path));
                        bitmap = new WriteableBitmap(new BitmapImage(new Uri(path)));
                        photoImage.Source = myimage.Source;
                        beginningLabel.Visibility = Visibility.Collapsed;
                        negationButton.IsEnabled = true;

                    }
                    catch
                    {
                        MessageBox.Show("Please choose correct image");
                    }
                }
            }
        }


        private void negationButton_Click(object sender, RoutedEventArgs e)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    int index = y * stride + 4 * x;
                    int tmp;
                    //red
                    byte red = pixels[index];
                    tmp = 255 - red;
                    pixels[index] = Convert.ToByte(tmp);
                    //green
                    byte green = pixels[index + 1];
                    tmp = 255 - green;
                    pixels[index+1] = Convert.ToByte(tmp);
                    //blue
                    byte blue = pixels[index + 2];
                    tmp = 255 - blue;
                    pixels[index+2] = Convert.ToByte(tmp);
                    //alpha
                    byte alpha = pixels[index + 3];
                    //tmp = 255 - alpha;
                    //pixels[index] = Convert.ToByte(tmp);
                }
            }
            prevSliderValue = 0;
            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap;
        }

        private void brightnessSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            int sliderValue = (int)brightnessSlider.Value;
           // if (sliderValue > prevSliderValue)
           // {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    for (int x = 0; x < bitmap.PixelWidth; x++)
                    {
                        int index = y * stride + 4 * x;
                        int tmp;
                        //red
                        byte red = pixels[index];
                        if (red > (255 - sliderValue))
                        {
                            pixels[index] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index] = Convert.ToByte(red + sliderValue);
                        }
                        //green
                        byte green = pixels[index + 1];
                        if (green > (255 - brightnessSlider.Value))
                        {
                            pixels[index + 1] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index + 1] = Convert.ToByte(green + sliderValue);
                        }
                        //blue
                        byte blue = pixels[index + 2];
                        if (blue > (255 - brightnessSlider.Value))
                        {
                            pixels[index + 2] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index + 2] = Convert.ToByte(blue + sliderValue);
                        }
                        //alpha
                        byte alpha = pixels[index + 3];
                    }
                }
            prevSliderValue = sliderValue;
            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap2;
        }

        private void contrastSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            int contrastValue = (int)contrastSlider.Value;
            if (contrastValue < 0)
            {
                contrastValue *= -1;
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    for (int x = 0; x < bitmap.PixelWidth; x++)
                    {
                        int index = y * stride + 4 * x;
                        int tmp;
                        //red
                        byte red = pixels[index];
                        tmp = (int)((float)red / 255 * (255 - 2 * contrastValue));
                        pixels[index] = Convert.ToByte(tmp + contrastValue);
                        //green
                        byte green = pixels[index + 1];
                        tmp = (int)((float)green / 255 * (255 - 2 * contrastValue));
                        pixels[index + 1] = Convert.ToByte(tmp + contrastValue);
                        //blue
                        byte blue = pixels[index + 2];
                        tmp = (int)((float)blue / 255 * (255 - 2 * contrastValue));
                        pixels[index + 2] = Convert.ToByte(tmp + contrastValue);
                        //alpha
                        byte alpha = pixels[index + 3];
                        //tmp = 255 - alpha;
                        //pixels[index] = Convert.ToByte(tmp);
                    }
                }
            }
            else
            {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    for (int x = 0; x < bitmap.PixelWidth; x++)
                    {
                        int index = y * stride + 4 * x;
                        int tmp;
                        //red
                        byte red = pixels[index];
                        tmp = (int)((float)red / 255 * (255 + 2 * contrastValue));
                        int tmp2 = tmp - contrastValue;
                        if (tmp2 < 0)
                        {
                            pixels[index] = Convert.ToByte(0);
                        }
                        else if (tmp2 > 255)
                        {
                            pixels[index] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index] = Convert.ToByte(tmp - contrastValue);
                        }
                        //green
                        byte green = pixels[index + 1];
                        tmp = (int)((float)green / 255 * (255 + 2 * contrastValue));
                        tmp2 = tmp - contrastValue;
                        if (tmp2 < 0)
                        {
                            pixels[index+1] = Convert.ToByte(0);
                        }
                        else if (tmp2 > 255)
                        {
                            pixels[index+1] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index+1] = Convert.ToByte(tmp - contrastValue);
                        }
                        //blue
                        byte blue = pixels[index + 2];
                        tmp = (int)((float)blue / 255 * (255 + 2 * contrastValue));
                        tmp2 = tmp - contrastValue;
                        if (tmp2 < 0)
                        {
                            pixels[index+2] = Convert.ToByte(0);
                        }
                        else if (tmp2 > 255)
                        {
                            pixels[index+2] = Convert.ToByte(255);
                        }
                        else
                        {
                            pixels[index+2] = Convert.ToByte(tmp - contrastValue);
                        }
                        //alpha
                        byte alpha = pixels[index + 3];
                        //tmp = 255 - alpha;
                        //pixels[index] = Convert.ToByte(tmp);
                    }
                }
            }
            prevSliderValue = 0;
            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap2;
        }
        public static WriteableBitmap convolutionFunction(int[][] filter,int valueD=0,int offset=0,int[] pivot = null)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            byte[] pixels2 = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            int filterWidth = filter.GetLength(0);
            int filterHeight = filter[0].GetLength(0);
            int pivotX = 0;
            int pivotY = 0;
            if (pivot == null)
            {
                pivotX = ((filterWidth-1) / 2);
                pivotY = ((filterHeight-1) / 2);
            }
            else
            {
                pivotX = pivot[0];
                pivotY = pivot[1];
            }
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {

                    int sumRed=0;
                    int sumGreen=0;
                    int sumBlue=0;
                    //weight of vector
                    int sumD=0;
                    int index = (y) * stride + 4 * x;
                    for (int i = 0; i < filterWidth; i++)
                    {
                        for (int j = 0; j < filterHeight; j++)
                        {
                            int tmpIndex = (y + j - pivotY) * stride + 4 * (x + i -pivotX);
                            if (!((y + j - pivotY) < 0 || (y + j - pivotY) > bitmap.PixelHeight || (x + i - pivotX) < 0 || (x + i - pivotX) > bitmap.PixelWidth || tmpIndex >= pixels.Length) )
                            {
                                sumRed += filter[i][j] * pixels[tmpIndex];
                                sumGreen += filter[i][j] * pixels[tmpIndex + 1];
                                sumBlue += filter[i][j] * pixels[tmpIndex + 2];
                                sumD += filter[i][j];
                            }
                        }
                    }
                    if (sumD == 0)
                    {
                        sumD = 1;
                    }
                    if (valueD != 0)
                    {
                        sumD = valueD;
                    }
                    sumRed = (sumRed / sumD) + offset;
                    sumGreen = (sumGreen / sumD) + offset;
                    sumBlue = (sumBlue / sumD) + offset;
                    index = y * stride + 4 * x;
                    int tmp;
                    //red
                    byte red = pixels[index];
                    if (sumRed > 255)
                    {
                        pixels2[index] = Convert.ToByte(255);
                    }
                    else if (sumRed < 0)
                    {
                        pixels2[index] = Convert.ToByte(0);
                    }
                    else
                    pixels2[index] = Convert.ToByte(sumRed);
                    //green
                    byte green = pixels[index + 1];
                    if (sumGreen > 255)
                    {
                        pixels2[index+1] = Convert.ToByte(255);
                    }
                    else if (sumGreen < 0)
                    {
                        pixels2[index+1] = Convert.ToByte(0);
                    }
                    else
                    pixels2[index + 1] = Convert.ToByte(sumGreen);
                    //blue
                    byte blue = pixels[index + 2];
                    if (sumBlue > 255)
                    {
                        pixels2[index+2] = Convert.ToByte(255);
                    }
                    else if (sumBlue < 0)
                    {
                        pixels2[index+2] = Convert.ToByte(0);
                    }
                    else
                    pixels2[index + 2] = Convert.ToByte(sumBlue);
                    //alpha
                    byte alpha = pixels[index + 3];
                    //tmp = 255 - alpha;
                    //pixels[index] = Convert.ToByte(tmp);
                }
            }
            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels2, stride, 0);
            return bitmap2;
        }

        private void blurButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] blurFilter = new int[3][];
            blurFilter = createBlurFilter(blurFilter);
             photoImage.Source = convolutionFunction(blurFilter,9);
        }

        private void gaussianSmothingButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] gaussianSmothingFilter = new int[3][];
            gaussianSmothingFilter = createGaussianSmothingFilter(gaussianSmothingFilter);       
            photoImage.Source = convolutionFunction(gaussianSmothingFilter, 8);
        }

        private void sharpeningButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] sharpeningFilter = new int[3][];
            sharpeningFilter = createSharpeningFilter(sharpeningFilter);
            photoImage.Source = convolutionFunction(sharpeningFilter, 1);
        }

        private void embossSouthButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] embossSouthFilter = new int[3][];
            embossSouthFilter = createEmbosSouthFilter(embossSouthFilter);
          
            photoImage.Source = convolutionFunction(embossSouthFilter);
        }

        private void edgeDetectionHorizonatlButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] edgeDetectionHorizontalFilter = new int[3][];

            edgeDetectionHorizontalFilter = createedgeDetectionHorizontalFilter(edgeDetectionHorizontalFilter);

            photoImage.Source = convolutionFunction(edgeDetectionHorizontalFilter, 1, 127);
        }
        public static int[][] createBlurFilter(int[][] blurFilter)
        {
            for (int i = 0; i < 3; i++)
            {
                blurFilter[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    blurFilter[i][j] = 1;
                }

            }
            return blurFilter;
        }
        public static int[][] createGaussianSmothingFilter(int[][] gaussianSmothingFilter)
        {
            for (int i = 0; i < 3; i++)
            {
                gaussianSmothingFilter[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    gaussianSmothingFilter[i][j] = 0;
                }
            }
            gaussianSmothingFilter[0][1] = 1;
            gaussianSmothingFilter[1][0] = 1;
            gaussianSmothingFilter[1][1] = 4;
            gaussianSmothingFilter[1][2] = 1;
            gaussianSmothingFilter[2][1] = 1;
            return gaussianSmothingFilter;
        }
        public static int[][] createSharpeningFilter(int[][] sharpeningFilter)
        {
            for (int i = 0; i < 3; i++)
            {
                sharpeningFilter[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    sharpeningFilter[i][j] = 0;
                }
            }
            sharpeningFilter[0][1] = -1;
            sharpeningFilter[1][0] = -1;
            sharpeningFilter[1][1] = 5;
            sharpeningFilter[1][2] = -1;
            sharpeningFilter[2][1] = -1;
            return sharpeningFilter;
        }
        public static int[][] createEmbosSouthFilter(int[][] embossSouthFilter)
        {
            for (int i = 0; i < 3; i++)
            {
                embossSouthFilter[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    embossSouthFilter[i][j] = 0;
                }
            }
            embossSouthFilter[0][0] = -1;
            embossSouthFilter[1][0] = -1;
            embossSouthFilter[0][1] = -1;
            embossSouthFilter[1][1] = 1;
            embossSouthFilter[1][2] = 1;
            embossSouthFilter[2][1] = 1;
            embossSouthFilter[2][2] = 1;
            return embossSouthFilter;
        }
        public static int[][] createedgeDetectionHorizontalFilter(int[][] filter)
        {
            for (int i = 0; i < 3; i++)
            {
                filter[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    filter[i][j] = 0;
                }
            }
            filter[0][1] = -1;
            filter[1][1] = 1;
            return filter;
        }

        private void customFilterButton_Click(object sender, RoutedEventArgs e)
        {
            CustomConvolutionFilter customConvolutionFIlter = new CustomConvolutionFilter();
            MainWindow.window.Content = customConvolutionFIlter;
        }

        private void gammaSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            double intensity;
            double power;
            bitmap.CopyPixels(pixels, stride, 0);
            double sliderValue = (double)gammaSlider.Value;
            // if (sliderValue > prevSliderValue)
            // {
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    int index = y * stride + 4 * x;
                    int tmp;
                    //red
                    byte red = pixels[index];
                    intensity = 1 * ((double)red / 255);
                    power = Math.Pow(intensity, sliderValue);
                    red = (Byte)(255.0 * power);
                    pixels[index] = Convert.ToByte(red);
                    
                    //green
                    byte green = pixels[index + 1];
                    intensity = 1 * ((double)green / 255);
                    power = Math.Pow(intensity, sliderValue);
                    green = (Byte)(255.0 * power);
                        pixels[index + 1] = Convert.ToByte(green);

                    //blue
                    byte blue = pixels[index + 2];
                    intensity = 1 * ((double)blue / 255);
                    power = Math.Pow(intensity, sliderValue);
                    blue = (Byte)(255.0 * power);
                        pixels[index + 2] = Convert.ToByte(blue);
                    
                    //alpha
                    byte alpha = pixels[index + 3];
                }
            }
            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap2;
        }

        private void ditheringButton_Click(object sender, RoutedEventArgs e)
        {
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    int index = y * stride + 4 * x;
                    //red
                    byte red = pixels[index];
                    if (red >= 220)
                    {
                        pixels[index] = Convert.ToByte(255);
                        pixels[index + 1] = Convert.ToByte(255);
                        pixels[index + 2] = Convert.ToByte(255);
                    }
                    else
                    {
                        pixels[index] = Convert.ToByte(0);
                        pixels[index + 1] = Convert.ToByte(0);
                        pixels[index + 2] = Convert.ToByte(0);
                    }
                }
            }
            prevSliderValue = 0;
            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap;
        }
        #endregion 
        /*
         * 
         * 
         * 
         *          SECOND PROJECT
         * 
         * 
         * 
         * */
        private void randomDitheringLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int numberOfGrayLevels = 0;
            float colorStep = 0;
            Random rand = new Random();
            int selected = randomDitheringLevelComboBox.SelectedIndex;
            if (selected == 0)
            {
                numberOfGrayLevels = 1;
            }
            else if (selected == 1)
            {
                numberOfGrayLevels = 3;
            }
            else if (selected == 2)
            {
                numberOfGrayLevels = 7;
            }
            else
            {
                numberOfGrayLevels = 15;
            }
            colorStep = (float)255 / numberOfGrayLevels;

            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            int randomNumber = 0;
            int newColor = 0;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    randomNumber = rand.Next(0, (int)colorStep);
                    int index = y * stride + 4 * x;
                    //red
                    byte red = pixels[index];
                    int bottomLevel = red / (int)colorStep;
                    if (bottomLevel != 0 && ((float)bottomLevel * colorStep + colorStep / (float)2) > red)
                    {
                        bottomLevel--;
                    }
                    if ((colorStep * bottomLevel + randomNumber) < red) { newColor = (int)((float)(bottomLevel + 1) * colorStep); }
                    else { newColor = (int)((float)bottomLevel * colorStep); }
                    pixels[index] = Convert.ToByte(newColor);
                    pixels[index + 1] = Convert.ToByte(newColor);
                    pixels[index + 2] = Convert.ToByte(newColor);
                }
            }
            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap2;
        }

        /// <summary>
        /// Przy dodawaniu nowego węzła przypisuję wartości red,green,blue do sumRed,sumGreen,sumBlue. Gdy nowo dodany kolor przejdzie przez dany węzeł, wartości red,green,blue
        /// zostaną dodane do sumRed,sumGreen,sumBlue oraz wartość countera zostanie zwiększona o jeden. Dzięki temu że wartości te zostają przypisywane na bieżąco przy redukcji 
        /// węzła usuwam tylko jego dzieci posiadając już sumaryczną wartość kanałów RGB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void octreeColorQuantization_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            /*********************** VARIABLES **************************/
            int NUMBER_OF_COLORS = (int)octreeColorQuantizationSlider.Value;
            Octree rootOctree = new Octree();
            rootOctree.isLeaf = false;
            Octree tmpOctree;
            int index;
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);
            byte red;
            byte green;
            byte blue;
            int redCounter;
            int greenCounter;
            int blueCounter;
            string redBinary;
            string greenBinary;
            string blueBinary;
            int bitSum = 0;
            /*************************************************************/
            
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    tmpOctree = rootOctree;
                    index = y * stride + 4 * x;
                    red = pixels[index];
                    green = pixels[index + 1];
                    blue = pixels[index + 2];
                    redBinary = Convert.ToString(red, 2);
                    greenBinary = Convert.ToString(green, 2);
                    blueBinary = Convert.ToString(blue, 2);
                    redCounter = 0;
                    greenCounter = 0;
                    blueCounter = 0;
                    for (int i = 0; i < 8; i++)
                    {      
                        if (!(redBinary.Length < 8 -i))
                        {
                            bitSum = bitSum + 4 * (int)char.GetNumericValue(redBinary.ElementAt(redCounter));
                            redCounter++;
                        }
                        if (!(greenBinary.Length < 8 - i))
                        {
                            bitSum = bitSum + 2 * (int)char.GetNumericValue(greenBinary.ElementAt(greenCounter));
                            greenCounter++;
                        }
                        if (!(blueBinary.Length < 8 - i))
                        {
                            bitSum = bitSum + 1 * (int)char.GetNumericValue(blueBinary.ElementAt(blueCounter));
                            blueCounter++;
                        }

                        if (tmpOctree.octreeLeaves[bitSum] == null)
                        {
                            tmpOctree.octreeLeaves[bitSum] = new Octree();
                            tmpOctree.octreeLeaves[bitSum].parent = tmpOctree;
                            tmpOctree.notNullLeaves++;
                            tmpOctree = tmpOctree.octreeLeaves[bitSum];
                            tmpOctree.nodeNumber = bitSum;
                            tmpOctree.counter++;
                            tmpOctree.sumRed = red;
                            tmpOctree.sumGreen = green;
                            tmpOctree.sumBlue = blue;
                            if (i != 7)
                            {
                                tmpOctree.isLeaf = false;
                            }

                            AddNodeToNonLeafList(tmpOctree, i);

                            leafCounter(rootOctree);
                            int infiniteLoop = 0;
                            while (numberOfLeaves > NUMBER_OF_COLORS)
                            {
                                infiniteLoop++;
                                if (infiniteLoop > 100000)
                                {
                                    MessageBox.Show("infnite loop (probably)");
                                }
                                reduceTree();
                                numberOfLeaves = 0;
                                leafCounter(rootOctree);
                            }
                            numberOfLeaves = 0;                               
                        }
                        else
                        {
                            tmpOctree = tmpOctree.octreeLeaves[bitSum];
                            tmpOctree.counter++;
                            tmpOctree.sumRed += red;
                            tmpOctree.sumGreen += green;
                            tmpOctree.sumBlue += blue;
                        }
                        bitSum = 0;
                    }                  
                }
            }


            AssignColorsToPixels(rootOctree, NUMBER_OF_COLORS, stride, pixels);

            /*************reseting global values ***********************/
            nonLeavesDepth2 = new List<Octree>();
            nonLeavesDepth3 = new List<Octree>();
            nonLeavesDepth4 = new List<Octree>();
            nonLeavesDepth5 = new List<Octree>();
            nonLeavesDepth6 = new List<Octree>();
            nonLeavesDepth7 = new List<Octree>();
            numberOfLeaves = 0;
            /***********************************************************/

            WriteableBitmap bitmap2 = new WriteableBitmap(bitmap);
            bitmap2.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, stride, 0);
            photoImage.Source = bitmap2;
        }
        /// <summary>
        /// Function assigning colors to pixels
        /// </summary>
        /// <param name="rootOctree"></param>
        /// <param name="NUMBER_OF_COLORS"></param>
        /// <param name="stride"></param>
        /// <param name="pixels"></param>
        public void AssignColorsToPixels(Octree rootOctree,int NUMBER_OF_COLORS,int stride,byte[] pixels)
        {
            //getting list of colors

            List<Octree> octreeLeavesColors = new List<Octree>();
            AddLeafNodeToList(rootOctree, octreeLeavesColors);
            byte[] colors = new byte[NUMBER_OF_COLORS * 3];

            for (int i = 0; i < octreeLeavesColors.Count - 1; i++)
            {
                colors[i * 3] = (byte)(octreeLeavesColors[i].sumRed / octreeLeavesColors[i].counter);
                colors[i * 3 + 1] = (byte)(octreeLeavesColors[i].sumGreen / octreeLeavesColors[i].counter);
                colors[i * 3 + 2] = (byte)(octreeLeavesColors[i].sumBlue / octreeLeavesColors[i].counter);
            }

            //assigning colours to pixels

            int difference = 750;
            byte[] color = new byte[3];
            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    int index = y * stride + 4 * x;
                    for (int i = 0; i < NUMBER_OF_COLORS; i++)
                    {
                        int tmpDifference = 0;
                        tmpDifference = Math.Abs(pixels[index] - colors[i * 3]) + Math.Abs(pixels[index + 1] - colors[i * 3 + 1]) + Math.Abs(pixels[index + 2] - colors[i * 3 + 2]);
                        if (tmpDifference < difference)
                        {
                            difference = tmpDifference;
                            color[0] = colors[i * 3];
                            color[1] = colors[i * 3 + 1];
                            color[2] = colors[i * 3 + 2];
                        }
                    }

                    pixels[index] = color[0];
                    pixels[index + 1] = color[1];
                    pixels[index + 2] = color[2];
                    difference = 750;
                }
            }
        }

        /// <summary>
        /// Function adding all leaves from tree to a given list. (Used after completition of tree reduction)
        /// </summary>
        /// <param name="octree"></param>
        /// <param name="list"></param>
        private void AddLeafNodeToList(Octree octree, List<Octree> list)
        {
            if (octree.isLeaf)
            {
                list.Add(octree);
                return;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (octree.octreeLeaves[i] != null)
                    {
                        AddLeafNodeToList(octree.octreeLeaves[i], list);
                    }
                }
            }
        }

        /// <summary>
        /// Adding node to a list of nodes which are not leaves (usefull in tree reducing)
        /// </summary>
        /// <param name="tmpOctree"></param>
        /// <param name="i">is a level on which a node is placed in tree</param>
        private void AddNodeToNonLeafList(Octree tmpOctree, int i)
        {
            if (i == 6)
            {
                if (!nonLeavesDepth7.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth7.Add(tmpOctree);
                }
            }
            if (i == 5)
            {
                if (!nonLeavesDepth6.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth6.Add(tmpOctree);
                }
            }
            if (i == 4)
            {
                if (!nonLeavesDepth5.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth5.Add(tmpOctree);
                }
            }
            if (i == 3)
            {
                if (!nonLeavesDepth4.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth4.Add(tmpOctree);
                }
            }
            if (i == 2)
            {
                if (!nonLeavesDepth3.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth3.Add(tmpOctree);
                }
            }
            if (i == 1)
            {
                if (!nonLeavesDepth2.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth2.Add(tmpOctree);
                }
            }
            if (i == 0)
            {
                if (!nonLeavesDepth1.Exists(tmp => tmp == tmpOctree))
                {
                    nonLeavesDepth1.Add(tmpOctree);
                }
            }
        }
        /// <summary>
        /// Function returning a non empty list containing nonleaf nodes from the highest possible level of tree
        /// </summary>
        private List<Octree> selectDepthLevelToReduce()
        {
            if (nonLeavesDepth7.Count > 0)
            {
                return nonLeavesDepth7;
            }
            else if (nonLeavesDepth6.Count > 0)
            {
                return nonLeavesDepth6;
            }
            else if (nonLeavesDepth5.Count > 0)
            {
                return nonLeavesDepth5;
            }
            else if (nonLeavesDepth4.Count > 0)
            {
                return nonLeavesDepth4;
            }
            else if (nonLeavesDepth3.Count > 0)
            {
                return nonLeavesDepth3;
            }
            else if (nonLeavesDepth2.Count > 0)
            {
                return nonLeavesDepth2;
            }
            else if (nonLeavesDepth1.Count > 0)
            {
                return nonLeavesDepth1;
            }
            return null;
        }

        /// <summary>
        /// Function reducing tree
        /// </summary>
        private void reduceTree()
        {
            int index = 0;
            int count = 10000000;
            Octree octreeNodeToReduce = new Octree();
            List<Octree> octreeList = selectDepthLevelToReduce();
            for (int i = 0; i < octreeList.Count ; i++)
            {
                if (octreeList[i].counter < count)
                {
                    index = i;
                    count = octreeList[i].counter;
                    octreeNodeToReduce = octreeList[i];
                }
            }
            for (int i = 0; i < 8; i++)
            {
                octreeNodeToReduce.octreeLeaves[i] = null;
            }
            octreeNodeToReduce.notNullLeaves = 0;
            octreeNodeToReduce.isLeaf = true;
            octreeList.Remove(octreeNodeToReduce);
            int ka = 0;
        }

        /// <summary>
        /// Function counting number of leaves
        /// </summary>
        private void leafCounter(Octree octree)
        {
            if (octree.isLeaf)
            {
                numberOfLeaves++;
                return;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (octree.octreeLeaves[i] != null)
                    {
                        leafCounter(octree.octreeLeaves[i]);
                    }
                }
            }
        }
        public int binToDec(string s){
        var dec = 0;
        for (int i = 0; i < s.Length; i++)
        {
            // we start with the least significant digit, and work our way to the left
            if (s[s.Length - i - 1] == '0') continue;
            dec += (int)Math.Pow(2, i);
        }
        return dec;
    }







        /********************GUI ********************************/
        private void showProjectOne_Click(object sender, RoutedEventArgs e)
        {
            if (projectOnePanel.Visibility == Visibility.Collapsed)
            {
                projectOnePanel.Visibility = Visibility.Visible;
            }
            else
            {
                projectOnePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();

            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            // op.Multiselect = true;
            if (op.ShowDialog() == true)
            {

                string path = op.FileName;
                string name = op.SafeFileName;
                {
                    try
                    {
                        Image myimage = new Image();

                        myimage.Source = new BitmapImage(new Uri(path));
                        bitmap = new WriteableBitmap(new BitmapImage(new Uri(path)));
                        photoImage.Source = myimage.Source;
                        beginningLabel.Visibility = Visibility.Collapsed;
                        negationButton.IsEnabled = true;

                    }
                    catch
                    {
                        MessageBox.Show("Please choose correct image");
                    }
                }
            }
        }
        
        }
}
