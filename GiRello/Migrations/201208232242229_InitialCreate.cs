namespace GiRello.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auths",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 128),
                        GithubUser = c.String(),
                        BitbucketUser = c.String(),
                    })
                .PrimaryKey(t => t.Token);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Auths");
        }
    }
}
