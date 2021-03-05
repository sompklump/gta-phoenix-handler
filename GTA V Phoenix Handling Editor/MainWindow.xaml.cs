using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace GTA_V_Phoenix_Handling_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stack<Func<object>> undoStack = new Stack<Func<object>>();
        bool shiftModifierIsDown = false;
        string curFileName;
        public List<UIElement> nodeGridUIElements = new List<UIElement>();
        public PhxHandling phxHandling;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openFile_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            bool? dialogResult = ofd.ShowDialog();
            switch (dialogResult)
            {
                case (true):
                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    DirectoryInfo directoryInfo = fileInfo.Directory;
                    openFile_label.Content = $"({directoryInfo.Root.Name.Replace("\\", string.Empty)}) {directoryInfo.Name}\\{ofd.SafeFileName}";
                    curFileName = ofd.FileName;

                    ReadFile readFile = new ReadFile();
                    if (!string.IsNullOrWhiteSpace(xmlModelName_input.Text)) readFile.StartHandlingOpen(xmlModelName_input.Text, curFileName);
                    break;
                case (false):
                    // No file opened, dialog closed or error
                    break;
                default:
                    break;
            }
        }

        private void xmlModelName_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTexInput(xmlModelName_input, @"pack://application:,,,/Resources/images/placeholder-modelName.png", xmlModelName_input.Text);
        }

        private void CheckTexInput(TextBox textBox, string path, string input = null)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    // Create an ImageBrush.
                    ImageBrush textImageBrush = new ImageBrush();
                    textImageBrush.ImageSource =
                        new BitmapImage(
                            new Uri(path, UriKind.Absolute)
                        );
                    textImageBrush.AlignmentX = AlignmentX.Left;
                    textImageBrush.Stretch = Stretch.None;
                    // Use the brush to paint the button's background.
                    textBox.Background = textImageBrush;
                }
                else textBox.Background = null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStack trace: {e.StackTrace}");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckTexInput(xmlModelName_input, @"pack://application:,,,/Resources/images/placeholder-modelName.png");
        }

        private void xmlModelName_input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ReadFile readFile = new ReadFile();
                if (!string.IsNullOrWhiteSpace(curFileName)) readFile.StartHandlingOpen(xmlModelName_input.Text, curFileName);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
                shiftModifierIsDown = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
                shiftModifierIsDown = false;
        }

        private void nodesGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (shiftModifierIsDown && gridScrollViewer.ScrollableWidth > 0)
            {
                if (e.Delta > 0) gridScrollViewer.LineLeft();
                else gridScrollViewer.LineRight();
            }
            else
            {
                if (e.Delta > 0) gridScrollViewer.LineUp();
                else gridScrollViewer.LineDown();
            }
        }
    }
}
