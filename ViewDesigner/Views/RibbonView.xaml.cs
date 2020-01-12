using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using ViewDesigner.Common;
using ViewDesigner.Models;

namespace ViewDesigner.Views
{
    /// <summary>
    /// RibbonView.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonView : UserControl
    {
        /// <summary>
        /// 静态初始化
        /// </summary>
        static RibbonView()
        {
            var openBinding = new CommandBinding(DiagramCommands.Open, ExecuteOpen, CanExecuteOpen);
            var saveBinding = new CommandBinding(DiagramCommands.Save, ExecuteSave, CanExecuteSave);
            var loadBinding = new CommandBinding(DiagramCommands.Load, ExecuteLoad, CanExecuteLoad);

            CommandManager.RegisterClassCommandBinding(typeof(RibbonView), openBinding);
            CommandManager.RegisterClassCommandBinding(typeof(RibbonView), saveBinding);
            CommandManager.RegisterClassCommandBinding(typeof(RibbonView), loadBinding);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public RibbonView()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                var nail = navigationPane.FindChildByType<RadDiagramThumbnail>();
                nail.Background = diagram.Background;
            };
        }
        /// <summary>
        /// 打开命令状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CanExecuteOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;
            e.CanExecute = owner != null && owner.diagram != null;
        }
        /// <summary>
        /// 加载SVG命令状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CanExecuteLoad(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;
            e.CanExecute = owner != null && owner.diagram != null;
        }
        /// <summary>
        /// 保存命令状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;
            e.CanExecute = owner != null && owner.diagram != null && owner.diagram.Items.Count > 0;
        }
        /// <summary>
        /// 执行打开命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;

            if (owner != null)
            {
                //取消当前缩略图关联(防止加载图元一直刷新缩略图)
                owner.navigationPane.Diagram = null;

                owner.diagram.Clear();
                using (Stream fStream = new FileStream(@"test.dat", FileMode.Open, FileAccess.ReadWrite))
                {
                    var binFormat = new BinaryFormatter();//创建二进制序列化器
                    var list = (List<FigureBase>)binFormat.Deserialize(fStream);//反序列化对象
                    list.ForEach(owner.AddFigureToDiagram);
                }

                //将缩略图重置关联
                owner.navigationPane.Diagram = owner.diagram;
            }
        }
        /// <summary>
        /// 执行加载命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ExecuteLoad(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;

            if (owner != null)
            {
                //取消当前缩略图关联(防止加载图元一直刷新缩略图)
                owner.navigationPane.Diagram = null;

                #region 加载SVG图
                owner.diagram.Clear();
                var fileName = string.Empty;
#if DEBUG
                fileName = @"Config\TideView.svg";
#else
                var ofd = new OpenFileDialog { Filter = @"SVG 图形 (*.svg)|*.svg|所有文件 (*.*)|*.*" };
                if (ofd.ShowDialog() == true)
                {
                    fileName = ofd.FileName;
                }
#endif
                if (!string.IsNullOrEmpty(fileName))
                {
                    var figures = SvgReader.LoadSvgFile(fileName);
                    foreach (var figure in figures)
                    {
                        owner.AddFigureToDiagram(figure);
                    }
                }
                #endregion

                //将缩略图重置关联
                owner.navigationPane.Diagram = owner.diagram;
            }
        }
        /// <summary>
        /// 执行保存命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            var owner = sender as RibbonView;

            if (owner != null)
            {
                var figureShapes = owner.diagram.Items.OfType<FigureShape>();
                var serialList = figureShapes.Select(figureShape => figureShape.Content as FigureBase).ToList();

                using (Stream fStream = new FileStream(@"test.dat", FileMode.Create, FileAccess.ReadWrite))
                {
                    var binFormat = new BinaryFormatter();//创建二进制序列化器
                    binFormat.Serialize(fStream, serialList);
                }
            }
        }
        /// <summary>
        /// 增加图元到绘图面板
        /// </summary>
        /// <param name="figure"></param>
        private void AddFigureToDiagram(FigureBase figure)
        {
            var shape = new FigureShape() { DataContext = figure };
            diagram.AddShape(shape);
        }
    }
}
