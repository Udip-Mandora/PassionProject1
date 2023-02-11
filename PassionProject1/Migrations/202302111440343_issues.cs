namespace PassionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.issues",
                c => new
                    {
                        issueId = c.Int(nullable: false, identity: true),
                        issueName = c.String(),
                        issueDescription = c.String(),
                    })
                .PrimaryKey(t => t.issueId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.issues");
        }
    }
}
