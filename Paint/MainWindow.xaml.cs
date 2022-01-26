using PaintContract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public Dictionary<string, ILayer> Layers { get; set; }
        public ObservableCollection<KeyValuePair<string, ILayer>> Layers { get; set; }
        public KeyValuePair<string, ILayer>  selectedLayer { get; set; }
        public KeyValuePair<string, ILayer> editLayer { get; set; }
        public Dictionary<string, ILayer> PenLines { get; set; }
        ILayer _preview;
        private bool _isDrawing = false;
        private bool _penMode = false;
        private bool _isSelecting = false;
        private Point _startPosition;
        private bool _isMoving = false;
        private bool _isResizing = false;
        private CursorState _direction = CursorState.Out;
        public MainWindow()
        {
            InitializeComponent();
            Layers = new ObservableCollection<KeyValuePair<string, ILayer>>();
            PenLines = new Dictionary<string, ILayer>();
            ListBoxLayer.ItemsSource = Layers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadExternalDllAndCreateCbbItems();
        }

        public void LoadExternalDllAndCreateCbbItems ()
        {
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");
            foreach(var dll in dlls)
            {
                var assembly = Assembly.LoadFrom(dll.FullName);
                var types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.IsClass)
                    {
                        
                        if(typeof(IShape).IsAssignableFrom(type))
                        {
                            IShape shape = (IShape)Activator.CreateInstance(type);
                            ShapeBuilder.GetInstance()._prototypes.Add(shape.Name, shape);
                        } 
                    }
                }
            }

            foreach(ILayer layer in ShapeBuilder.GetInstance()._prototypes.Values)
            {
                IShape s = layer as IShape;
                if(s != null)
                {
                    Cbb_Shape.Items.Add(s.Name);
                }
            }
            Cbb_Shape.SelectedIndex = 0;
            _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
            
        }

        private void TwitterButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

        }


        private void Color_Table_Selected(object sender, RoutedEventArgs e)
        {
            var dlg = new ColorDialog(); //Khởi tạo đối tượng ColorDialog 

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Nếu nhấp vào nút OK trên hộp thoại
            {
                var color = System.Windows.Media.Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                color_react.Background = new SolidColorBrush(color);

                ShapeBuilder.GetInstance().BuildColor(color);
                _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
            }
        }

        private void Color_Table_Fill_Selected(object sender, RoutedEventArgs e)
        {
            var dlg = new ColorDialog(); //Khởi tạo đối tượng ColorDialog 

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) //Nếu nhấp vào nút OK trên hộp thoại
            {
                var color = System.Windows.Media.Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                color_fill_react.Background = new SolidColorBrush(color);

                ShapeBuilder.GetInstance().BuildColorFill(color);
                _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Brush_Button_Click(object sender, RoutedEventArgs e)
        {
            _penMode = !_penMode;
            if (_penMode)
            {
                var colorBlack = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                brushButton.Foreground = colorBlack;
            }
            else
            {
                var colorGray = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                brushButton.Foreground = colorGray;
            }
        }

        //private void Shape_Selected(object sender, RoutedEventArgs e)
        //{
        //    var colorGray = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 128, 128));
        //    if (brushButton != null)
        //        brushButton.Foreground = colorGray;
        //}

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isSelecting) return;
            _isDrawing = true;
            Point pos = e.GetPosition(canvas);
            if (_penMode)
            {
                _preview = ShapeBuilder.GetInstance().BuildShape("Line");
            }
            _preview.HandleStart(pos.X, pos.Y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(_isResizing)
            {
                _isResizing = false;
                return;
            }
            if(_isSelecting)
            {
                if(selectedLayer.Value != null)
                {
                    _isMoving = false;
                    selectedLayer.Value.handleMoveDone();
                    ReDraw();
                }
            }
            if (!_isDrawing) return;
           
            _isDrawing = false;

            // Thêm đối tượng cuối cùng vào mảng quản lí
            Point pos = e.GetPosition(canvas);
            _preview.HandleEnd(pos.X, pos.Y);

            // generate name

            //Layers.Add(_preview.GetUniqueName(), _preview);
            
            if (!_penMode) Layers.Insert(0, new KeyValuePair<string, ILayer>(_preview.GetUniqueName(), _preview));
            else PenLines.Add(_preview.GetUniqueName(), _preview);
            // Sinh ra đối tượng mẫu kế
            if(_preview as IShape != null)
            {
                _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
            } else if(_preview as ILayer != null)
            {
                _preview = new Text2D();
            }

            ReDraw();
        }

        private void ReDraw()
        {
            // Ve lai Xoa toan bo
            canvas.Children.Clear();
            // Ve lai tat ca cac hinh
            for(int i = Layers.Count-1; i >= 0; i--)
            {
                var shape = Layers[i].Value;
                var element = shape.Draw();
                if (element == null) continue;
                canvas.Children.Add(element);
            }
            foreach (var penLine in PenLines)
            {
                var shape = penLine.Value;
                var element = shape.Draw();
                if (element == null) continue;
                canvas.Children.Add(element);
            }
            DrawRectangleBorder();
        }

        private void DrawRectangleBorder()
        {
            if((selectedLayer.Value as IEditableLayer) !=null)
            {
                canvas.Children.Add(((IEditableLayer)selectedLayer.Value).DrawBorder());
            }
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(_isDrawing)
            {
                Point pos = e.GetPosition(canvas);
                _preview.HandleEnd(pos.X, pos.Y);
                if (_penMode)
                {
                    PenLines.Add(_preview.GetUniqueName(), _preview);
                    _preview = ShapeBuilder.GetInstance().BuildShape("Line");
                    _preview.HandleStart(pos.X, pos.Y);
                }
                ReDraw();

                if (!_penMode) canvas.Children.Add(_preview.Draw());
                Title = $"{pos.X} {pos.Y}";
            }
            if(_isSelecting)
            {
                // handle move object
                Point position = e.GetPosition(canvas);
                double deltaY = position.Y - _startPosition.Y;
                double deltaX = position.X - _startPosition.X;
                if (selectedLayer.Value != null && _isMoving)
                {
                    
                    selectedLayer.Value.handleMove(new Point2D(deltaX, deltaY));
                    ReDraw();
                }

                var currentDirection = CursorState.Out;
                // change cursor type
                foreach (var x in Layers)
                {
                    if (x.Value.checkPosition(new Point2D(position.X, position.Y)) != CursorState.Out)
                    {
                        currentDirection = x.Value.checkPosition(new Point2D(position.X, position.Y));
                        break;
                    }
                }

                double delta = 0;

                switch (currentDirection)
                {
                    case CursorState.In:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeAll;
                        break;
                    case CursorState.Left:
                    case CursorState.Right:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeWE;
                        break;
                    case CursorState.Top:
                    case CursorState.Bottom:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeNS;
                        break;
                    case CursorState.CornerTopLeft:
                    case CursorState.CornerBottomRight:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeNWSE;
                        break;
                    case CursorState.CornerBottomLeft:
                    case CursorState.CornerTopRight:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeNESW;
                        break;
                    case CursorState.Out:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                        break;
                    case CursorState.Start:
                    case CursorState.End:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.SizeWE;
                        break;
                    default:
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                        break;
                }

                if (_isResizing)
                {
                    selectedLayer.Value.handleResize(_direction, deltaX, deltaY);
                    _startPosition = position;
                    ReDraw();
                }
            }
        }

        private void Shape_Selected(object sender, SelectionChangedEventArgs e)
        {
            //_penMode = false;
            //var colorGray = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            //brushButton.Foreground = colorGray;
            _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
        }

        private void mySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = (double)e.NewValue;

            ShapeBuilder.GetInstance().BuildThickness(value);
            if (Cbb_Shape.SelectedItem == null) return;
            _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)CbbStroke.SelectedItem;
            string value = typeItem.Name;
            StrokeType type = StrokeType.SOLID;

            switch(value)
            {
                case "SOLID":
                    type = StrokeType.SOLID;
                    break;
                case "DASHDOT":
                    type = StrokeType.DASHDOT;
                    break;
                default:
                    break;
            }
            ShapeBuilder.GetInstance().BuildStroke(type);
            if (Cbb_Shape.SelectedItem == null) return;
            _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());
        }

        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json|All files (*.*)|(*.*)";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;
                var newDto = new LayerSaveDto()
                {
                    Layers = this.Layers,
                    PenLines = this.PenLines,
                };
                IOManager.GetInstance().SaveToBinaryFile(newDto, filename);
            }
        }

        private void LoadMenu_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file (*.json)|*.json|All files (*.*)|(*.*)";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                var newDto = IOManager.GetInstance().LoadFromBinaryFile(filename);
                Layers = newDto.Layers ?? Layers;
                PenLines = newDto.PenLines ?? PenLines;
                ListBoxLayer.ItemsSource = Layers;
                ReDraw();
            }
        }

        private void ListBoxLayer_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if(ListBoxLayer.Items.Count > 0)
            {
                ListBoxLayer.ScrollIntoView(ListBoxLayer.Items[0]);
            }
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog imgDialog = new OpenFileDialog();
            if(imgDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = imgDialog.FileName;
                var image = new Image2D(fileName);
                Layers.Insert(0, new KeyValuePair<string, ILayer>(image.GetUniqueName(), image));
                ReDraw();
            }
            
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var matTrans = canvas.RenderTransform as MatrixTransform;
            var pos1 = e.GetPosition(grid);
            
            if (e.Delta > 0 && matTrans.Value.M11 > 4.0) return;
            if (e.Delta < 0 && matTrans.Value.M11 < 0.1) return;
            var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
            var mat = matTrans.Matrix;
            mat.ScaleAt(scale, scale, pos1.X, pos1.Y);
            
            matTrans.Matrix = mat;
            e.Handled = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExportMenu_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG image (*.png)|*.png|All files (*.*)|(*.*)";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string err = IOManager.GetInstance().ExportToPNG(canvas, saveFileDialog.FileName);
                if (err != "")
                {
                    System.Windows.MessageBox.Show(err, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            _isSelecting = !_isSelecting;
            if (_isSelecting)
            {
                var colorBlack = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                selectButton.Foreground = colorBlack;
            }
            else
            {
                var colorGray = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                selectButton.Foreground = colorGray;
                selectedLayer = new KeyValuePair<string, ILayer>();
                ReDraw();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(_isSelecting)
            {
                Point pos = e.GetPosition(canvas);
                _startPosition = pos;
                
                bool isSelectNone = true;
                foreach (var x in Layers)
                {
                    var Currentdirection = x.Value.checkPosition(new Point2D(pos.X, pos.Y));
                    if(Currentdirection != CursorState.Out)
                    {
                        switch (Currentdirection)
                        {
                            case CursorState.In:
                                selectedLayer = x;
                                _isMoving = true;
                                isSelectNone = false;
                                break;
                            default:
                                selectedLayer = x;
                                _isResizing = true;
                                isSelectNone = false;
                                _direction = Currentdirection;
                                break;
                        }
                        break;
                    }
               
                }
                if(isSelectNone)
                {
                    selectedLayer = new KeyValuePair<string, ILayer>();
                }
                ReDraw();
            }
        }

        private void addText_Click(object sender, RoutedEventArgs e)
        {
            _preview = new Text2D();
        }

        private void cut_Click(object sender, RoutedEventArgs e)
        {
           
            if (selectedLayer.Value != null)
            {
                editLayer = selectedLayer;
            }
            Layers.Remove(selectedLayer);
            selectedLayer = new KeyValuePair<string, ILayer>();
            ReDraw();

        }

        private void paste_Click(object sender, RoutedEventArgs e)
        {
            Layers.Insert(0, editLayer);
            //selectedLayer = editLayer;
            //editLayer = new KeyValuePair<string, ILayer>();
            ReDraw();
        }

        private void copy_Click(object sender, RoutedEventArgs e)
        {
            if(selectedLayer.Value != null)
            {
                var newObject = (ILayer)selectedLayer.Value.Clone();
                editLayer = new KeyValuePair<string, ILayer>(
                    newObject.GetUniqueName(),
                    newObject
                    );
            }
        }
    }
}
