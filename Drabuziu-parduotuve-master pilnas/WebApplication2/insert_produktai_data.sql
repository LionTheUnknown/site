CREATE TABLE [dbo].[produktas] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [pavadinimas]    VARCHAR (255) NOT NULL,
    [kaina]          FLOAT (53)    NOT NULL,
    [aprasas]        VARCHAR (MAX) NOT NULL,
    [mase]           FLOAT (53)    NOT NULL,
    [gamintojas]     VARCHAR (255) NOT NULL,
    [nuotraukos_url] VARCHAR (255) NOT NULL,
    [fk_pardavėjas]  INT           DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


INSERT INTO produktas (pavadinimas, kaina, aprasas, mase, gamintojas, nuotraukos_url, fk_pardavėjas)
VALUES
('Baltas Suknelė', 89.99, 'Elegantiška balta suknelė vasarai.', 0.8, 'Zara', 'https://unsplash.com/photos/d1UPkiFd04A', NULL),
('Beige Paltas', 129.99, 'Šiltas paltas rudens sezonui.', 1.5, 'H&M', 'https://unsplash.com/photos/POd02z5wzWs', NULL),
('Juodas Švarkas', 199.99, 'Klasikinis juodas švarkas ypatingoms progoms.', 1.2, 'Gucci', 'https://unsplash.com/photos/7WujkDJ8H-8', NULL),
('Raudonas Megztinis', 59.99, 'Patogus megztinis su rudens motyvais.', 0.9, 'Louis Vuitton', 'https://unsplash.com/photos/d1UPkiFd04A', NULL),
('Pilkos Kelnės', 49.99, 'Modernios ir stilingos kelnės darbui.', 0.6, 'Zara', 'https://unsplash.com/photos/POd02z5wzWs', NULL),
('Šilkinė Suknelė', 149.99, 'Prabangi suknelė ypatingiems vakarams.', 0.7, 'Gucci', 'https://unsplash.com/photos/7WujkDJ8H-8', NULL),
('Žieminė Striukė', 249.99, 'Neperšlampama žieminė striukė.', 2.0, 'Louis Vuitton', 'https://unsplash.com/photos/POd02z5wzWs', NULL),
('Sportiniai Marškinėliai', 39.99, 'Sportui tinkantys marškinėliai su logotipu.', 0.3, 'H&M', 'https://unsplash.com/photos/d1UPkiFd04A', NULL),
('Džinsai', 79.99, 'Aukštos kokybės džinsai su klasikiniu kirpimu.', 0.8, 'Zara', 'https://unsplash.com/photos/7WujkDJ8H-8', NULL),
('Šalikėlis', 19.99, 'Minkštas šalikėlis su raštais.', 0.2, 'Gucci', 'https://unsplash.com/photos/POd02z5wzWs', NULL);
