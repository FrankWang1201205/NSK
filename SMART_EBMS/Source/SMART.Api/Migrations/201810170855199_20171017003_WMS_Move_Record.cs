namespace SMART.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20171017003_WMS_Move_Record : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WMS_Move_Record", "Move_Type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WMS_Move_Record", "Move_Type");
        }
    }
}
