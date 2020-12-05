using System;
using System.Collections.Generic;

namespace Common.DBModel
{
    public partial class TFileinfo
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MappingKey { get; set; }
        public string FilePath { get; set; }
        public string TypeId { get; set; }
        public string CreateName { get; set; }
        public string CreateIp { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateName { get; set; }
        public string UpdateIp { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
