using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA_V_Phoenix_Handling_Editor
{
    public class PhxNode
    {
        public string Key;
        public string Value;
        public List<string> Attributes = new List<string>();
    }

    public class PhxHandling
    {
        public List<PhxNode> NodesList = new List<PhxNode>();
        NodeType lastNodeType;
        PhxNode currentNode = null;
        public enum NodeType
        {
            Key,
            Value,
            Attribute
        }
        public void Add(string item, NodeType nodeType)
        {
            if (!lastNodeType.Equals(nodeType) || !string.IsNullOrWhiteSpace(item))
            {
                lastNodeType = nodeType;
                switch (nodeType)
                {
                    case NodeType.Key:
                        if (currentNode != null)
                            NodesList.Add(currentNode);
                        currentNode = new PhxNode();
                        currentNode.Key = item;
                        break;
                    case NodeType.Value:
                        currentNode.Value = item;
                        break;
                    case NodeType.Attribute:
                        if (currentNode != null)
                        {
                            currentNode.Attributes.Add(item);
                        }
                        break;
                }
            }
        }
    }
}
