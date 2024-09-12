CREATE DATABASE IF NOT EXISTS MyAppDB;
USE MyAppDB;

-- Table: Users
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Type TINYINT NOT NULL, -- 0 for flat, 1 for admin
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(300) NOT NULL
);

-- Table: Requests
CREATE TABLE Requests (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    RequestStartDate DATETIME NOT NULL,
    EffectiveStartDate DATETIME,
    EffectiveEndDate DATETIME,
    Status TINYINT NOT NULL, -- 0: under study, 1: waiting for info, 2: pre-approved, 3: rejected, 4: cancelled, 5: approved
    StepInFlow TINYINT NOT NULL,
    AutoEstimatedIncome INT,
    AutoEstimatedPaymentToRecover INT,
    CalculatedIncome INT,
    FinalPaymentToRecover INT,
    SpotifyLink VARCHAR(300) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Table: Comments
CREATE TABLE Comments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Text VARCHAR(400) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Table: Contracts
CREATE TABLE Contracts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RequestId INT NOT NULL,
    Signed BOOLEAN NOT NULL,
    Path VARCHAR(300),
    FOREIGN KEY (RequestId) REFERENCES Requests(Id)
);

-- Table: Files
CREATE TABLE Files (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RequestId INT NOT NULL,
    Path VARCHAR(300),
    FOREIGN KEY (RequestId) REFERENCES Requests(Id)
);
