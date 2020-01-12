using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Telerik.Windows.Controls;
using ViewDesigner.Models;
using SerializationInfo = Telerik.Windows.Diagrams.Core.SerializationInfo;

namespace ViewDesigner.Common
{
    /// <summary>
    /// 图形Shape控件
    /// </summary>
    public class FigureShape : RadDiagramShape
    {
        public FigureShape()
        {
            IsConnectorsManipulationEnabled = false;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override SerializationInfo Serialize()
        {
            SerializationInfo serializationInfo = base.Serialize();

            try
            {
                var obj = base.Content as FigureBase;
                if (obj != null)
                {
                    IFormatter formatter = new BinaryFormatter();
                    using (var ms = new MemoryStream())
                    {
                        formatter.Serialize(ms, obj);
                        serializationInfo["Figure"] = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("序列化过程失败:" + e.Message);
            }
            return serializationInfo;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="serializationInfo"></param>
        public override void Deserialize(SerializationInfo serializationInfo)
        {
            base.Deserialize(serializationInfo);

            try
            {
                if (serializationInfo["Figure"] != null)
                {
                    var buffer = Convert.FromBase64String(serializationInfo["Figure"].ToString());
                    IFormatter formatter = new BinaryFormatter();
                    using (var ms = new MemoryStream(buffer))
                    {
                        Content = formatter.Deserialize(ms);
                        //绑定Shape坐标和Figure坐标
                        this.DataContext = Content;
                        var binding = new Binding("Position") { Mode = BindingMode.TwoWay };
                        this.SetBinding(PositionProperty, binding);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("反序列化过程失败:" + e.Message);
            }
        }
    }
}
