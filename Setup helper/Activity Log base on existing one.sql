----------------------------------------------------------------------
----------------------------------------------------------------------
Add one NuGet package (if you don’t have it already)

Install-Package FastMember
Install-Package Microsoft.AspNet.Identity.Core
Install-Package PagedList
Install-Package PagedList.Mvc
Install-Package OWIN
Install-Package Owin.Security.Cookies;
Install-Package Microsoft.AspNet.Identity.Owin
Microsoft.Owin.Host.SystemWeb



----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 1 – add the new columns we need
----------------------------------------------------------------------
----------------------------------------------------------------------
USE [project_management];
GO

ALTER TABLE dbo.ActivityLog
ADD
    TenantId      varchar(200) NULL,   -- company id (you already have Companies.id)
    UserId        varchar(200) NULL,   -- useraccount.id
    DepartmentId  varchar(200) NULL,   -- optional, leave NULL for now
    Action        varchar(50)   NULL,  -- Created / Updated / Deleted / Viewed / LoggedIn / LoggedOut …
    EntityType    varchar(100)  NULL,  -- table or class name
    EntityId      varchar(200)  NULL,  -- primary key of the row that was touched
    OldValues     nvarchar(max) NULL,  -- JSON before
    NewValues     nvarchar(max) NULL,  -- JSON after
    LoggedAtUtc   datetime2(2)  NOT NULL DEFAULT (sysutcdatetime());
GO
----------------------------------------------------------------------
----------------------------------------------------------------------

----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 2 – create the partition function & scheme (monthly slices)
----------------------------------------------------------------------
----------------------------------------------------------------------
-- 1-time: monthly boundaries for 10 years (2024-2034)
DECLARE @sql nvarchar(max)=N'';
;WITH n AS (SELECT 0 i UNION ALL SELECT i+1 FROM n WHERE i<120)
SELECT @sql+='
ALTER PARTITION FUNCTION PF_ActivityLog() SPLIT RANGE ('''+
CONVERT(char(7),DATEADD(MONTH,i,'2024-01-01'),126)+'-01'');'
FROM n;
EXEC (@sql);

-- create function + scheme (empty at start)
CREATE PARTITION FUNCTION PF_ActivityLog (datetime2(2))
AS RANGE RIGHT FOR VALUES ('2024-01-01');

CREATE PARTITION SCHEME PS_ActivityLog
AS PARTITION PF_ActivityLog ALL TO ([PRIMARY]);
GO

----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 3 – re-create the clustered index on the partition scheme
----------------------------------------------------------------------
----------------------------------------------------------------------
-- drop old PK (you used varchar(200) id as PK)
ALTER TABLE dbo.ActivityLog DROP CONSTRAINT [PK_ActivityLog];
GO

-- new composite PK that includes partition column
ALTER TABLE dbo.ActivityLog
ADD CONSTRAINT PK_ActivityLog
PRIMARY KEY CLUSTERED (TenantId, LoggedAtUtc, id)
ON PS_ActivityLog(LoggedAtUtc);
GO

----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 4 – helper indexes for lightning queries
----------------------------------------------------------------------
----------------------------------------------------------------------
-- 1.  Admin “show me everything for company X this month”
CREATE NONCLUSTERED INDEX IX_ActivityLog_TenantId_Month
ON dbo.ActivityLog (TenantId, LoggedAtUtc)
INCLUDE (UserId, Action, EntityType, EntityId);

-- 2.  User “show me my own history”
CREATE NONCLUSTERED INDEX IX_ActivityLog_UserId_LoggedAtUtc
ON dbo.ActivityLog (UserId, LoggedAtUtc)
INCLUDE (Action, EntityType, EntityId, Url);

-- 3.  Drill-down “who touched this project?”
CREATE NONCLUSTERED INDEX IX_ActivityLog_Entity
ON dbo.ActivityLog (TenantId, EntityType, EntityId, LoggedAtUtc);
GO

----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 5 – automatic monthly partition creation (SQL-Agent job)
Create one SQL-Agent job that runs the first day of every month:
----------------------------------------------------------------------
----------------------------------------------------------------------
DECLARE @nextMonth char(7) = FORMAT(DATEADD(MONTH,1,GETUTCDATE()),'yyyy-MM');
DECLARE @boundary datetime2 = CAST(@nextMonth+'-01' AS datetime2);

DECLARE @sql nvarchar(400)=
N'ALTER PARTITION FUNCTION PF_ActivityLog() SPLIT RANGE ('''+
CONVERT(char(10),@boundary,126)+''');';

EXEC (@sql);

----------------------------------------------------------------------
----------------------------------------------------------------------
STEP 6 – (optional) row-level security so each company sees only its rows
----------------------------------------------------------------------
----------------------------------------------------------------------
-- predicate function
CREATE FUNCTION dbo.fn_tenant_filter(@TenantId varchar(200))
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN SELECT 1 AS fn_securitypredicate
WHERE @TenantId = CAST(SESSION_CONTEXT(N'TenantId') AS varchar(200));
GO

-- bind it to the table
CREATE SECURITY POLICY dbo.tenant_policy
ADD FILTER PREDICATE dbo.fn_tenant_filter(TenantId)
ON dbo.ActivityLog
WITH (STATE = ON);
GO

----------------------------------------------------------------------
----------------------------------------------------------------------
In your C# code set the tenant once per connection:
csharp
Copy
cmd.CommandText = "EXEC sp_set_session_context @key=N'TenantId', @value=@tid;";
cmd.Parameters.AddWithValue("@tid", currentUser.CompanyId);
You are now on STEP 1 done – the table is partitioned monthly, indexed, and ready for the MVC filter + bulk-insert code that follows in the next steps

----------------------------------------------------------------------
----------------------------------------------------------------------