namespace SMART.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20181018001_WMS_Location : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WMS_Location", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WMS_Location", "Type");
        }
    }
}
