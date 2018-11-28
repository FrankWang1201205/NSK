using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using SMART.Api.Models;

namespace SMART.Api
{
    public class SmartdbContext : DbContext
    {
        // 构造函数中的参数用于指定 connectionStrings 的 name
        public SmartdbContext()
            : base("SmartdbConnectionString")
        {
            // DbContextConfiguration.LazyLoadingEnabled - 是否启用延迟加载，默认值为 true
            //     true - 延迟加载（Lazy Loading）：获取实体时不会加载其导航属性，一旦用到导航属性就会自动加载
            //     false - 直接加载（Eager loading）：通过 Include 之类的方法显示加载导航属性，获取实体时会即时加载通过 Include 指定的导航属性
            this.Configuration.LazyLoadingEnabled = false;

            // DbContextConfiguration.AutoDetectChangesEnabled - 是否自动监测变化，默认值为 true
            //this.Configuration.AutoDetectChangesEnabled = false;

            //this.Configuration.ValidateOnSaveEnabled = false;

            //this.Configuration.ProxyCreationEnabled = false;
        }

        //这个方法可以没有，重写的这个方法里，我们可以移除一些契约，还可以配置数据库映射关系
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//防止黑幕交易 要不然每次都要访问 
            modelBuilder.Entity<Material>().Property(c => c.Price_AM).HasPrecision(18, 8);
            modelBuilder.Entity<Material>().Property(c => c.Price_AM_Rate).HasPrecision(18, 3);

            modelBuilder.Entity<Material>().Property(c => c.Price_Cost_Ref).HasPrecision(18, 8);
            modelBuilder.Entity<Material>().Property(c => c.Price_Cost_Ref_Vat).HasPrecision(18, 8);
            modelBuilder.Entity<Material>().Property(c => c.Price_Cost_Target).HasPrecision(18, 8);

            modelBuilder.Entity<Material>().Property(c => c.Price_Retail_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Material>().Property(c => c.Price_Trade_A_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Material>().Property(c => c.Price_Trade_B_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Material>().Property(c => c.Price_Trade_NoTax_Rate).HasPrecision(18, 3);


            modelBuilder.Entity<Material_CODE>().Property(c => c.Order_Price).HasPrecision(18, 8);

            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.CODE_Order_Price).HasPrecision(18, 8);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_AM).HasPrecision(18, 8);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_AM_Rate).HasPrecision(18, 4);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Cost_Ref).HasPrecision(18, 8);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Cost_Target).HasPrecision(18, 8);

            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Retail_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Trade_A_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Trade_B_Rate).HasPrecision(18, 3);
            modelBuilder.Entity<Mat_Excel_Line>().Property(c => c.Price_Trade_NoTax_Rate).HasPrecision(18, 3);

            modelBuilder.Entity<Po_Line>().Property(c => c.CostPrice).HasPrecision(18, 8);
            modelBuilder.Entity<RFQ_Head_Line>().Property(c => c.Sales_Price).HasPrecision(18, 8);
            modelBuilder.Entity<RFQ_Head_Line_To_Buyer>().Property(c => c.Cost_Price).HasPrecision(18, 8);

            modelBuilder.Entity<WMS_In_Line>().Property(c => c.Price_Cost).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Stock>().Property(c => c.Price).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Out_Line>().Property(c => c.Price).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Track>().Property(c => c.Logistics_Cost).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Track>().Property(c => c.Kilometers).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Track>().Property(c => c.Weight).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Stock_Record>().Property(c => c.Price).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Stock_Temp>().Property(c => c.Price).HasPrecision(18, 8);
            modelBuilder.Entity<WMS_Profit_Loss>().Property(c => c.Price).HasPrecision(18, 8);

            modelBuilder.Entity<Purchase_Temp>().Property(c => c.Contract_Price).HasPrecision(18, 8);
            modelBuilder.Entity<Purchase_Temp_Search>().Property(c => c.Price_Cost).HasPrecision(18, 8);

        }

        //Enable-Migrations
        //Add-Migration xxxxx
        //Update-Database
        public DbSet<Material> Material { get; set; }
        public DbSet<Material_Name> Material_Name { get; set; }
        public DbSet<Material_CODE> Material_CODE { get; set; }
        public DbSet<MatImage> MatImage { get; set; }
        public DbSet<MatImage_Detail> MatImage_Detail { get; set; }

        public DbSet<Mat_Excel> Mat_Excel { get; set; }
        public DbSet<Mat_Excel_Line> Mat_Excel_Line { get; set; }

        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }

        public DbSet<MatSales_Lib> MatSales_Lib { get; set; }
        public DbSet<SalesPlan> SalesPlan { get; set; }
        public DbSet<SalesPlan_Line> SalesPlan_Line { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<User_Profile> User_Profile { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<MainCompany> MainCompany { get; set; }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Customer_Group> Customer_Group { get; set; }
        public DbSet<Customer_Mat> Customer_Mat { get; set; }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<PurchasePlan_Line> PurchasePlan_Line { get; set; }

        public DbSet<Po_Head> Po_Head { get; set; }
        public DbSet<Po_Line> Po_Line { get; set; }

        public DbSet<RFQ_Head> RFQ_Head { get; set; }
        public DbSet<RFQ_Head_Line> RFQ_Head_Line { get; set; }
        public DbSet<RFQ_Head_Line_To_Buyer> RFQ_Head_Line_To_Buyer { get; set; }

        public DbSet<WMS_Logistics> WMS_Logistics { get; set; }
        public DbSet<WMS_Location> WMS_Location { get; set; }
        public DbSet<WMS_Work_Person> WMS_Work_Person { get; set; }
        public DbSet<WMS_In_Head> WMS_In_Head { get; set; }
        public DbSet<WMS_In_Line> WMS_In_Line { get; set; }
        public DbSet<WMS_In_Line_Other> WMS_In_Line_Other { get; set; }
        public DbSet<WMS_In_Scan> WMS_In_Scan { get; set; }
        public DbSet<WMS_In_Scan_Error> WMS_In_Scan_Error { get; set; }
        
        public DbSet<WMS_Track> WMS_Track { get; set; }

        public DbSet<WMS_Stock> WMS_Stock { get; set; }
        public DbSet<WMS_Stock_Temp> WMS_Stock_Temp { get; set; }
        public DbSet<WMS_Stock_Record> WMS_Stock_Record { get; set; }
        public DbSet<WMS_Stock_Task> WMS_Stock_Task { get; set; }

        public DbSet<WMS_Out_Head> WMS_Out_Head { get; set; }
        public DbSet<WMS_Out_Line> WMS_Out_Line { get; set; }
        public DbSet<WMS_Out_Scan> WMS_Out_Scan { get; set; }
        public DbSet<WMS_Out_Pick_Scan> WMS_Out_Pick_Scan { get; set; }
        
        public DbSet<WMS_Move> WMS_Move { get; set; }
        public DbSet<WMS_Move_Scan> WMS_Move_Scan { get; set; }
        public DbSet<WMS_Move_Record> WMS_Move_Record { get; set; }

        public DbSet<WMS_Stocktaking> WMS_Stocktaking { get; set; }
        public DbSet<WMS_Stocktaking_Scan> WMS_Stocktaking_Scan { get; set; }
        public DbSet<WMS_Profit_Loss> WMS_Profit_Loss { get; set; }
        public DbSet<WMS_Profit_Loss_Other> WMS_Profit_Loss_Other { get; set; }
        
        public DbSet<MaxInt> MaxInt { get; set; }
        public DbSet<SentEmail> SentEmail { get; set; }
        public DbSet<SentEmailRecord> SentEmailRecord { get; set; }

        public DbSet<Purchase_Temp> Purchase_Temp { get; set; }
        public DbSet<Purchase_Temp_Search> Purchase_Temp_Search { get; set; }

    }
}
