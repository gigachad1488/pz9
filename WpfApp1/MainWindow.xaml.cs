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
using System.Windows.Threading;
using System.IO;
using System.Media;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox ctext;
        SoundPlayer sound = new SoundPlayer(Properties.Resources.sham);
        RGBColor rgbcolor;
        Color color;
        bool isedit;
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 72; )
            {
                i += 9;
                textcombobox.Items.Add(i);
            }
            r1.IsChecked = true;
            media.LoadedBehavior = MediaState.Manual;
            media.Position = new TimeSpan(300000);
            media.Play();
            
            sound.PlayLooping();
            isedit = true;
            ChangeEdit();
            rgbcolor = new RGBColor();
            rgbcolor.Red = 0;
            rgbcolor.Green = 0;
            rgbcolor.Blue = 0;
            Colorlabel.Background = new SolidColorBrush(Color.FromRgb((byte)rgbcolor.Red, (byte)rgbcolor.Green, (byte)rgbcolor.Blue));
        }

        private void Clearbutton_Click(object sender, RoutedEventArgs e)
        {
            inkpanel.Strokes.Clear();
            inkpanel.Children.RemoveRange(1, inkpanel.Children.Count - 1);
        }

        private void Savebutton_Click(object sender, RoutedEventArgs e)
        {
            isedit = true;
            ChangeEdit();
            string path = @"d:\users\student\Desktop\forimags\imag.png";
            
            FileStream fileStream = new FileStream(path, FileMode.Create);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)inkpanel.Width, (int)inkpanel.Height, 96, 96, PixelFormats.Default);
            rtb.Render(inkpanel);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(fileStream);
            fileStream.Close();
            MessageBox.Show("Сохранено на : " + path);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            string name = slider.Name;
            double val = slider.Value;
            switch (name)
            {
                case "Redslider":
                    rgbcolor.Red = Convert.ToByte(val);
                    break;
                case "Greenslider":
                    rgbcolor.Green = Convert.ToByte(val);
                    break;
                case "Blueslider":
                    rgbcolor.Blue = Convert.ToByte(val);
                    break;
            }
            
            color = Color.FromRgb((byte)rgbcolor.Red, (byte)rgbcolor.Green, (byte)rgbcolor.Blue);
            Colorlabel.Background = new SolidColorBrush(Color.FromRgb((byte)rgbcolor.Red, (byte)rgbcolor.Green, (byte)rgbcolor.Blue));
            inkpanel.DefaultDrawingAttributes.Color = color;

        }

        private void Editbutton_Click(object sender, RoutedEventArgs e)
        {
            ChangeEdit();
        }

        public void ChangeEdit()
        {
            if (!isedit)
            {
                inkpanel.EditingMode = InkCanvasEditingMode.Select;
                isedit = true;
                Editbutton.Content = "выделение";

            }
            else
            {
                inkpanel.EditingMode = InkCanvasEditingMode.Ink;
                isedit = false;
                Editbutton.Content = "рисование";
            }
        }

        private void Textbutton_Click(object sender, RoutedEventArgs e)
        {
            ctext = new TextBox
            {
                Width = 100,
                Height = 60,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(5, 5, 5)),
                Margin = new Thickness(20, 20, 0, 0)
            };
            inkpanel.Children.Add(ctext);
            ctext.Focus();
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Stop();
            media.Play();
        }

        private void menuitem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            string name = but.Name;
            Brush brush = but.Background;
            inkpanel.DefaultDrawingAttributes.Color = (brush as SolidColorBrush).Color;
            Colorlabel.Background = brush;
        }

        private void r3_Checked(object sender, RoutedEventArgs e)
        {
            inkpanel.DefaultDrawingAttributes.Height = 10;
            inkpanel.DefaultDrawingAttributes.Width = 10;
        }

        private void r2_Checked(object sender, RoutedEventArgs e)
        {
            inkpanel.DefaultDrawingAttributes.Height = 5;
            inkpanel.DefaultDrawingAttributes.Width = 5;
        }

        private void r1_Checked(object sender, RoutedEventArgs e)
        {
            inkpanel.DefaultDrawingAttributes.Height = 1;
            inkpanel.DefaultDrawingAttributes.Width = 1;
        }

        private void textcombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ctext.FontSize = Convert.ToInt32(textcombobox.SelectedItem);
        }

        private void inkpanel_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {          
        }
    }
}
