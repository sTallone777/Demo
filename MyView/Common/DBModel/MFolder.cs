using System;
using System.Collections.Generic;

namespace Common.DBModel
{
    public partial class MFolder
    {
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public sbyte FolderLevel { get; set; }
        public string ParentId { get; set; }
        public string DriverId { get; set; }
        public int? SortKey { get; set; }
        public string CreateName { get; set; }
        public string CreateIp { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string UpdateName { get; set; }
        public string UpdateIp { get; set; }
        public DateTime UpdateDatetime { get; set; }
    }
}
