/* SOURCE: jasmine.arvixe.com  (DB: LuxDiablo)
   OUTPUT: ONE big INSERT script for dbo.UserSubscriptions (preserves SubscriptionId, skips RowVersion)
   NEXT: Copy the single output block and run on ws3.win.arvixe.com (DB: lottojump)
*/
SET NOCOUNT ON;
USE LuxDiablo;
GO

DECLARE @values nvarchar(max);

-- Build VALUES (...),(...),... as NVARCHAR(MAX) to avoid 8K truncation
SELECT
  @values =
    STRING_AGG(
      CAST(
        N'(' +
        /* SubscriptionId (identity int) */   COALESCE(CAST([SubscriptionId] AS nvarchar(20)), N'NULL') + N',' +
        /* UserId (int, FK) */                COALESCE(CAST([UserId] AS nvarchar(20)), N'NULL') + N',' +
        /* UserAlias (varchar(8)) */          COALESCE(N'''' + REPLACE([UserAlias], '''', '''''') + N'''', N'NULL') + N',' +
        /* PlanCode (varchar(50)) */          COALESCE(N'''' + REPLACE([PlanCode], '''', '''''') + N'''', N'NULL') + N',' +
        /* Status (varchar(20)) */            COALESCE(N'''' + REPLACE([Status], '''', '''''') + N'''', N'NULL') + N',' +
        /* AutoRenew (bit) */                 COALESCE(CAST([AutoRenew] AS nvarchar(1)), N'NULL') + N',' +
        /* Quantity (int) */                  COALESCE(CAST([Quantity] AS nvarchar(20)), N'NULL') + N',' +
        /* StartUtc (datetime2(0)) */         COALESCE(N'''' + CONVERT(varchar(19), [StartUtc], 120) + N'''', N'NULL') + N',' +
        /* CurrentPeriodStartUtc (dt2(0)) */  COALESCE(N'''' + CONVERT(varchar(19), [CurrentPeriodStartUtc], 120) + N'''', N'NULL') + N',' +
        /* CurrentPeriodEndUtc (dt2(0)) */    COALESCE(N'''' + CONVERT(varchar(19), [CurrentPeriodEndUtc], 120) + N'''', N'NULL') + N',' +
        /* TrialEndUtc (dt2(0)) */            COALESCE(N'''' + CONVERT(varchar(19), [TrialEndUtc], 120) + N'''', N'NULL') + N',' +
        /* CanceledAtUtc (dt2(0)) */          COALESCE(N'''' + CONVERT(varchar(19), [CanceledAtUtc], 120) + N'''', N'NULL') + N',' +
        /* EndedAtUtc (dt2(0)) */             COALESCE(N'''' + CONVERT(varchar(19), [EndedAtUtc], 120) + N'''', N'NULL') + N',' +
        /* PriceAtPurchase (decimal(10,2)) */ COALESCE(CAST([PriceAtPurchase] AS nvarchar(50)), N'NULL') + N',' +
        /* Currency (char(3)) */              COALESCE(N'''' + REPLACE([Currency], '''', '''''') + N'''', N'NULL') + N',' +
        /* ExternalProvider (varchar(30)) */  COALESCE(N'''' + REPLACE([ExternalProvider], '''', '''''') + N'''', N'NULL') + N',' +
        /* ExternalSubscriptionId (varchar)*/ COALESCE(N'''' + REPLACE([ExternalSubscriptionId], '''', '''''') + N'''', N'NULL') + N',' +
        /* CreatedAt (datetime2(0)) */        COALESCE(N'''' + CONVERT(varchar(19), [CreatedAt], 120) + N'''', N'NULL') + N',' +
        /* UpdatedAt (datetime2(0)) */        COALESCE(N'''' + CONVERT(varchar(19), [UpdatedAt], 120) + N'''', N'NULL') +
        N')'
        AS nvarchar(max)
      ),
      N','  -- separator
    ) WITHIN GROUP (ORDER BY [SubscriptionId])
FROM dbo.UserSubscriptions;

-- Safety: empty table
IF @values IS NULL OR LEN(@values) = 0
BEGIN
  SELECT N'-- UserSubscriptions has no rows to export.' AS [-- Script];
  RETURN;
END

-- Wrap into a single transactional script with IDENTITY_INSERT
DECLARE @sql nvarchar(max) =
N'SET XACT_ABORT ON;
BEGIN TRAN;

/* Ensure dbo.Users is already loaded on destination (FK UserId) */
SET IDENTITY_INSERT dbo.UserSubscriptions ON;

INSERT INTO dbo.UserSubscriptions
([SubscriptionId],[UserId],[UserAlias],[PlanCode],[Status],[AutoRenew],[Quantity],
 [StartUtc],[CurrentPeriodStartUtc],[CurrentPeriodEndUtc],[TrialEndUtc],[CanceledAtUtc],[EndedAtUtc],
 [PriceAtPurchase],[Currency],[ExternalProvider],[ExternalSubscriptionId],[CreatedAt],[UpdatedAt])
VALUES
' + @values + N';

SET IDENTITY_INSERT dbo.UserSubscriptions OFF;

COMMIT;';

-- Emit one big block to copy/paste
SELECT @sql AS [-- Script];
GO
