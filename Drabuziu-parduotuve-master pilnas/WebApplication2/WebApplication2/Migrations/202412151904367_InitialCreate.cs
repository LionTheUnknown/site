namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.kategorija",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pavadinimas = c.String(),
                        aprasas = c.String(),
                        fk_tevinekategorija = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.kategorija", t => t.fk_tevinekategorija)
                .Index(t => t.fk_tevinekategorija);
            
            CreateTable(
                "dbo.produktas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pavadinimas = c.String(),
                        aprasas = c.String(maxLength: 500),
                        kaina = c.Double(nullable: false),
                        mase = c.Double(nullable: false),
                        kiekis = c.Int(nullable: false),
                        gamintojas = c.String(),
                        nuotraukos_url = c.String(),
                        fk_pardavėjas = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.kategorijaProduktas",
                c => new
                    {
                        fk_produktas = c.Int(nullable: false),
                        fk_kategorija = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.fk_produktas, t.fk_kategorija })
                .ForeignKey("dbo.kategorija", t => t.fk_kategorija, cascadeDelete: true)
                .ForeignKey("dbo.produktas", t => t.fk_produktas, cascadeDelete: true)
                .Index(t => t.fk_produktas)
                .Index(t => t.fk_kategorija);
            
            CreateTable(
                "dbo.uzsakymas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        siuntimoadresas = c.String(),
                        kiekis = c.Int(nullable: false),
                        verte = c.Single(nullable: false),
                        pradzia = c.DateTime(nullable: false),
                        pabaiga = c.DateTime(nullable: false),
                        fk_pirkejas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.sandelis",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pavadinimas = c.String(),
                        vieta = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProductCategory1",
                c => new
                    {
                        Product_id = c.Int(nullable: false),
                        Category_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_id, t.Category_id })
                .ForeignKey("dbo.produktas", t => t.Product_id, cascadeDelete: true)
                .ForeignKey("dbo.kategorija", t => t.Category_id, cascadeDelete: true)
                .Index(t => t.Product_id)
                .Index(t => t.Category_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.kategorijaProduktas", "fk_produktas", "dbo.produktas");
            DropForeignKey("dbo.kategorijaProduktas", "fk_kategorija", "dbo.kategorija");
            DropForeignKey("dbo.ProductCategory1", "Category_id", "dbo.kategorija");
            DropForeignKey("dbo.ProductCategory1", "Product_id", "dbo.produktas");
            DropForeignKey("dbo.kategorija", "fk_tevinekategorija", "dbo.kategorija");
            DropIndex("dbo.ProductCategory1", new[] { "Category_id" });
            DropIndex("dbo.ProductCategory1", new[] { "Product_id" });
            DropIndex("dbo.kategorijaProduktas", new[] { "fk_kategorija" });
            DropIndex("dbo.kategorijaProduktas", new[] { "fk_produktas" });
            DropIndex("dbo.kategorija", new[] { "fk_tevinekategorija" });
            DropTable("dbo.ProductCategory1");
            DropTable("dbo.sandelis");
            DropTable("dbo.uzsakymas");
            DropTable("dbo.kategorijaProduktas");
            DropTable("dbo.produktas");
            DropTable("dbo.kategorija");
        }
    }
}
