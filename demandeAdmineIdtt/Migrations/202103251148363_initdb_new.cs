namespace demandeAdmineIdtt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdb_new : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestDocuments", "Request_Id", "dbo.Requests");
            DropIndex("dbo.RequestDocuments", new[] { "Request_Id" });
            DropPrimaryKey("dbo.Requests");
            DropPrimaryKey("dbo.RequestDocuments");
            AlterColumn("dbo.Requests", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.RequestDocuments", "Request_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Requests", "Id");
            AddPrimaryKey("dbo.RequestDocuments", new[] { "Request_Id", "Document_Id" });
            CreateIndex("dbo.RequestDocuments", "Request_Id");
            AddForeignKey("dbo.RequestDocuments", "Request_Id", "dbo.Requests", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestDocuments", "Request_Id", "dbo.Requests");
            DropIndex("dbo.RequestDocuments", new[] { "Request_Id" });
            DropPrimaryKey("dbo.RequestDocuments");
            DropPrimaryKey("dbo.Requests");
            AlterColumn("dbo.RequestDocuments", "Request_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Requests", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.RequestDocuments", new[] { "Request_Id", "Document_Id" });
            AddPrimaryKey("dbo.Requests", "Id");
            CreateIndex("dbo.RequestDocuments", "Request_Id");
            AddForeignKey("dbo.RequestDocuments", "Request_Id", "dbo.Requests", "Id", cascadeDelete: true);
        }
    }
}
