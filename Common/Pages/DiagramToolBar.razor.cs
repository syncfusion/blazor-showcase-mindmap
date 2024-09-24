using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Blazor.Diagram;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MindMap
{
#pragma warning disable BL0005
    public partial class DiagramToolBar
    {
        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for interop between C# and JavaScript in Blazor.
        /// </summary>
        /// <remarks>
        /// The JavaScript runtime is responsible for executing JavaScript code from C# and handling JavaScript interop calls.
        /// In a Blazor application, this property should be set to an instance of the IJSRuntime interface provided by the framework.
        /// </remarks>
        [Inject]
        protected IJSRuntime? jsRuntime { get; set; } = null!;
        #region events
        /// <summary>
        /// This is used to update the zoom in/ zoom out the diagram
        /// </summary>
        private void DrawZoomChange(Syncfusion.Blazor.SplitButtons.MenuEventArgs args)
        {
            var diagram = Parent.DiagramContent.Diagram;
            double currentZoom = Parent.DiagramContent.CurrentZoom;
            switch (args.Item.Text)
            {
                case "Zoom In":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { Type = "ZoomIn", ZoomFactor = 0.2 });
                    break;
                case "Zoom Out":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { Type = "ZoomOut", ZoomFactor = 0.2 });
                    break;
                case "Zoom to Fit":
                    FitOptions fitoption = new FitOptions()
                    {
                        Mode = FitMode.Both,
                        Region = DiagramRegion.Content,
                    };
                    diagram.FitToPage(fitoption);
                    break;
                case "Zoom to 50%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((0.5 / currentZoom) - 1) });
                    break;
                case "Zoom to 100%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((1 / currentZoom) - 1) });
                    break;
                case "Zoom to 200%":
                    Parent.DiagramContent.ZoomTo(new DiagramMainContent.ZoomOptions() { ZoomFactor = ((2 / currentZoom) - 1) });
                    break;
            }
            ZoomItemDropdownContent = FormattableString.Invariant($"{Math.Round(Parent.DiagramContent.CurrentZoom * 100)}") + "%";
        }
        /// <summary>
        /// This is used to update the toolbar functionality
        /// </summary>
        private async Task ToolbarEditorClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            var diagram = Parent.DiagramContent.Diagram;
            var commandType = args.Item.TooltipText.ToLower(new CultureInfo("en-US"));
            switch (commandType)
            {
                case "undo":
                    diagram.Undo();
                    EnableToolbarItems(new object() { }, "historychange");
                    break;
                case "redo":
                    diagram.Redo();
                    EnableToolbarItems(new object() { }, "historychange");
                    break;
                case "pan tool":
                    Parent.DiagramContent.UpdateTool();
                    diagram.ClearSelection();
                   
                    break;
                case "pointer":
                    Parent.DiagramContent.UpdatePointerTool();
                    break;
                case "delete":
                    DeleteData();
                    toolbarClassName = "db-toolbar-container db-undo";
                    break;
                case "add child":
                    Parent.MindMapPropertyPanel.AddNode("Left");
                    break;
                case "add sibling":
                    Parent.MindMapPropertyPanel.AddSiblingChild("Bottom");
                    break;
                case "add multiple child":
                   
                    Parent.MindMapPropertyPanel.UpdatePropertyPanel();
                    break;
            }
            if (commandType == "pan tool" || commandType == "pointer" || commandType == "text tool")
            {
#pragma warning disable CA1307 // Specify StringComparison
                if (args.Item.CssClass.IndexOf("tb-item-selected") == -1)
#pragma warning restore CA1307 // Specify StringComparison
                {
                    if (commandType == "pan tool")
                        PanItemCssClass += " tb-item-selected";
                    if (commandType == "pointer")
                        PointerItemCssClass += " tb-item-selected";
                    await removeSelectedToolbarItem(commandType).ConfigureAwait(true);
                }
            }
         
            Parent.DiagramContent.StateChanged();
        }
        /// <summary>
        /// This is used to delete the selected items.
        /// </summary>
        public void DeleteData()
        {
            bool GroupAction = false;
            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            if ((diagram.SelectionSettings.Nodes.Count > 1 || diagram.SelectionSettings.Connectors.Count > 1 || ((diagram.SelectionSettings.Nodes.Count + diagram.SelectionSettings.Connectors.Count) > 1)))
            {
                GroupAction = true;
            }
            if (GroupAction)
            {
                diagram.StartGroupAction();
            }
            if (diagram.SelectionSettings.Nodes.Count != 0)
            {
                for (var i = diagram.SelectionSettings.Nodes.Count - 1; i >= 0; i--)
                {
                    var item = diagram.SelectionSettings.Nodes[i];

                    diagram.Nodes.Remove(item);
                }
            }
            if (diagram.SelectionSettings.Connectors.Count != 0)
            {
                for (var i = diagram.SelectionSettings.Connectors.Count - 1; i >= 0; i--)
                {
                    var item1 = diagram.SelectionSettings.Connectors[i];

                    diagram.Connectors.Remove(item1);
                }
            }
            if (GroupAction)
                diagram.EndGroupAction();
        }
        /// <summary>
        /// This is used to remove the selected toolbar item.
        /// </summary>
        private async Task removeSelectedToolbarItem(string tool)
        {
#pragma warning disable CA1307 // Specify StringComparison

           
            if (tool != "pan tool" && PanItemCssClass.IndexOf("tb-item-selected") != -1)
            {
                PanItemCssClass = PanItemCssClass.Replace(" tb-item-selected", "");
            }
            if (tool != "pointer" && PointerItemCssClass.IndexOf("tb-item-selected") != -1)
            {
                PointerItemCssClass = PointerItemCssClass.Replace(" tb-item-selected", "");
            }
           
            StateHasChanged();
#pragma warning restore CA1307 // Specify StringComparison
        }
        #endregion
        /// <summary>
        /// This is used to remove the selected toolbar items.
        /// </summary>
        
        public void SingleSelectionToolbarItems()
        {
            bool diagram = Parent.DiagramContent.diagramSelected;
            StateHasChanged();
        }
        public void MutipleSelectionToolbarItems()
        {
            bool diagram = Parent.DiagramContent.diagramSelected;
            SingleSelectionToolbarItems();

            StateHasChanged();
        }
        public void DiagramSelectionToolbarItems()
        {
            SingleSelectionToolbarItems();
        }
        /// <summary>
        /// This is used to update the zoom value.
        /// </summary>
        public void DiagramZoomValueChange()
        {
            ZoomItemDropdownContent = FormattableString.Invariant($"{Math.Round(Parent.DiagramContent.CurrentZoom * 100)}") + "%";
            StateHasChanged();
        }

        #region public methods
        /// <summary>
        /// This is used to enable the toolbar items.
        /// </summary>
        public void EnableToolbarItems<T>(T obj, string eventname)
        {

            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            ObservableCollection<NodeBase> collection = new ObservableCollection<NodeBase>();
            if (eventname == "selectionchange")
            {
                ObservableCollection<IDiagramObject>? list = obj as ObservableCollection<IDiagramObject>;
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list?.Count; i++)
                    {
                        NodeBase node = (NodeBase)list[i];
                        collection.Add(node);
                    }
                }
                UtilityMethods_enableToolbarItems(collection);
            }

            if (eventname == "historychange")
            {
                RemoveUndo();
                RemoveRedo();
                if (diagram.HistoryManager.CanUndo)
                {
                    this.Parent.DiagramContent.IsUndo = diagram.HistoryManager.CanUndo;
                    this.Parent.DiagramContent.IsRedo = diagram.HistoryManager.CanRedo;
                    toolbarClassName += " db-undo";
                }
                if (diagram.HistoryManager.CanRedo)
                {
                    this.Parent.DiagramContent.IsRedo = diagram.HistoryManager.CanRedo;
                    this.Parent.DiagramContent.IsUndo = diagram.HistoryManager.CanUndo;
                    toolbarClassName += " db-redo";
                }
                StateHasChanged();
            }
        }
        /// <summary>
        /// This is used to remove the undo selection.
        /// </summary>
        public void RemoveUndo()
        {
            string undo = "undo";
            if (toolbarClassName.Contains(undo))
            {
                int first = toolbarClassName.IndexOf(undo);
                toolbarClassName = toolbarClassName.Remove(first - 4, 8);
            }
        }
        /// <summary>
        /// This is used to remove the redo selection.
        /// </summary>
        public void RemoveRedo()
        {
            string redo = "redo";
            if (toolbarClassName.Contains(redo))
            {
                int first = toolbarClassName.IndexOf(redo);
                toolbarClassName = toolbarClassName.Remove(first - 4, 8);
            }
        }
        /// <summary>
        /// This is used to enable the toolbar items.
        /// </summary>
        public void UtilityMethods_enableToolbarItems(ObservableCollection<NodeBase> SelectedObjects)
        {
            SfDiagramComponent diagram = Parent.DiagramContent.Diagram;
            removeClassElement();
            if (this.Parent.DiagramContent.IsUndo)
            {
                toolbarClassName += " db-undo";
            }
            if (this.Parent.DiagramContent.IsRedo)
            {
                toolbarClassName += " db-redo";
            }
            else if (SelectedObjects.Count > 0 && Parent.MindMapPropertyPanel.IsMindMap)
            {
                toolbarClassName = toolbarClassName + " db-child-sibling";
                addSiblingCssName = SelectedObjects[0].ID == "rootNode" ? "tb-item-start tb-item-sibling" : "tb-item-start tb-item-child";
            }
            if (SelectedObjects.Count > 1)
                StateHasChanged();
        }
        /// <summary>
        /// This is used to hide the property container.
        /// </summary>
        public async Task HidePropertyContainer()
        {
            int index = Parent.MenuBar.WindowMenuItems.FindIndex(item => item.Text == "Show Properties");
            Parent.MenuBar.WindowMenuItems[index].IconCss = Parent.MenuBar.WindowMenuItems[index].IconCss == "sf-icon-Selection" ? "sf-icon-Remove" : "sf-icon-Selection";
            //HideButtonBackground = (Parent.MenuBar.WindowMenuItems[index].IconCss == "sf-icon-Selection") ? "#0078d4" : "rgb(227, 227, 227)";
            //HideButtonCss = (Parent.MenuBar.WindowMenuItems[index].IconCss == "sf-icon-Selection") ? "db-toolbar-hide-btn tb-property-open" : "db-toolbar-hide-btn tb-property-close";
            await this.HideElements("hide-properties",Parent.MenuBar.IsNewClick);
            if (Parent.MindMapPropertyPanel.IsMindMap)
            {
                object bounds;
                await Task.Delay(1000);
                if (jsRuntime != null)
                {
                    bounds = await jsRuntime.InvokeAsync<object>("getViewportBounds").ConfigureAwait(true);
                    if (bounds != null)
                    {
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        };
                        DiagramSize dataObj = System.Text.Json.JsonSerializer.Deserialize<DiagramSize>(bounds.ToString(), options);
                        if (dataObj != null)
                        {
                            if (Parent.DiagramContent != null)
                            {
                                Parent.DiagramContent.Diagram.BeginUpdate();
                                Parent.DiagramContent.Diagram.Width = dataObj.Width + "px";
                                Parent.DiagramContent.Diagram.Height = dataObj.Height + "px";
                                await Parent.DiagramContent.Diagram.EndUpdateAsync();
                            }
                        }

                    }
                }
                
            }
            Parent.MenuBar.StateChanged();
            Parent.DiagramContent.StateChanged();
            Parent.MindMapPropertyPanel.StateHasChange();
        }
        /// <summary>
        /// This is used to remove the toolbar items class name.
        /// </summary>
        public void removeClassElement()
        {
            string space = " ";
            if (toolbarClassName.Contains(space))
            {
                int first = toolbarClassName.IndexOf(space);
                if (first != 0)
                {
                    toolbarClassName = toolbarClassName.Remove(20);
                }
            }
           
        }
        /// <summary>
        /// This is used to remove the toolbar items.
        /// </summary>
        private async Task HideToolBar()
        {
#pragma warning disable CA1307 // Specify StringComparison
            if (MenuHideIconCss.Contains("sf-icon-Collapse"))
#pragma warning restore CA1307 // Specify StringComparison
            {
                MenuHideIconCss = "sf-icon-DownArrow tb-icons";
            }
            else
            {
                MenuHideIconCss = "sf-icon-Collapse tb-icons";
            }
            if(jsRuntime!=null)
               await jsRuntime.InvokeAsync<object>("hideMenubar").ConfigureAwait(true);
        }
        /// <summary>
        /// This is used to remove the toolbar items.
        /// </summary>
        public async Task HideElements(string eventname, bool isNewClick = false)
        {
            if(jsRuntime!=null)
               await jsRuntime.InvokeAsync<object>("UtilityMethods_hideElements", eventname, isNewClick).ConfigureAwait(true);
        }
        #endregion
    }
#pragma warning restore BL0005
}
