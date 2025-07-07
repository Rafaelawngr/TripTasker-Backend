-- Cria tabela de Viagens
CREATE TABLE Trips (
    TripId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL
);

-- Cria tabela de Tarefas
CREATE TABLE Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Status INT NOT NULL,
    DueDate DATE NOT NULL,
    TripId INT NOT NULL,
    FOREIGN KEY (TripId) REFERENCES Trips(TripId)
);

-- Cria tabela de Usuários
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL
);

-- Cria tabela alternativa de Tarefas (TaskItems)
CREATE TABLE dbo.TaskItems (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Status INT NOT NULL,
    DueDate DATETIME NOT NULL,
    TripId INT NOT NULL,
    CONSTRAINT FK_TaskItems_Trips FOREIGN KEY (TripId) REFERENCES dbo.Trips(TripId)
);

-- Inserir Viagens
INSERT INTO Trips (Title) VALUES 
('Viagem para Salvador'),
('Viagem para Florianópolis'),
('Viagem para São Paulo');

-- Inserir Tarefas para a Viagem ID 1
INSERT INTO Tasks (Title, Description, Status, DueDate, TripId) VALUES
('Comprar passagens', 'Verificar preços online', 0, '2024-12-01', 1),
('Reservar hotel', 'Hotel próximo à praia', 0, '2024-12-05', 1),
('Montar roteiro', 'Roteiro de passeios', 0, '2024-11-20', 1);

-- Inserir Tarefas para a Viagem ID 2
INSERT INTO Tasks (Title, Description, Status, DueDate, TripId) VALUES
('Alugar carro', 'Para deslocamento na cidade', 0, '2024-12-10', 2),
('Comprar ingressos', 'Atrações turísticas', 0, '2024-12-15', 2);

-- Copia os dados de Tasks para TaskItems
INSERT INTO dbo.TaskItems (Title, Description, Status, DueDate, TripId)
SELECT Title, Description, Status, DueDate, TripId
FROM dbo.Tasks;

-- Inserir tarefas adicionais diretamente em TaskItems
INSERT INTO dbo.TaskItems (Title, Description, Status, DueDate, TripId)
VALUES 
('Comprar passagens', 'Procurar melhores preços', 0, '2025-06-10', 2),
('Reservar hotel', 'Verificar opções no centro', 1, '2025-06-11', 2),
('Fazer roteiro', 'Definir pontos turísticos', 2, '2025-06-12', 2);
