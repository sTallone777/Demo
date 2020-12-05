using System;
using System.Collections.Generic;

namespace Common.DBModel
{
    public partial class MDriver
    {
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string FilesysType { get; set; }
        public int SortKey { get; set; }
        public string CreateName { get; set; }
        public string CreateIp { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateName { get; set; }
        public string UpdateIp { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
