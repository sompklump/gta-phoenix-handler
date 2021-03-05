using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GTA_V_Phoenix_Handling_Editor
{
    public class UIFuncs
    {
        public void DisplayNodes(PhxHandling phxHand)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            CustomEvents customEvents = new CustomEvents();
            List<UIElement> nodeGridUIElements = mainWindow.nodeGridUIElements;
            Grid nodesGrid = mainWindow.nodesGrid;
            int irow = 1;
            if (nodeGridUIElements.Count > 0)
            {
                foreach (UIElement element in nodeGridUIElements)
                {
                    nodesGrid.Children.Remove(element);
                }
            }
            nodeGridUIElements.Clear();
            nodesGrid.RowDefinitions.Clear();
            Grid.SetRow(new Label(), 0);
            foreach (PhxNode phxNode in phxHand.NodesList)
            {
                #region Labels and Grid display
                #region Key box
                Label keyLbl = new Label();
                keyLbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                keyLbl.VerticalAlignment = VerticalAlignment.Stretch;
                nodeGridUIElements.Add(keyLbl);
                keyLbl.Name = $"Keylbl{irow}";
                keyLbl.Content = phxNode.Key;
                keyLbl.FontSize = 17;
                keyLbl.FontWeight = FontWeights.Bold;
                keyLbl.Foreground = new SolidColorBrush(Colors.White);
                #endregion

                #region Value box
                TextBox valueBox = new TextBox();
                valueBox.TextChanged += new TextChangedEventHandler(customEvents.NodesTextChanged);
                valueBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                valueBox.VerticalAlignment = VerticalAlignment.Stretch;
                nodeGridUIElements.Add(valueBox);
                valueBox.Name = $"Valuebox{irow}";
                valueBox.Text = phxNode.Value;
                valueBox.FontSize = 17;
                valueBox.FontWeight = FontWeights.Bold;
                valueBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#405570"));
                valueBox.Foreground = new SolidColorBrush(Colors.White);
                #endregion

                #region Attributes box
                TextBox attributeBox = new TextBox();
                attributeBox.TextChanged += new TextChangedEventHandler(customEvents.NodesTextChanged);
                attributeBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                attributeBox.VerticalAlignment = VerticalAlignment.Stretch;
                nodeGridUIElements.Add(valueBox);
                attributeBox.Name = $"Attbox{irow}";
                foreach (string att in phxNode.Attributes)
                {
                    attributeBox.AppendText($"{att} ");
                }
                attributeBox.FontSize = 17;
                attributeBox.FontWeight = FontWeights.Bold;
                attributeBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#405570"));
                attributeBox.Foreground = new SolidColorBrush(Colors.White);
                #endregion

                #region Display all the nodes to the grid
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength();
                nodesGrid.RowDefinitions.Add(rowDefinition);
                nodesGrid.Children.Add(keyLbl);
                Grid.SetRow(keyLbl, irow);
                Grid.SetColumn(keyLbl, 0);
                nodesGrid.Children.Add(valueBox);
                Grid.SetRow(valueBox, irow);
                Grid.SetColumn(valueBox, 1);
                nodesGrid.Children.Add(attributeBox);
                Grid.SetRow(attributeBox, irow);
                Grid.SetColumn(attributeBox, 2);
                #endregion
                irow += 1;
                #endregion
            }
        }
    }
}
