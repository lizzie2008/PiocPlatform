using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ViewDesigner.Common;

namespace ViewDesigner.Models.SvgFigs
{
    [Serializable]
    public class LineFig : FigureBase
    {
        /// <summary>
        /// xml节点构造
        /// </summary>
        /// <param name="node"></param>
        public LineFig(XmlNode node)
        {
            var infoNode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(s => s.Name == "path");
            var pathAttri = infoNode.GetAttributeByName("d");

            var pathSplit = pathAttri.Split(new char[] { ' ' });
            var startX = double.Parse(pathSplit[1]);
            var startY = double.Parse(pathSplit[2]);
            for (int i = 0; i < pathSplit.Count() / 3; i++)
            {
                pathSplit[3 * i + 1] = (double.Parse(pathSplit[3 * i + 1]) - startX).ToString();
                pathSplit[3 * i + 2] = (double.Parse(pathSplit[3 * i + 2]) - startY).ToString();
            }

            this.Position = new Point(startX, startY);
            this.Path = string.Join(" ", pathSplit);
            this.LineType = infoNode.GetAttributeByName("class");
        }

        /// <summary>
        /// 绘图路径
        /// </summary>
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; RaisePropertyChanged("Path"); }
        }
        /// <summary>
        /// 线路类型(220kv,500kv)
        /// </summary>
        private string lineType;
        public string LineType
        {
            get { return lineType; }
            set { lineType = value; RaisePropertyChanged("LineType"); }
        }
    }
}
