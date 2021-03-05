using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GTA_V_Phoenix_Handling_Editor
{
    public class ReadFile
    {
        ProgressWaiter progressWaiter = new ProgressWaiter();
        public async void StartHandlingOpen(string modelName, string fileName)
        {
            progressWaiter.Show();
            Task<PhxHandling> handXml = null;
            handXml = await Task.Factory.StartNew(() => ReadXML(fileName, modelName));
            handXml.Wait();
            UIFuncs uIFuncs = new UIFuncs();
            uIFuncs.DisplayNodes(handXml.Result);
        }
        private async Task<PhxHandling> ReadXML(string fileName, string modelName)
        {
            PhxHandling phxHandling = new PhxHandling();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            bool isAllFound = false;
            bool isStarted = false;
            string curNode = null;
            float currentLines = 0;

            Task<int> linesEstimated = await Task.Factory.StartNew(() => GetLines(modelName, fileName));
            linesEstimated.Wait();

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
                                // Get the element node key or attributes
                                case XmlNodeType.Element:
                                    if (isStarted)
                                    {
                                        currentLines += 1;
                                        try
                                        {
                                            await Task.Run(() => progressWaiter.Update((currentLines / linesEstimated.Result) * 100f));
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show(e.StackTrace);
                                        }
                                        phxHandling.Add(reader.Name, PhxHandling.NodeType.Key);
                                        for (int i = 0; i < reader.AttributeCount; i++)
                                        {
                                            //MessageBox.Show(reader.GetAttribute(i));
                                            phxHandling.Add(reader.GetAttribute(i), PhxHandling.NodeType.Attribute);
                                        }
                                    }
                                    curNode = reader.Name;
                                    break;
                                // Get the element node value or contents
                                case XmlNodeType.Text:
                                    //MessageBox.Show($"End element: {isAllFound} | Node: {await reader.GetValueAsync()}");
                                    if (curNode.ToLower() == "modelname" && isStarted == false)
                                    {
                                        isStarted = (reader.Value == modelName);
                                        break;
                                    }
                                    if (isStarted)
                                        phxHandling.Add(reader.Value, PhxHandling.NodeType.Value);
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
            catch (Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStack trace: {e.StackTrace}");
            }
            return phxHandling;
        }

        private async Task<int> GetLines(string modelName, string fileName)
        {
            int lines = 0;
            string curNode = null;
            bool isAllFound = false;
            bool isStarted = false;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

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
                                // Get the element node key or attributes
                                case XmlNodeType.Element:
                                    if (isStarted)
                                    {
                                        lines += 1;
                                    }
                                    curNode = reader.Name;
                                    break;
                                case XmlNodeType.Text:
                                    //MessageBox.Show($"End element: {isAllFound} | Node: {await reader.GetValueAsync()}");
                                    if (curNode.ToLower() == "modelname" && isStarted == false)
                                    {
                                        isStarted = (reader.Value == modelName);
                                        break;
                                    }
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
            catch (Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\r\n\r\nStack trace: {e.StackTrace}");
            }
            return lines;
        }
    }
}
