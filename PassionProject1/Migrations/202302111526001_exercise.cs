namespace PassionProject1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exercise : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.exercises",
                c => new
                    {
                        exerciseId = c.Int(nullable: false, identity: true),
                        exerciseName = c.String(),
                    })
                .PrimaryKey(t => t.exerciseId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.exercises");
        }
    }
}
