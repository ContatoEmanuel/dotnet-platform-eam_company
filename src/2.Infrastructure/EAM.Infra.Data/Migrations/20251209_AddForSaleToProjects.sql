-- Migration: Add ForSale and Price columns to Projects table
-- Date: 2025-12-09

USE EAM_Database;
GO

-- Add new columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'ForSale')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [ForSale] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'Price')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [Price] DECIMAL(18,2) NULL;
END
GO

-- Update existing WhatsApp Extension project to ForSale = true
UPDATE [dbo].[Projects]
SET [ForSale] = 1,
    [Price] = 0.00  -- Free trial
WHERE [Title] LIKE '%WhatsApp%Extension%'
   OR [Title] LIKE '%react-extension-dynamics_whatsapp%';
GO

PRINT 'Migration completed: ForSale and Price columns added to Projects table';
GO
