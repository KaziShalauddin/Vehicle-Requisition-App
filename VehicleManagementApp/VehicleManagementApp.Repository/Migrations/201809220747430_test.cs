namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
           CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        CommntId = c.Int(),
                        EmployeeId = c.Int(nullable: false),
                        RequsitionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommntId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Requsitions", t => t.RequsitionId, cascadeDelete: true)
                .Index(t => t.CommntId)
                .Index(t => t.EmployeeId)
                .Index(t => t.RequsitionId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(maxLength: 255),
                        ContactNo = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        Address1 = c.String(maxLength: 255),
                        Address2 = c.String(),
                        LicenceNo = c.String(maxLength: 255),
                        IsDriver = c.Boolean(nullable: false),
                        DepartmentId = c.Int(),
                        DesignationId = c.Int(),
                        DivisionId = c.Int(),
                        DistrictId = c.Int(),
                        ThanaId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Designations", t => t.DesignationId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.Thanas", t => t.ThanaId)
                .Index(t => t.DepartmentId)
                .Index(t => t.DesignationId)
                .Index(t => t.DivisionId)
                .Index(t => t.DistrictId)
                .Index(t => t.ThanaId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OrganaizationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organaizations", t => t.OrganaizationId, cascadeDelete: true)
                .Index(t => t.OrganaizationId);
            
            CreateTable(
                "dbo.Organaizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        OrganaizationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organaizations", t => t.OrganaizationId, cascadeDelete: true)
                .Index(t => t.OrganaizationId);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        DivisionId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Thanas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
            CreateTable(
                "dbo.Requsitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Form = c.String(),
                        To = c.String(),
                        Description = c.String(),
                        JourneyStart = c.DateTime(nullable: false),
                        JouneyEnd = c.DateTime(nullable: false),
                        Status = c.String(),
                        EmployeeId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverNo = c.String(),
                        Status = c.String(),
                        RequsitionId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        EmployeeId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Requsitions", t => t.RequsitionId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.RequsitionId)
                .Index(t => t.VehicleId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleName = c.String(),
                        VModel = c.String(),
                        VRegistrationNo = c.String(),
                        VChesisNo = c.String(),
                        VCapacity = c.String(),
                        Description = c.String(),
                        Status = c.String(),
                        VehicleTypeId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Managers", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.Managers", "RequsitionId", "dbo.Requsitions");
            DropForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Comments", "RequsitionId", "dbo.Requsitions");
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "ThanaId", "dbo.Thanas");
            DropForeignKey("dbo.Thanas", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Employees", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Employees", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Districts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Employees", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Designations", "OrganaizationId", "dbo.Organaizations");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "OrganaizationId", "dbo.Organaizations");
            DropForeignKey("dbo.Comments", "CommntId", "dbo.Comments");
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.Managers", new[] { "EmployeeId" });
            DropIndex("dbo.Managers", new[] { "VehicleId" });
            DropIndex("dbo.Managers", new[] { "RequsitionId" });
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            DropIndex("dbo.Thanas", new[] { "DistrictId" });
            DropIndex("dbo.Districts", new[] { "DivisionId" });
            DropIndex("dbo.Designations", new[] { "OrganaizationId" });
            DropIndex("dbo.Departments", new[] { "OrganaizationId" });
            DropIndex("dbo.Employees", new[] { "ThanaId" });
            DropIndex("dbo.Employees", new[] { "DistrictId" });
            DropIndex("dbo.Employees", new[] { "DivisionId" });
            DropIndex("dbo.Employees", new[] { "DesignationId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropIndex("dbo.Comments", new[] { "RequsitionId" });
            DropIndex("dbo.Comments", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "CommntId" });
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Managers");
            DropTable("dbo.Requsitions");
            DropTable("dbo.Thanas");
            DropTable("dbo.Divisions");
            DropTable("dbo.Districts");
            DropTable("dbo.Designations");
            DropTable("dbo.Organaizations");
            DropTable("dbo.Departments");
            DropTable("dbo.Employees");
            DropTable("dbo.Comments");
        }
    }
}
