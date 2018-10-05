namespace AMS.Module.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myob : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ModelDifferenceAspects",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            Xml = c.String(),
            //            Owner_ID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.ModelDifferences", t => t.Owner_ID)
            //    .Index(t => t.Owner_ID);
            
            //CreateTable(
            //    "dbo.ModelDifferences",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            UserId = c.String(),
            //            ContextId = c.String(),
            //            Version = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.ModuleInfoes",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            Version = c.String(),
            //            AssemblyFileName = c.String(),
            //            IsMain = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.MYOBAccounts",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 30),
            //            DisplayID = c.String(maxLength: 6),
            //            Type = c.String(maxLength: 60),
            //            Uid = c.Guid(nullable: false),
            //            RowVersion = c.String(maxLength: 30),
            //            TaxCode_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.MYOBTaxCodes", t => t.TaxCode_Id)
            //    .Index(t => t.DisplayID, unique: true, name: "IX_MYOBAccounts_DisplayId")
            //    .Index(t => t.TaxCode_Id);
            
            //CreateTable(
            //    "dbo.MYOBTaxCodes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Code = c.String(maxLength: 3),
            //            Description = c.String(maxLength: 30),
            //            Rate = c.Double(nullable: false),
            //            URI = c.String(),
            //            TypeCode = c.String(),
            //            Uid = c.Guid(nullable: false),
            //            RowVersion = c.String(maxLength: 30),
            //            TaxCollectedAccount_Id = c.Int(),
            //            TaxPaidAccount_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.MYOBAccounts", t => t.TaxCollectedAccount_Id)
            //    .ForeignKey("dbo.MYOBAccounts", t => t.TaxPaidAccount_Id)
            //    .Index(t => t.TaxCollectedAccount_Id)
            //    .Index(t => t.TaxPaidAccount_Id);
            
            //CreateTable(
            //    "dbo.MYOBCustomers",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(maxLength: 50),
            //            LastName = c.String(maxLength: 50),
            //            FirstName = c.String(maxLength: 20),
            //            IsIndividual = c.Boolean(nullable: false),
            //            LastModified = c.DateTime(),
            //            DisplayID = c.String(maxLength: 15),
            //            IsActive = c.Boolean(nullable: false),
            //            Uid = c.Guid(nullable: false),
            //            RowVersion = c.String(maxLength: 30),
            //            URI = c.String(),
            //            DocumentPrintActionId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.CompanyName, unique: true, name: "IX_MYOBContact_Name");
            
            //CreateTable(
            //    "dbo.MYOBCustomerAddresses",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Location = c.Int(nullable: false),
            //            Street = c.String(maxLength: 255),
            //            City = c.String(maxLength: 255),
            //            State = c.String(maxLength: 255),
            //            Postcode = c.String(maxLength: 15),
            //            Phone1 = c.String(maxLength: 21),
            //            Country = c.String(maxLength: 255),
            //            Phone2 = c.String(maxLength: 21),
            //            Phone3 = c.String(maxLength: 21),
            //            Fax = c.String(maxLength: 21),
            //            Email = c.String(maxLength: 255),
            //            WebSite = c.String(maxLength: 255),
            //            ContactName = c.String(maxLength: 25),
            //            Salutation = c.String(maxLength: 15),
            //            MyobCustomer_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.MYOBCustomers", t => t.MyobCustomer_Id)
            //    .Index(t => t.MyobCustomer_Id);
            
            //CreateTable(
            //    "dbo.MYOBItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //            URI = c.String(),
            //            LastModified = c.DateTime(),
            //            Number = c.String(nullable: false, maxLength: 30),
            //            IsBought = c.Boolean(nullable: false),
            //            IsSold = c.Boolean(nullable: false),
            //            IsInventoried = c.Boolean(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            UseDescription = c.Boolean(nullable: false),
            //            Name = c.String(maxLength: 30),
            //            UnitDescription = c.String(maxLength: 30),
            //            UnitName = c.String(),
            //            Type = c.Int(nullable: false),
            //            Uid = c.Guid(nullable: false),
            //            RowVersion = c.String(maxLength: 30),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Number, unique: true, name: "IX_MYOBItem_Number");
            
            //CreateTable(
            //    "dbo.MYOBSuppliers",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(maxLength: 50),
            //            LastName = c.String(maxLength: 50),
            //            FirstName = c.String(maxLength: 20),
            //            IsIndividual = c.Boolean(nullable: false),
            //            LastModified = c.DateTime(),
            //            DisplayID = c.String(maxLength: 15),
            //            IsActive = c.Boolean(nullable: false),
            //            Uid = c.Guid(nullable: false),
            //            RowVersion = c.String(maxLength: 30),
            //            URI = c.String(),
            //            DocumentPrintActionId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.CompanyName, unique: true, name: "IX_MYOBContact_Name");
            
            //CreateTable(
            //    "dbo.MYOBSupplierAddresses",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Location = c.Int(nullable: false),
            //            Street = c.String(maxLength: 255),
            //            City = c.String(maxLength: 255),
            //            State = c.String(maxLength: 255),
            //            Postcode = c.String(maxLength: 15),
            //            Phone1 = c.String(maxLength: 21),
            //            Country = c.String(maxLength: 255),
            //            Phone2 = c.String(maxLength: 21),
            //            Phone3 = c.String(maxLength: 21),
            //            Fax = c.String(maxLength: 21),
            //            Email = c.String(maxLength: 255),
            //            WebSite = c.String(maxLength: 255),
            //            ContactName = c.String(maxLength: 25),
            //            Salutation = c.String(maxLength: 15),
            //            MyobSupplier_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.MYOBSuppliers", t => t.MyobSupplier_Id)
            //    .Index(t => t.MyobSupplier_Id);
            
            //CreateTable(
            //    "dbo.PermissionPolicyRoleBases",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            IsAdministrative = c.Boolean(nullable: false),
            //            CanEditModel = c.Boolean(nullable: false),
            //            PermissionPolicy = c.Int(nullable: false),
            //            IsAllowPermissionPriority = c.Boolean(nullable: false),
            //            Discriminator = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyNavigationPermissionObjects",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            ItemPath = c.String(),
            //            TargetTypeFullName = c.String(),
            //            NavigateState = c.Int(),
            //            Role_ID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.PermissionPolicyRoleBases", t => t.Role_ID)
            //    .Index(t => t.Role_ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyTypePermissionObjects",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            TargetTypeFullName = c.String(),
            //            ReadState = c.Int(),
            //            WriteState = c.Int(),
            //            CreateState = c.Int(),
            //            DeleteState = c.Int(),
            //            NavigateState = c.Int(),
            //            Role_ID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.PermissionPolicyRoleBases", t => t.Role_ID)
            //    .Index(t => t.Role_ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyMemberPermissionsObjects",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Members = c.String(),
            //            Criteria = c.String(),
            //            ReadState = c.Int(),
            //            WriteState = c.Int(),
            //            TypePermissionObject_ID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.PermissionPolicyTypePermissionObjects", t => t.TypePermissionObject_ID)
            //    .Index(t => t.TypePermissionObject_ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyObjectPermissionsObjects",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Criteria = c.String(),
            //            ReadState = c.Int(),
            //            WriteState = c.Int(),
            //            DeleteState = c.Int(),
            //            NavigateState = c.Int(),
            //            TypePermissionObject_ID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID)
            //    .ForeignKey("dbo.PermissionPolicyTypePermissionObjects", t => t.TypePermissionObject_ID)
            //    .Index(t => t.TypePermissionObject_ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyUsers",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            UserName = c.String(),
            //            IsActive = c.Boolean(nullable: false),
            //            ChangePasswordOnFirstLogon = c.Boolean(nullable: false),
            //            StoredPassword = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.PermissionPolicyUserPermissionPolicyRoles",
            //    c => new
            //        {
            //            PermissionPolicyUser_ID = c.Int(nullable: false),
            //            PermissionPolicyRole_ID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.PermissionPolicyUser_ID, t.PermissionPolicyRole_ID })
            //    .ForeignKey("dbo.PermissionPolicyUsers", t => t.PermissionPolicyUser_ID, cascadeDelete: true)
            //    .ForeignKey("dbo.PermissionPolicyRoleBases", t => t.PermissionPolicyRole_ID, cascadeDelete: true)
            //    .Index(t => t.PermissionPolicyUser_ID)
            //    .Index(t => t.PermissionPolicyRole_ID);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.PermissionPolicyUserPermissionPolicyRoles", "PermissionPolicyRole_ID", "dbo.PermissionPolicyRoleBases");
            //DropForeignKey("dbo.PermissionPolicyUserPermissionPolicyRoles", "PermissionPolicyUser_ID", "dbo.PermissionPolicyUsers");
            //DropForeignKey("dbo.PermissionPolicyTypePermissionObjects", "Role_ID", "dbo.PermissionPolicyRoleBases");
            //DropForeignKey("dbo.PermissionPolicyObjectPermissionsObjects", "TypePermissionObject_ID", "dbo.PermissionPolicyTypePermissionObjects");
            //DropForeignKey("dbo.PermissionPolicyMemberPermissionsObjects", "TypePermissionObject_ID", "dbo.PermissionPolicyTypePermissionObjects");
            //DropForeignKey("dbo.PermissionPolicyNavigationPermissionObjects", "Role_ID", "dbo.PermissionPolicyRoleBases");
            //DropForeignKey("dbo.MYOBSupplierAddresses", "MyobSupplier_Id", "dbo.MYOBSuppliers");
            //DropForeignKey("dbo.MYOBCustomerAddresses", "MyobCustomer_Id", "dbo.MYOBCustomers");
            //DropForeignKey("dbo.MYOBAccounts", "TaxCode_Id", "dbo.MYOBTaxCodes");
            //DropForeignKey("dbo.MYOBTaxCodes", "TaxPaidAccount_Id", "dbo.MYOBAccounts");
            //DropForeignKey("dbo.MYOBTaxCodes", "TaxCollectedAccount_Id", "dbo.MYOBAccounts");
            //DropForeignKey("dbo.ModelDifferenceAspects", "Owner_ID", "dbo.ModelDifferences");
            //DropIndex("dbo.PermissionPolicyUserPermissionPolicyRoles", new[] { "PermissionPolicyRole_ID" });
            //DropIndex("dbo.PermissionPolicyUserPermissionPolicyRoles", new[] { "PermissionPolicyUser_ID" });
            //DropIndex("dbo.PermissionPolicyObjectPermissionsObjects", new[] { "TypePermissionObject_ID" });
            //DropIndex("dbo.PermissionPolicyMemberPermissionsObjects", new[] { "TypePermissionObject_ID" });
            //DropIndex("dbo.PermissionPolicyTypePermissionObjects", new[] { "Role_ID" });
            //DropIndex("dbo.PermissionPolicyNavigationPermissionObjects", new[] { "Role_ID" });
            //DropIndex("dbo.MYOBSupplierAddresses", new[] { "MyobSupplier_Id" });
            //DropIndex("dbo.MYOBSuppliers", "IX_MYOBContact_Name");
            //DropIndex("dbo.MYOBItems", "IX_MYOBItem_Number");
            //DropIndex("dbo.MYOBCustomerAddresses", new[] { "MyobCustomer_Id" });
            //DropIndex("dbo.MYOBCustomers", "IX_MYOBContact_Name");
            //DropIndex("dbo.MYOBTaxCodes", new[] { "TaxPaidAccount_Id" });
            //DropIndex("dbo.MYOBTaxCodes", new[] { "TaxCollectedAccount_Id" });
            //DropIndex("dbo.MYOBAccounts", new[] { "TaxCode_Id" });
            //DropIndex("dbo.MYOBAccounts", "IX_MYOBAccounts_DisplayId");
            //DropIndex("dbo.ModelDifferenceAspects", new[] { "Owner_ID" });
            //DropTable("dbo.PermissionPolicyUserPermissionPolicyRoles");
            //DropTable("dbo.PermissionPolicyUsers");
            //DropTable("dbo.PermissionPolicyObjectPermissionsObjects");
            //DropTable("dbo.PermissionPolicyMemberPermissionsObjects");
            //DropTable("dbo.PermissionPolicyTypePermissionObjects");
            //DropTable("dbo.PermissionPolicyNavigationPermissionObjects");
            //DropTable("dbo.PermissionPolicyRoleBases");
            //DropTable("dbo.MYOBSupplierAddresses");
            //DropTable("dbo.MYOBSuppliers");
            //DropTable("dbo.MYOBItems");
            //DropTable("dbo.MYOBCustomerAddresses");
            //DropTable("dbo.MYOBCustomers");
            //DropTable("dbo.MYOBTaxCodes");
            //DropTable("dbo.MYOBAccounts");
            //DropTable("dbo.ModuleInfoes");
            //DropTable("dbo.ModelDifferences");
            //DropTable("dbo.ModelDifferenceAspects");
        }
    }
}
