namespace demandeAdmineIdtt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Status");
        }
    }
}
