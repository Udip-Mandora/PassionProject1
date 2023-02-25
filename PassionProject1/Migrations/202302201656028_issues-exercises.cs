namespace PassionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issuesexercises : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.issuesexercises",
                c => new
                    {
                        issues_issueId = c.Int(nullable: false),
                        exercise_exerciseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.issues_issueId, t.exercise_exerciseId })
                .ForeignKey("dbo.issues", t => t.issues_issueId, cascadeDelete: true)
                .ForeignKey("dbo.exercises", t => t.exercise_exerciseId, cascadeDelete: true)
                .Index(t => t.issues_issueId)
                .Index(t => t.exercise_exerciseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.issuesexercises", "exercise_exerciseId", "dbo.exercises");
            DropForeignKey("dbo.issuesexercises", "issues_issueId", "dbo.issues");
            DropIndex("dbo.issuesexercises", new[] { "exercise_exerciseId" });
            DropIndex("dbo.issuesexercises", new[] { "issues_issueId" });
            DropTable("dbo.issuesexercises");
        }
    }
}
