using Microsoft.Win32;
using System;
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
        string curFileName;
        PhxHandling phxHandling;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void openFile_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            bool? dialogResult = ofd.ShowDialog();
            switch (dialogResult)
            {
                case (true):
                    openFile_label.Content = $"Open file: {ofd.FileName}";
                    curFileName = ofd.FileName;
                    if (!string.IsNullOrWhiteSpace(xmlModelName_input.Text)) StartHandlingOpen();
                    break;
                case (false):
                    // No file opened, dialog closed or error
                    break;
                default:
                    // gay!!
                    break;
            }
        }
        private async void StartHandlingOpen()
        {
            string modelName = xmlModelName_input.Text;
            PhxHandling handXml = await Task.Run(() => ReadXML(curFileName, modelName));
            DisplayNodes(handXml);
        }
        private async Task<PhxHandling> ReadXML(string fileName, string modelName)
        {
            phxHandling = new PhxHandling();
            phxHandling.hello = "Hello!";
            MessageBox.Show(phxHandling.hello);
            MessageBox.Show(fileName);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            Dictionary<string, string> nodes = new Dictionary<string, string>();
            string tempPlacer = null;
            string curNode = null;
            bool isAllFound = false;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    using (XmlReader reader = XmlReader.Create(fs, settings))
                    {
                        while (await reader.ReadAsync())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (isAllFound) break;
                                    curNode = reader.Name;
                                    if (reader.Name.ToLower() == "modelname")
                                        break;
                                    tempPlacer += $"&{reader.Name}";
                                    break;
                                case XmlNodeType.Text:
                                    if (isAllFound) break;
                                    tempPlacer += $",{await reader.GetValueAsync()}";
                                    break;
                                case XmlNodeType.EndElement:
                                    if (isAllFound) break;
                                    if (!reader.Name.ToLower().Equals("item"))
                                        break;
                                    else isAllFound = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                string[] tempPlaceerArr = tempPlacer.Split('&');
                foreach (string node in tempPlaceerArr)
                {
                    MessageBox.Show(node);
                    try
                    {
                        string[] tempNodeArr = node.Split(',');
                        nodes.Add(tempNodeArr[0], tempNodeArr[1]);
                    }
                    catch
                    {
                        //
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStack trace: {e.StackTrace}");
            }
            phxHandling.handlingNodes = nodes;
            return phxHandling;
        }

        private void DisplayNodes(PhxHandling handlingNodes)
        {
            foreach(Dictionary<string, string> node in handlingNodes.handlingNodes)
            {

            }
        }

        private void xmlModelName_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            // CheckTexInput(xmlModelName_input, @"pack://application:,,,/Resources/images/placeholder-modelName.png");
        }

        private void CheckTexInput(TextBox input, string path)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                MessageBox.Show("Humbugg");
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(path, UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.Stretch = Stretch.None;
                // Use the brush to paint the button's background.
                input.Background = textImageBrush;
            }
            else
            {
                input.Background = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CheckTexInput(xmlModelName_input, @"pack://application:,,,/Resources/images/placeholder-modelName.png");
        }

        private void xmlModelName_input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(curFileName)) StartHandlingOpen();
            }
        }
    }

    public struct PhxHandling
    {
        public string hello;
        public Dictionary<string, string> handlingNodes;
    }
}
