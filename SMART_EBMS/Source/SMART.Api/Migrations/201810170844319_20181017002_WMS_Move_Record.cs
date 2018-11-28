namespace SMART.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20181017002_WMS_Move_Record : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WMS_Move_Record",
                c => new
                    {
                        Record_ID = c.Guid(nullable: false),
                        Out_Location = c.String(nullable: false),
                        In_Location = c.String(nullable: false),
                        Create_DT = c.DateTime(nullable: false),
                        LinkMainCID = c.Guid(nullable: false),
                        Link_TaskID = c.Guid(nullable: false),
                        Work_Person = c.String(),
                        MatSn = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Package_Type = c.String(),
                        MatName = c.String(),
                        MatBrand = c.String(),
                        MatUnit = c.String(),
                    })
                .PrimaryKey(t => t.Record_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WMS_Move_Record");
        }
    }
}
