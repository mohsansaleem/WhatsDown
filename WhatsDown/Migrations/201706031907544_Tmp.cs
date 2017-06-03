namespace WhatsDown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tmp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserConversations", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.UserConversations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserConversations", new[] { "Conversation_Id" });
            DropIndex("dbo.UserConversations", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "Conversation_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropColumn("dbo.AspNetUsers", "LastSeen");
            DropTable("dbo.Conversations");
            DropTable("dbo.UserConversations");
            DropTable("dbo.Messages");
            DropTable("dbo.Tests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageTitle = c.String(),
                        MessageBody = c.String(),
                        SendTime = c.DateTime(nullable: false),
                        Conversation_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserConversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        LastActiveOnThread = c.DateTime(nullable: false),
                        LastDelivered = c.DateTime(nullable: false),
                        Conversation_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LastSeen", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Messages", "User_Id");
            CreateIndex("dbo.Messages", "Conversation_Id");
            CreateIndex("dbo.UserConversations", "User_Id");
            CreateIndex("dbo.UserConversations", "Conversation_Id");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "Conversation_Id", "dbo.Conversations", "Id");
            AddForeignKey("dbo.UserConversations", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserConversations", "Conversation_Id", "dbo.Conversations", "Id");
        }
    }
}
