using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ViewDesigner.Models;
using ViewDesigner.Models.SvgFigs;

namespace ViewDesigner.Common
{
    /// <summary>
    /// SVG图读取
    /// </summary>
    public static class SvgReader
    {
        /// <summary>
        /// 根据属性名称获得属性字符串
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetAttributeByName(this XmlNode node,string attribute)
        {
            if (node.Attributes != null)
            {
                return node.Attributes[attribute].Value;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 加载SVG图,获得图元信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<FigureBase> LoadSvgFile(string fileName)
        {
            var result = new List<FigureBase>();

            try
            {
                var doc = new XmlDocument();
                doc.Load(fileName);
                var root = doc.ChildNodes.Cast<XmlNode>().First(s => s.Name == "svg");
                if (root != null)
                {
                    var gNodes = (from XmlNode s in root.ChildNodes where s.Name == "g" select s).ToList();
                    //线路信息
                    var lineInfo = gNodes.FirstOrDefault(s => s.Attributes != null && s.Attributes["id"].Value == "ACLineSegmentClass");
                    if (lineInfo != null)
                    {
                        result.AddRange(lineInfo.ChildNodes.Cast<XmlNode>().Select(lineNode => new LineFig(lineNode)));
                    }
                    //厂站信息
                    var stationInfo = gNodes.FirstOrDefault(s => s.Attributes != null && s.Attributes["id"].Value == "SubStationClass");
                    if (stationInfo != null)
                    {
                        result.AddRange(stationInfo.ChildNodes.Cast<XmlNode>().Select(stationNode => new StationFig(stationNode)));
                    }
                    //文本信息
                    var textInfo = gNodes.FirstOrDefault(s => s.Attributes != null && s.Attributes["id"].Value == "TextClass");
                    if (textInfo != null)
                    {
                        result.AddRange(textInfo.ChildNodes.Cast<XmlNode>().Select(textNode => new TextFig(textNode)));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("打开svg图失败:"+e.Message);
            }

            return result;
        }
    }
}
