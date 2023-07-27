USE [data]
GO

/****** Object:  View [dbo].[test]    Script Date: 27/07/2023 09:38:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[test] AS
SELECT
    D.ID AS Direction,
    YEAR(TP.DatePaiement) AS Annee,
    MONTH(TP.DatePaiement) AS Mois,
    COUNT(DISTINCT C.ID) AS NombreCollaborateurs,
    SUM(TP.MontantTotalPaye) AS SommeMontantTotalPaye
FROM
    [data].[dbo].[TicketPDCPayes] TP
JOIN
    [data].[dbo].[TicketPDCs] T
    ON TP.TicketPDCID = T.ID
JOIN
    [data].[dbo].[Collaborateurs] C
    ON T.DemandeurID = C.ID
JOIN
    [data].[dbo].[Fonctions] F
    ON C.FonctionID = F.ID
JOIN
    [data].[dbo].[Services] S
    ON F.ServiceID = S.ID
JOIN
    [data].[dbo].[Directions] D
    ON S.DirectionID = D.ID
GROUP BY
    D.ID,
    YEAR(TP.DatePaiement),
    MONTH(TP.DatePaiement)
GO



--select top 250 CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs, CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee desc

select CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs,CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee DESC OFFSET 250 ROWS;

select*from test

select top 250 CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs,CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee desc
select CAST(Direction as REAL) as Direction,CAST(NombreCollaborateurs as REAL) as NombreCollaborateurs,CAST(Annee as REAL) as Annee,CAST(Mois as REAL) as Mois,CAST(SommeMontantTotalPaye as REAL) as SommeMontantTotalPaye from test ORDER BY Annee desc OFFSET 250 ROWS


select avg(SommeMontantTotalPaye) from test where direction = 2 and mois = 8;