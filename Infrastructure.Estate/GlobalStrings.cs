using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Estate
{
    public class DatabaseInfo
    {
        public const string fullConnectionString = @"metadata=res://*/Estate.csdl|res://*/Estate.ssdl|res://*/Estate.msl;provider=System.Data.SQLite;provider connection string='data source=databases\estate'";
        public const string connectionString = @"data source=databases\EstateCompact.sdf";
    }

    public class RegionNames
    {
        public const string MenuPanelRegion = "MenuPanel";
        public const string ContentPanelRegion = "ContentPanel";
        public const string DatabaseDirectoryRegion = "DatabaseDirectory";
    }
}
