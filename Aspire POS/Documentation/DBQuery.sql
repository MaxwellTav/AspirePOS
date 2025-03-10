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
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    ClientKey NVARCHAR(255) NOT NULL,
    ClientSecret NVARCHAR(255) NOT NULL,
    ApiUrl NVARCHAR(500) NOT NULL,
    TokenEndpoint NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    CONSTRAINT FK_HostCredentials_User FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);


INSERT INTO HostCredentials (UserId, ClientKey, ClientSecret, ApiUrl, TokenEndpoint, CreatedAt)
VALUES 
    ('Hacer un select a la tabla AspNetUsers y tomar el ID', 
	'ck_a1a2a3a4a5a6a7', 
    'cs_1a2a3a4a5a6a7a', 
    'https://mihost.hostingersite.com/wp-json/',
	'(Esto es un ejemplo) s1s2s2s3a1d1w5w1w5w7w7w7w',
    GETDATE());


CREATE TABLE AuditLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NULL,
    Action NVARCHAR(255) NOT NULL,
    TableName NVARCHAR(255) NOT NULL,
    RecordId INT NULL,
    OldValues NVARCHAR(MAX) NULL,
    NewValues NVARCHAR(MAX) NULL,
    Timestamp DATETIME NOT NULL DEFAULT GETDATE()
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

CREATE TABLE PathTable (
    Id INT PRIMARY KEY IDENTITY(1,1),
	AuthPathName VARCHAR(50) NOT NULL UNIQUE,
    AuthPath NVARCHAR(255) NOT NULL UNIQUE,
    Description NVARCHAR(500) NOT NULL,
    UpdatedAt DATETIME NULL DEFAULT GETDATE()
);

INSERT INTO PathTable (AuthPathName, AuthPath, Description, UpdatedAt) 
VALUES ('Users', '/wp-json/wp/v2/users', 'Obtiene todos los usuarios registrados en WordPress', GETDATE());
