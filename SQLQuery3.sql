CREATE VIEW TestCarb AS
SELECT
    D.ID AS Direction,
	TP.PrixCarb as PrixCarburant,
	AVG(T.QteCarb) as QteCarbMoyen,
    YEAR(TP.DatePaiement) AS Annee,
    MONTH(TP.DatePaiement) AS Mois,
    COUNT(DISTINCT C.ID) AS NombreCollaborateurs,
    SUM(TP.MontantTotalPaye) AS SommeMontantTotalPaye
FROM
    [data].[dbo].[TicketDACarbPayes] TP
JOIN
    [data].[dbo].[TicketDACarbs] T
    ON TP.TicketDACarbID = T.ID
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
    MONTH(TP.DatePaiement),
	TP.PrixCarb
GO