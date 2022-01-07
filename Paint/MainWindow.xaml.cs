﻿using PaintContract;
using System;
using System.Collections.Generic;
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
        private Dictionary<string, ILayer> _layers = new Dictionary<string, ILayer>();
        IShape _preview;
        private bool _isDrawing = false;
        public MainWindow()
        {
            InitializeComponent();
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
                var assembly = Assembly.LoadFile(dll.FullName);
                var types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.IsClass)
                    {
                        if(typeof(ILayer).IsAssignableFrom(type))
                        {
                            ILayer layer = (ILayer)Activator.CreateInstance(type);
                            ShapeBuilder.GetInstance()._prototypes.Add(layer.Name, (IShape)layer);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Brush_Button_Click(object sender, RoutedEventArgs e)
        {
            var colorBlack = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            brushButton.Foreground = colorBlack;
        }

        private void Shape_Selected(object sender, RoutedEventArgs e)
        {
            var colorGray = new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 128, 128));
            if (brushButton != null)
                brushButton.Foreground = colorGray;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;
            Point pos = e.GetPosition(canvas);
            _preview.HandleStart(pos.X, pos.Y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            // Thêm đối tượng cuối cùng vào mảng quản lí
            Point pos = e.GetPosition(canvas);
            _preview.HandleEnd(pos.X, pos.Y);

            // generate name
            
            _layers.Add(_preview.GetUniqueName(), _preview);

            // Sinh ra đối tượng mẫu kế
            _preview = ShapeBuilder.GetInstance().BuildShape(Cbb_Shape.SelectedItem.ToString());

            ReDraw();
        }

        private void ReDraw()
        {
            // Ve lai Xoa toan bo
            canvas.Children.Clear();
            // Ve lai tat ca cac hinh
            foreach (var shape in _layers.Values)
            {
                var element = shape.Draw();
                if (element == null) continue;
                canvas.Children.Add(element);
            }
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(_isDrawing)
            {
                Point pos = e.GetPosition(canvas);
                _preview.HandleEnd(pos.X, pos.Y);
                canvas.Children.Clear();

                foreach(var shape in _layers.Values)
                {
                    UIElement element = shape.Draw();
                    canvas.Children.Add(element);
                }

                canvas.Children.Add(_preview.Draw());
                Title = $"{pos.X} {pos.Y}";
            }
        }

        private void Cbb_Shape_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Shape_Selected(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
