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
        string curFileName;
        List<UIElement> nodeGridUIElements = new List<UIElement>();
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
                    openFile_label.Content = $"Open file: {ofd.FileName}";
                    curFileName = ofd.FileName;
                    if (!string.IsNullOrWhiteSpace(xmlModelName_input.Text)) StartHandlingOpen();
                    break;
                case (false):
                    // No file opened, dialog closed or error
                    break;
                default:
                    // Eimas er gey?!!
                    break;
            }
        }
        private async void StartHandlingOpen()
        {
            string modelName = xmlModelName_input.Text;
            Task<PhxHandling> handXml = null;
            try
            {
                handXml = await Task.Factory.StartNew(() => ReadXML(curFileName, modelName));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            while (!handXml.IsCompleted)
            {
                continue;
            }
            DisplayNodes(handXml.Result);
        }
        private async Task<PhxHandling> ReadXML(string fileName, string modelName)
        {
            phxHandling = new PhxHandling();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            bool isAllFound = false;
            bool isStarted = false;
            string curNode = null;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    using (XmlReader reader = XmlReader.Create(fs, settings))
                    {
                        while (await reader.ReadAsync())
                        {
                            if (isAllFound)
                                break;
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (isStarted)
                                        phxHandling.handlingNodes.Add(reader.Name);
                                    curNode = reader.Name;
                                    break;
                                case XmlNodeType.Text:
                                    //MessageBox.Show($"End element: {isAllFound} | Node: {await reader.GetValueAsync()}");
                                    if (curNode.ToLower() == "modelname" && isStarted == false)
                                    {
                                        isStarted = (reader.Value == modelName);
                                        break;
                                    }
                                    if (isStarted)
                                        phxHandling.handlingNodes.Add(reader.Value);
                                    break;
                                case XmlNodeType.EndElement:
                                    if ((reader.Name.ToLower() == "item" && curNode.ToLower() != "item") && isStarted == true)
                                    {
                                        isAllFound = true;
                                        isStarted = false;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStack trace: {e.StackTrace}");
            }
            return phxHandling;
        }

        private void DisplayNodes(PhxHandling phxHand)
        {
            int irow = 0;
            if (nodeGridUIElements.Count > 0)
            {
                foreach(UIElement element in nodeGridUIElements)
                {
                    nodesGrid.Children.Remove(element);
                }
            }
            nodeGridUIElements.Clear();
            nodesGrid.RowDefinitions.Clear();
            Grid.SetRow(new Label(), 0);
            foreach (string node in phxHand.handlingNodes.GetNodes())
            {
                string[] tempNodeArr = node.Split(new string[] { phxHand.handlingNodes.keyValueSplitter }, StringSplitOptions.None);
                if (tempNodeArr.Length > 1)
                {
                    #region Labels and Grid display
                    string key = tempNodeArr[0];
                    string value = tempNodeArr[1];
                    Label keyLbl = new Label();
                    nodeGridUIElements.Add(keyLbl);
                    keyLbl.Name = $"Keylbl{irow}";
                    keyLbl.Content = key;
                    keyLbl.FontSize = 17;
                    keyLbl.FontWeight = FontWeights.Bold;
                    keyLbl.Foreground = new SolidColorBrush(Colors.Green);
                    keyLbl.VerticalAlignment = VerticalAlignment.Top;

                    TextBox valueBox = new TextBox();
                    nodeGridUIElements.Add(valueBox);
                    valueBox.Name = $"Valuebox{irow}";
                    valueBox.Text = value;
                    valueBox.FontSize = 17;
                    valueBox.FontWeight = FontWeights.Bold;
                    valueBox.Foreground = new SolidColorBrush(Colors.Green);
                    valueBox.VerticalAlignment = VerticalAlignment.Top;

                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength();
                    nodesGrid.RowDefinitions.Add(rowDefinition);
                    nodesGrid.Children.Add(keyLbl);
                    Grid.SetRow(keyLbl, irow);
                    Grid.SetColumn(keyLbl, 0);
                    nodesGrid.Children.Add(valueBox);
                    Grid.SetRow(valueBox, irow);
                    Grid.SetColumn(valueBox, 1);
                    #endregion
                }
                irow += 1;
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
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStacktrace: {e.StackTrace}");
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
                if (!string.IsNullOrWhiteSpace(curFileName)) StartHandlingOpen();
            }
        }
    }

    public class PhxNodes
    {
        private string _nodeSplitter = ";&;";
        public string nodeSplitter
        {
            get { return _nodeSplitter; }
            set { _nodeSplitter = nodeSplitter; }
        }
        private string _keyValueSplitter = ";,;";
        public string keyValueSplitter
        {
            get { return _keyValueSplitter; }
            set { _keyValueSplitter = keyValueSplitter; }
        }

        string Raw = string.Empty;
        public IEnumerable GetNodes()
        {
            return Raw.Split(new string[] { _nodeSplitter }, StringSplitOptions.None);
        }
        public void Add(string item)
        {
            Raw += (Raw.EndsWith(_keyValueSplitter)) ? $"{item}" : $"{_nodeSplitter}{item}{_keyValueSplitter}";
        }
    }

    public class PhxHandling
    {
        public PhxNodes handlingNodes = new PhxNodes();
    }
}
