using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;
using Telerik.Windows.Controls;

namespace ViewDesigner.Models
{
    /// <summary>
    /// 图形基类
    /// </summary>
    [Serializable]
    public abstract class FigureBase : NotificationObject
    {
        /// <summary>
        /// 图形位置
        /// </summary>
        private Point position;
        public Point Position
        {
            get { return position; }
            set { position = value; RaisePropertyChanged("Position"); }
        }
    }
}
