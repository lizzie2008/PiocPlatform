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
    public class StationFig : FigureBase
    {
        /// <summary>
        /// xml节点构造
        /// </summary>
        /// <param name="node"></param>
        public StationFig(XmlNode node)
        {
            var infoNode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(s => s.Name == "use");
            var xAttri = infoNode.GetAttributeByName("x");
            var yAttri = infoNode.GetAttributeByName("y");

            this.Position = new Point(double.Parse(xAttri), double.Parse(yAttri));
            this.StationType = infoNode.GetAttributeByName("class");
        }

        /// <summary>
        /// 厂站类型(220kv,500kv)
        /// </summary>
        private string stationType;
        public string StationType
        {
            get { return stationType; }
            set { stationType = value; RaisePropertyChanged("StationType"); }
        }
    }
}
