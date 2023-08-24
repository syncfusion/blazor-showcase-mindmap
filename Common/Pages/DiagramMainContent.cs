using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Blazor.Diagram;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;


namespace MindMap
{
    public partial class DiagramMainContent
    {
        /// <summary>
        /// Initializes the diagram model.
        /// </summary>
        public void InitDiagramModel()
        {
            Dictionary<string, object> nodeInfo = new Dictionary<string, object>();
            nodeInfo.Add("Level", "0");
            nodeInfo.Add("Orientation", "Root");
            Node node = new Node()
            {
                ID = "rootNode",
                //Size of the node.
                Height =50,
                Width = 150,
                //Position of the node.
                OffsetX = 505,
                OffsetY = 248.66667175292969,
                Annotations = new DiagramObjectCollection<ShapeAnnotation>()
                {
                    new ShapeAnnotation
                    {
                        Content = "Creativity", 
                    }
                },
                AdditionalInfo= nodeInfo,
                //Sets the type of the shape as basic.
                Shape = new BasicShape()
                {
                    Type = NodeShapes.Basic,
                    Shape = NodeBasicShapes.Ellipse,
                    //Sets the corner radius to the node shape.
                    CornerRadius = 10
                },
                Style = new ShapeStyle() { Fill = "#D0ECFF", StrokeColor="#80BFEA" },
               
            };
            nodes.Add(node);
            UserHandle cloneHandle = new UserHandle()
            {
                Name = "AddRight",
                PathData = "M0,4.55 L8.41,4.55 L8.41,0 L13.75,6.88 L8.41,13.75 L8.41,8.48 L0,8.48 L0,4.55 Z ",
                Visible = true,
                Side = Direction.Right,
                Offset=0.5,
                Displacement=10,
                Margin = new DiagramThickness { Top = 0, Bottom = 0, Left = 0, Right = 0 },
                Size = 25,
                PathColor = "white",
                BackgroundColor = "black",
                BorderWidth = 3,
            };
            UserHandle cloneHandle2 = new UserHandle()
            {
                Name = "AddLeft",
                PathData = "M13.75,9.2 L5.34,9.2 L5.34,13.75 L0,6.88 L5.34,0 L5.34,5.27 L13.75,5.27 L13.75,9.2 Z ",
                Visible = true,
                Side = Direction.Left,
                Offset = 0.5,
                Displacement = 10,
                Margin = new DiagramThickness { Top = 0, Bottom = 0, Left = 0, Right = 0 },
                Size = 25,
                PathColor = "white",
                BackgroundColor = "black",
                BorderWidth = 3,
            };
            UserHandle DeleteHandle = new UserHandle()
            {
                Name = "DelteRight",
                PathData = "M1.0000023,3 L7.0000024,3 7.0000024,8.75 C7.0000024,9.4399996 6.4400025,10 5.7500024,10 L2.2500024,10 C1.5600024,10 1.0000023,9.4399996 1.0000023,8.75 z M2.0699998,0 L5.9300004,0 6.3420029,0.99999994 8.0000001,0.99999994 8.0000001,2 0,2 0,0.99999994 1.6580048,0.99999994 z",
                Visible = false,
                Side = Direction.Right,
                Offset = 0.5,
                Displacement = 10,
                Margin = new DiagramThickness { Top = 0, Bottom = 0, Left = 0, Right = 0 },
                Size = 25,
                PathColor = "white",
                BackgroundColor = "black",
                BorderWidth = 3,
            };
            UserHandle DeleteHandle2 = new UserHandle()
            {
                Name = "DeleteLeft",
                PathData = "M1.0000023,3 L7.0000024,3 7.0000024,8.75 C7.0000024,9.4399996 6.4400025,10 5.7500024,10 L2.2500024,10 C1.5600024,10 1.0000023,9.4399996 1.0000023,8.75 z M2.0699998,0 L5.9300004,0 6.3420029,0.99999994 8.0000001,0.99999994 8.0000001,2 0,2 0,0.99999994 1.6580048,0.99999994 z",
                Visible = false,
                Side = Direction.Left,
                Offset = 0.5,
                Displacement = 10,
                Margin = new DiagramThickness { Top = 0, Bottom = 0, Left = 0, Right = 0 },
                Size = 25,
                PathColor = "white",
                BackgroundColor = "black",
                BorderWidth = 3,
            };
            UserHandles = new DiagramObjectCollection<UserHandle>()
            {
               DeleteHandle,DeleteHandle2, cloneHandle,cloneHandle2,
            };
            SelectedModel = new DiagramSelectionSettings()
            {
                //Enable userhandle for selected model.
                Constraints = SelectorConstraints.All & ~(SelectorConstraints.Rotate | SelectorConstraints.ResizeAll),
                UserHandles = this.UserHandles
            };
        }
    }
    /// <summary>
    /// Represents an item in a TreeView component
    /// </summary>
    public class TreeItem
    {
        public string? Id { get; set; }
        public string? ParentId { get; set; }
        public string? Name { get; set; }
        public bool Expanded { get; set; }
        public bool HasChild { get; set; }
        
    }
   
}
