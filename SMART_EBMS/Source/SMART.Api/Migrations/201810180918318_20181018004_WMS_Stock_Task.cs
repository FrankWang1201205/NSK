namespace SMART.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20181018004_WMS_Stock_Task : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WMS_Stock_Task", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WMS_Stock_Task", "Type");
        }
    }
}
