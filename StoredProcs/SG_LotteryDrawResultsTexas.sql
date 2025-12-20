/* SOURCE: jasmine.arvixe.com  (DB: LuxDiablo)
   OUTPUT: ONE big INSERT script for dbo.LotteryDrawResultsTexas (preserves ResultID)
*/
SET NOCOUNT ON;
USE LuxDiablo;
GO

DECLARE @values nvarchar(max);

-- Build VALUES (...),(...),... as NVARCHAR(MAX)
SELECT
  @values =
    STRING_AGG(
      CAST(
        N'(' +
        /* ResultID (identity int) */        COALESCE(CAST([ResultID] AS nvarchar(20)), N'NULL') + N',' +
        /* UserID (int) */                   COALESCE(CAST([UserID] AS nvarchar(20)), N'NULL') + N',' +
        /* UserAlias (nvarchar) */           COALESCE(N'N''' + REPLACE([UserAlias], N'''', N'''''''') + N'''', N'NULL') + N',' +
        /* CardName (nvarchar) */            COALESCE(N'N''' + REPLACE([CardName], N'''', N'''''''') + N'''', N'NULL') + N',' +
        /* GameName (nvarchar) */            COALESCE(N'N''' + REPLACE([GameName], N'''', N'''''''') + N'''', N'NULL') + N',' +
        /* CardSlot (int) */                 COALESCE(CAST([CardSlot] AS nvarchar(20)), N'NULL') + N',' +
        /* WID (int) */                      COALESCE(CAST([WID] AS nvarchar(20)), N'NULL') + N',' +
        /* DrawDate (date) */                COALESCE(N'''' + CONVERT(varchar(10), [DrawDate], 23) + N'''', N'NULL') + N',' +
        /* Num1..Num6 (tinyint) */           COALESCE(CAST([Num1] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([Num2] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([Num3] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([Num4] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([Num5] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([Num6] AS nvarchar(3)), N'NULL') + N',' +
        /* UNum1..UNum6 (tinyint) */         COALESCE(CAST([UNum1] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([UNum2] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([UNum3] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([UNum4] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([UNum5] AS nvarchar(3)), N'NULL') + N',' +
                                            COALESCE(CAST([UNum6] AS nvarchar(3)), N'NULL') + N',' +
        /* Winner (bit) */                   COALESCE(CAST([Winner] AS nvarchar(1)), N'NULL') + N',' +
        /* RequestMatch (tinyint) */         COALESCE(CAST([RequestMatch] AS nvarchar(3)), N'NULL') + N',' +
        /* ActualMatch (tinyint) */          COALESCE(CAST([ActualMatch] AS nvarchar(3)), N'NULL') + N',' +
        /* CreatedAt (datetime2(7)) */       COALESCE(N'''' + CONVERT(varchar(33), [CreatedAt], 126) + N'''', N'NULL') + N',' +
        /* EMailSent (datetime2(7)) */       COALESCE(N'''' + CONVERT(varchar(33), [EMailSent], 126) + N'''', N'NULL') +
        N')'
        AS nvarchar(max)
      ),
      N','  -- separator
    ) WITHIN GROUP (ORDER BY [ResultID])
FROM dbo.LotteryDrawResultsTexas;

-- Safety: empty table
IF @values IS NULL OR LEN(@values) = 0
BEGIN
  SELECT N'-- LotteryDrawResultsTexas has no rows to export.' AS [-- Script];
  RETURN;
END

-- Wrap into one transactional script with IDENTITY_INSERT
DECLARE @sql nvarchar(max) =
N'SET XACT_ABORT ON;
BEGIN TRAN;

/* Ensure related Users exist on destination if required by your app logic */
SET IDENTITY_INSERT dbo.LotteryDrawResultsTexas ON;

INSERT INTO dbo.LotteryDrawResultsTexas
([ResultID],[UserID],[UserAlias],[CardName],[GameName],[CardSlot],[WID],[DrawDate],
 [Num1],[Num2],[Num3],[Num4],[Num5],[Num6],
 [UNum1],[UNum2],[UNum3],[UNum4],[UNum5],[UNum6],
 [Winner],[RequestMatch],[ActualMatch],
 [CreatedAt],[EMailSent])
VALUES
' + @values + N';

SET IDENTITY_INSERT dbo.LotteryDrawResultsTexas OFF;

COMMIT;';

-- Emit one copy/paste block
SELECT @sql AS [-- Script];
GO
