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

namespace ComputerGraphicsProject1
{
    /// <summary>
    /// Interaction logic for CustomConvolutionFilter.xaml
    /// </summary>
    public partial class CustomConvolutionFilter : Page
    {
        int currentNumRows = 0;
        int currentNumCol = 0;
        bool pivotChanging = false;
        TextBox currentPivot = null;
        WriteableBitmap bitmap = BasicFilters.bitmap;
        BitmapSource bitmapSource = null;
        public CustomConvolutionFilter()
        {

            InitializeComponent();
            photoImage.Source = bitmap;
            addRowColumnToMatrix(3, 3);
        }
        public void addRowColumnToMatrix(int width, int height)
        {
            for (int i = 0; i < (width - currentNumCol); i++)
            {
                matrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < (height - currentNumRows); i++)
            {
                matrixGrid.RowDefinitions.Add(new RowDefinition());
            }
            var brush = new SolidColorBrush { Color = Colors.Red };
            for (int i = currentNumCol; i < width; i++)
            {
                for (int j = 0; j < currentNumRows; j++)
                {
                    TextBox textbox = new TextBox();
                    textbox.Margin = new Thickness(3);
                    textbox.Background = brush;
                    textbox.SetValue(Grid.ColumnProperty, i);
                    textbox.SetValue(Grid.RowProperty, j);
                    textbox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    textbox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    textbox.FontSize = 25;
                    textbox.Text = "0";
                    textbox.PreviewMouseDown += pivotChanging_Click;
                    matrixGrid.Children.Add(textbox);
                }
            }
            currentNumCol = width;
            for (int i = currentNumRows; i < height; i++)
            {
                for (int j = 0; j < currentNumCol; j++)
                {
                    TextBox textbox = new TextBox();
                    textbox.Margin = new Thickness(3);
                    textbox.Background = brush;
                    textbox.SetValue(Grid.ColumnProperty, j);
                    textbox.SetValue(Grid.RowProperty, i);
                    textbox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    textbox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    textbox.FontSize = 25;
                    textbox.Text = "0";
                    textbox.PreviewMouseDown += pivotChanging_Click;
                    matrixGrid.Children.Add(textbox);
                }
            }

            currentNumRows = height;
        }

        private void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            addRowColumnToMatrix(currentNumCol, currentNumRows + 1);
        }

        private void addColumnButton_Click(object sender, RoutedEventArgs e)
        {
            addRowColumnToMatrix(currentNumCol + 1, currentNumRows);
        }

        private void pivotChanging_Click(object sender, RoutedEventArgs e)
        {
            if (pivotChanging == true)
            {
                if (currentPivot != null)
                {
                    currentPivot.Background = new SolidColorBrush { Color = Colors.Red };
                }
                TextBox textbox = (TextBox)sender;
                textbox.Background = new SolidColorBrush { Color = Colors.Coral };
                currentPivot = textbox;
                pivotChangingLabel.Visibility = Visibility.Collapsed;
                pivotChanging = false;
            }
        }

        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < currentNumCol; j++)
            {
                for (int i = 0; i < matrixGrid.Children.Count; i++)
                    if ((Grid.GetRow(matrixGrid.Children[i]) == currentNumRows - 1)
                        && (Grid.GetColumn(matrixGrid.Children[i]) == j))
                    {
                        matrixGrid.Children.Remove(matrixGrid.Children[i]);
                        break;
                    }
            }
            matrixGrid.RowDefinitions.RemoveAt(currentNumRows - 1);
            currentNumRows -= 1;
        }

        private void deleteColumnButton_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < currentNumRows; j++)
            {
                for (int i = 0; i < matrixGrid.Children.Count; i++)
                    if ((Grid.GetRow(matrixGrid.Children[i]) == j)
                        && (Grid.GetColumn(matrixGrid.Children[i]) == currentNumCol - 1))
                    {
                        matrixGrid.Children.Remove(matrixGrid.Children[i]);
                        break;
                    }
            }
            matrixGrid.ColumnDefinitions.RemoveAt(currentNumCol - 1);
            currentNumCol -= 1;
        }

        private void changePivotButton_Click(object sender, RoutedEventArgs e)
        {
            pivotChanging = true;
            pivotChangingLabel.Visibility = Visibility.Visible;
        }

        private void pivotChangingLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pivotChanging = false;
            pivotChangingLabel.Visibility = Visibility.Collapsed;
        }

        private void applyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            int pivotY = (int)pivotChangingLabel.GetValue(Grid.RowProperty);
            int pivotX = (int)pivotChangingLabel.GetValue(Grid.ColumnProperty);
            int[] pivot = new int[] { pivotX, pivotY };
            int[][] customFilter = getCustomFilter();
            int divisorValue = int.Parse(divisorValueTextBox.Text);
            int offsetValue = int.Parse(offsetValueTextBlock.Text);
            bitmapSource = BasicFilters.convolutionFunction(customFilter, divisorValue, offsetValue, pivot);
            photoImage.Source = bitmapSource;
            mainGrid.ColumnDefinitions.ElementAt(0).Width = new GridLength(7, GridUnitType.Star);
            mainGrid.ColumnDefinitions.ElementAt(1).Width = new GridLength(2, GridUnitType.Star);
            editFilter.Visibility = Visibility.Visible;
        }
        private int[][] getCustomFilter()
        {
            int[][] customFilter = new int[currentNumCol][];
            for (int i = 0; i < currentNumCol; i++)
            {
                customFilter[i] = new int[currentNumRows];
                for (int j = 0; j < currentNumRows; j++)
                {
                    TextBox textBox = matrixGrid.Children.Cast<TextBox>().First(e => Grid.GetRow(e) == j && Grid.GetColumn(e) == i);
                    int textBoxValue = int.Parse(textBox.Text);
                    customFilter[i][j] = textBoxValue;
                }
            }

            return customFilter;
        }
        private void editFilter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            editFilter.Visibility = Visibility.Collapsed;
            mainGrid.ColumnDefinitions.ElementAt(0).Width = new GridLength(2, GridUnitType.Star);
            mainGrid.ColumnDefinitions.ElementAt(1).Width = new GridLength(7, GridUnitType.Star);
        }

        private void editFilter_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush { Color = Colors.Coral };
            editFilter.Background = brush;
        }

        private void editFilter_MouseLeave(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush { Color = Colors.AliceBlue };
            editFilter.Background = brush;

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Functions.CreateThumbnail("meinPhoto.png", bitmapSource);
        }


        private void defaultFiltersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentNumCol > 3)
            {
                int tmp = currentNumCol;
                for (int k = 0; k < tmp - 3; k++)
                {
                    for (int j = 0; j < currentNumRows; j++)
                    {
                        for (int i = 0; i < matrixGrid.Children.Count; i++)
                            if ((Grid.GetRow(matrixGrid.Children[i]) == j)
                                && (Grid.GetColumn(matrixGrid.Children[i]) == currentNumCol - 1))
                            {
                                matrixGrid.Children.Remove(matrixGrid.Children[i]);
                                break;
                            }
                    }
                    matrixGrid.ColumnDefinitions.RemoveAt(currentNumCol - 1);
                    currentNumCol -= 1;
                }
            }
            else
            {
                int tmp = currentNumCol;
                for (int k = 0; k < (tmp - 3) * -1; k++)
                {
                    addRowColumnToMatrix(currentNumCol + 1, currentNumRows);
                }

            }
            if (currentNumRows > 3)
            {
                int tmp = currentNumRows;
                for (int k = 0; k < tmp - 3; k++)
                {
                    for (int j = 0; j < currentNumCol; j++)
                    {
                        for (int i = 0; i < matrixGrid.Children.Count; i++)
                            if ((Grid.GetRow(matrixGrid.Children[i]) == currentNumRows - 1)
                                && (Grid.GetColumn(matrixGrid.Children[i]) == j))
                            {
                                matrixGrid.Children.Remove(matrixGrid.Children[i]);
                                break;
                            }
                    }
                    matrixGrid.RowDefinitions.RemoveAt(currentNumRows - 1);
                    currentNumRows -= 1;
                }
            }
            else
            {
                int tmp = currentNumRows;
                for (int k = 0; k < (tmp - 3) * -1; k++)
                {
                    addRowColumnToMatrix(currentNumCol, currentNumRows + 1);
                }
            }
            int selected = defaultFiltersComboBox.SelectedIndex;
            if (selected == 0)
            {
                int[][] filter = new int[3][];
                filter = BasicFilters.createBlurFilter(filter);
                setTextBoxValues(filter);
            }
            if (selected == 1)
            {
                int[][] filter = new int[3][];
                filter = BasicFilters.createGaussianSmothingFilter(filter);
                setTextBoxValues(filter);
            }
            if (selected == 2)
            {
                int[][] filter = new int[3][];
                filter = BasicFilters.createSharpeningFilter(filter);
                setTextBoxValues(filter);
            }
            if (selected == 3)
            {
                int[][] filter = new int[3][];
                filter = BasicFilters.createEmbosSouthFilter(filter);
                setTextBoxValues(filter);
            }
            if (selected == 4)
            {
                int[][] filter = new int[3][];
                filter = BasicFilters.createedgeDetectionHorizontalFilter(filter);
                setTextBoxValues(filter);
            }
        }

        public void setTextBoxValues(int[][] filter)
        {

            for (int j = 0; j < currentNumCol; j++)
            {
                for (int k = 0; k < currentNumRows; k++)
                {
                    for (int i = 0; i < matrixGrid.Children.Count; i++)
                    {
                        if ((Grid.GetRow(matrixGrid.Children[i]) == k)
                            && (Grid.GetColumn(matrixGrid.Children[i]) == j))
                        {
                            TextBox tmp = (TextBox)matrixGrid.Children[i];
                            tmp.Text = filter[j][k].ToString();
                        }
                    }
                }
            }
        }
    }
}
