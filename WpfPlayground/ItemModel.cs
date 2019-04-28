using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WpfPlayground
{
    [DataContract]
    public class ItemModel
    {
        [IgnoreDataMember]
        private static int StaticID = 1;

        [DataMember]
        public int ID { get; private set; } = StaticID++;
        [DataMember]
        public int Quantity { get; set; } = 0;
        [DataMember]
        public string PartNumber { get; set; } = "";
        [DataMember]
        public string PartName { get; set; } = "";

        public override string ToString()
            => "ItemModel {\n" +
            $"\t\"{nameof(ID)}\" : \"{ID}\"\n" +
            $"\t\"{nameof(Quantity)}\" : \"{Quantity}\"\n" +
            $"\t\"{nameof(PartNumber)}\" : \"{PartNumber}\"\n" +
            $"\t\"{nameof(PartName)}\" : \"{PartName}\"\n" +
            "}\n";
    }
}
