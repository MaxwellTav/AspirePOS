Use AspirePosDB;

CREATE TABLE PaymentDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL, -- Usuario que realizó el pago
    PaymentMethod NVARCHAR(50) NOT NULL, -- Tarjeta, PayPal, etc.
    TransactionId NVARCHAR(255) NOT NULL UNIQUE, -- ID de la transacción
    AmountPaid DECIMAL(10,2) NOT NULL, -- Monto pagado
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha de pago
    Status NVARCHAR(50) NOT NULL DEFAULT 'Completed', -- Estado (Completed, Failed, Pending)
    CONSTRAINT FK_Payment_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

INSERT INTO PaymentDetails (UserId, PaymentMethod, TransactionId, AmountPaid, PaymentDate, Status)
VALUES 
    ('6b9ccf8e-2463-451c-ac11-53711a17997b', 'PayPal', 'TXN-001', 11.11, GETDATE(), 'Completed');


CREATE TABLE SubscriptionPlans (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlanName NVARCHAR(100) NOT NULL, -- Nombre del plan
    Price DECIMAL(10,2) NOT NULL, -- Precio del plan
    DurationMonths INT NOT NULL, -- Duración en meses
    Features NVARCHAR(MAX) NULL -- Características del plan
);

INSERT INTO SubscriptionPlans (PlanName, Price, DurationMonths, Features)
VALUES 
    ('Basic Plan', 11.11, 1, 'Access to basic features'),
    ('Pro Plan', 22.22, 3, 'Access to all features, priority support'),
    ('Enterprise Plan', 33.33, 12, 'Unlimited access, dedicated support, advanced analytics');

CREATE TABLE DBSources (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador único
    Name NVARCHAR(100) NOT NULL UNIQUE, -- Nombre de la fuente (Ej: SQL Server, WooCommerce, etc.)
    Description NVARCHAR(255) NULL -- Descripción opcional
);

Insert Into DBSources
(Name, Description) Values('WooComerce', 'Almacenamiento nativo de WooComerce, perfecto para una configuración rápida y sencilla.');
Insert Into DBSources
(Name, Description) Values('Sql Server', 'Almacenamiento local, para más privacidad y mejor control de sus datos.');
Select * From DBSources;

CREATE TABLE UserDBSources (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador único
    UserId NVARCHAR(450) NOT NULL, -- Usuario al que aplica la disponibilidad
    DBSourceId INT NOT NULL, -- Fuente de datos asociada
    IsAvailable BIT NOT NULL DEFAULT 1, -- Indica si la fuente está disponible para este usuario
    CONSTRAINT FK_UserDBSources_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
    CONSTRAINT FK_UserDBSources_DBSource FOREIGN KEY (DBSourceId) REFERENCES DBSources(Id),
    UNIQUE (UserId, DBSourceId) -- Evita duplicados en la relación
);

Insert Into UserDBSources
Values
('6b9ccf8e-2463-451c-ac11-53711a17997b', 2, 0);

CREATE TABLE HostCredentials (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador único
    UserId NVARCHAR(450) NOT NULL, -- Usuario dueño de la clave
	
    ClientKey NVARCHAR(255) NOT NULL, -- Clave del cliente
    ClientSecret NVARCHAR(255) NOT NULL, -- Clave secreta del cliente
    ApiUrl NVARCHAR(500) NOT NULL, -- URL del host
    TokenEndpoint NVARCHAR(500) NULL, -- URL del endpoint para autenticación (Nuevo)
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha de creación del registro
    UpdatedAt DATETIME NULL, -- Fecha de última actualización
    CONSTRAINT FK_HostCredentials_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

INSERT INTO HostCredentials (UserId, ClientKey, ClientSecret, ApiUrl, TokenEndpoint, CreatedAt)
VALUES 
    ('6b9ccf8e-2463-451c-ac11-53711a17997b', 
	'ck_c4d1d2b150882f2ab139922dd0d50c294e063721', 
    'cs_3039c5c62cc03f3a872ba1389f1a757c551a6602', 
    'https://yellow-whale-846955.hostingersite.com/wp-json/',
	'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL3llbGxvdy13aGFsZS04NDY5NTUuaG9zdGluZ2Vyc2l0ZS5jb20iLCJpYXQiOjE3NDA3MTUxNzMsIm5iZiI6MTc0MDcxNTE3MywiZXhwIjoxNzQxMzE5OTczLCJkYXRhIjp7InVzZXIiOnsiaWQiOiIxIn19fQ.Vjcd067WcWNIQA8bqzuXR8ox-pfxaA1Sw19OVO0UXlo',
    GETDATE());

	Select * From HostCredentials

CREATE TABLE AuditLogs (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador único
    UserId NVARCHAR(450) NULL, -- Usuario que realizó la acción
    Action NVARCHAR(255) NOT NULL, -- Tipo de acción (Insert, Update, Delete, Login, etc.)
    TableName NVARCHAR(255) NOT NULL, -- Tabla afectada
    RecordId INT NULL, -- ID del registro afectado
    OldValues NVARCHAR(MAX) NULL, -- Valores anteriores
    NewValues NVARCHAR(MAX) NULL, -- Valores nuevos
    Timestamp DATETIME NOT NULL DEFAULT GETDATE() -- Fecha y hora del evento
);

CREATE TABLE ProgramRegisterTable (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador único
    UserId NVARCHAR(450) NOT NULL, -- Usuario que compró el programa
    ProgramName NVARCHAR(255) NOT NULL, -- Nombre del programa adquirido
    PurchaseDate DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha de compra
    PlanId INT NOT NULL, -- Tipo de plan
    PaymentId INT NOT NULL, -- ID del pago realizado
    ExpirationDate DATETIME NULL, -- Fecha de expiración del plan
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active', -- Estado (Active, Expired, Canceled)
    Notes NVARCHAR(MAX) NULL, -- Notas adicionales
    CONSTRAINT FK_ProgramRegister_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
    CONSTRAINT FK_ProgramRegister_Plan FOREIGN KEY (PlanId) REFERENCES SubscriptionPlans(Id),
    CONSTRAINT FK_ProgramRegister_Payment FOREIGN KEY (PaymentId) REFERENCES PaymentDetails(Id)
);

INSERT INTO ProgramRegisterTable (UserId, ProgramName, PurchaseDate, PlanId, PaymentId, ExpirationDate, Status, Notes)
VALUES ('6b9ccf8e-2463-451c-ac11-53711a17997b', 'Accounting Software', GETDATE(), 1, 1, DATEADD(MONTH, 1, GETDATE()), 'Active', 'Single-user license');
