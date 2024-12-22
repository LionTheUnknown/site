namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sandelisproduktas",
                c => new
                    {
                        fk_produktas = c.Int(nullable: false),
                        fk_sandelis = c.Int(nullable: false),
                        kiekis = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.fk_produktas, t.fk_sandelis })
                .ForeignKey("dbo.produktas", t => t.fk_produktas, cascadeDelete: true)
                .ForeignKey("dbo.sandelis", t => t.fk_sandelis, cascadeDelete: true)
                .Index(t => t.fk_produktas)
                .Index(t => t.fk_sandelis);
            
            CreateTable(
                "dbo.nuolaidoskodas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sukurimodata = c.DateTime(nullable: false),
                        veikimopradziosdata = c.DateTime(),
                        panaudojimuskaičius = c.Int(),
                        kodas = c.String(),
                        verte = c.Double(nullable: false),
                        aprasymas = c.String(),
                        pavadinimas = c.String(nullable: false),
                        galiojimopabaigosdata = c.DateTime(),
                        yraribotas = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.nuolaidoskodas_produktas",
                c => new
                    {
                        fk_nuolaidoskodas = c.Int(nullable: false),
                        fk_produktas = c.Int(nullable: false),
                        minkiekis = c.Int(),
                    })
                .PrimaryKey(t => new { t.fk_nuolaidoskodas, t.fk_produktas })
                .ForeignKey("dbo.nuolaidoskodas", t => t.fk_nuolaidoskodas, cascadeDelete: true)
                .ForeignKey("dbo.produktas", t => t.fk_produktas, cascadeDelete: true)
                .Index(t => t.fk_nuolaidoskodas)
                .Index(t => t.fk_produktas);
            
            CreateTable(
                "dbo.uzsakymasproduktas",
                c => new
                    {
                        fk_produktas = c.Int(nullable: false),
                        fk_uzsakymas = c.Int(nullable: false),
                        kaina = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.fk_produktas, t.fk_uzsakymas })
                .ForeignKey("dbo.uzsakymas", t => t.fk_uzsakymas, cascadeDelete: true)
                .ForeignKey("dbo.produktas", t => t.fk_produktas, cascadeDelete: true)
                .Index(t => t.fk_produktas)
                .Index(t => t.fk_uzsakymas);
            
            CreateTable(
                "dbo.mokestis",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        mokestis = c.Single(nullable: false),
                        fk_mokejimotipas = c.Int(nullable: false),
                        fk_pirkejas = c.Int(nullable: false),
                        fk_uzsakymas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.atlyginimas",
                c => new
                    {
                        FkAdministratorius = c.Int(nullable: false),
                        FkAdministratorius1 = c.Int(nullable: false),
                        mokestis = c.Single(),
                        bonusas = c.Single(),
                    })
                .PrimaryKey(t => new { t.FkAdministratorius, t.FkAdministratorius1 });
            
            AddColumn("dbo.uzsakymas", "statusas", c => c.String());
            AlterColumn("dbo.uzsakymas", "verte", c => c.Double(nullable: false));
            DropColumn("dbo.produktas", "kiekis");
        }
        
        public override void Down()
        {
            AddColumn("dbo.produktas", "kiekis", c => c.Int(nullable: false));
            DropForeignKey("dbo.uzsakymasproduktas", "fk_produktas", "dbo.produktas");
            DropForeignKey("dbo.uzsakymasproduktas", "fk_uzsakymas", "dbo.uzsakymas");
            DropForeignKey("dbo.nuolaidoskodas_produktas", "fk_produktas", "dbo.produktas");
            DropForeignKey("dbo.nuolaidoskodas_produktas", "fk_nuolaidoskodas", "dbo.nuolaidoskodas");
            DropForeignKey("dbo.sandelisproduktas", "fk_sandelis", "dbo.sandelis");
            DropForeignKey("dbo.sandelisproduktas", "fk_produktas", "dbo.produktas");
            DropIndex("dbo.uzsakymasproduktas", new[] { "fk_uzsakymas" });
            DropIndex("dbo.uzsakymasproduktas", new[] { "fk_produktas" });
            DropIndex("dbo.nuolaidoskodas_produktas", new[] { "fk_produktas" });
            DropIndex("dbo.nuolaidoskodas_produktas", new[] { "fk_nuolaidoskodas" });
            DropIndex("dbo.sandelisproduktas", new[] { "fk_sandelis" });
            DropIndex("dbo.sandelisproduktas", new[] { "fk_produktas" });
            AlterColumn("dbo.uzsakymas", "verte", c => c.Single(nullable: false));
            DropColumn("dbo.uzsakymas", "statusas");
            DropTable("dbo.atlyginimas");
            DropTable("dbo.mokestis");
            DropTable("dbo.uzsakymasproduktas");
            DropTable("dbo.nuolaidoskodas_produktas");
            DropTable("dbo.nuolaidoskodas");
            DropTable("dbo.sandelisproduktas");
        }
    }
}
