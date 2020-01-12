using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ViewDesigner.Common;
using Point = System.Windows.Point;

namespace ViewDesigner.Models.SvgFigs
{
    [Serializable]
    public class TextFig : FigureBase
    {
        /// <summary>
        /// xml节点构造
        /// </summary>
        /// <param name="node"></param>
        public TextFig(XmlNode node)
        {
            var infoNode = node;
            var xAttri = infoNode.GetAttributeByName("x");
            var yAttri = infoNode.GetAttributeByName("y");
            var fontSizeAttri = infoNode.GetAttributeByName("font-size");
            var fontFamilyAttri = infoNode.GetAttributeByName("font-family");

            this.Text = infoNode.InnerText;
            this.FontSize = double.Parse(fontSizeAttri);
            this.FontFamily = fontFamilyAttri;

            var formattedText = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            new Typeface(fontFamily), fontSize, Brushes.White);
            var halfWidth = formattedText.WidthIncludingTrailingWhitespace / 2;
            var halfHeight = formattedText.Height / 2 + 4;
            this.Position = new Point(double.Parse(xAttri) - halfWidth, double.Parse(yAttri) - halfHeight);
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; RaisePropertyChanged("Text"); }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        private double fontSize;
        public double FontSize
        {
            get { return fontSize; }
            set { fontSize = value; RaisePropertyChanged("FontSize"); }
        }
        /// <summary>
        /// 字体
        /// </summary>
        private string fontFamily;
        public string FontFamily
        {
            get { return fontFamily; }
            set { fontFamily = value; RaisePropertyChanged("FontFamily"); }
        }
    }
}
