CREATE DATABASE IF NOT EXISTS MyAppDB;
USE MyAppDB;

-- Table: Users
CREATE TABLE Users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    type TINYINT NOT NULL, -- 0 for flat, 1 for admin
    name VARCHAR(100) NOT NULL,
);

-- Table: Requests
CREATE TABLE Requests (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    request_start_date DATETIME NOT NULL,
    effective_start_date DATETIME,
    effective_end_date DATETIME,
    status TINYINT NOT NULL, -- 0: under study, 1: waiting for info, 2: pre-approved, 3: rejected, 4: cancelled, 5: approved
    step_in_flow TINYINT NOT NULL,
    auto_estimated_income INT,
    auto_estimated_payment_to_recover INT,
    calculated_income INT,
    final_payment_to_recover INT,
    spotify_link VARCHAR(300) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id)
);

-- Table: Comments
CREATE TABLE Comments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    text VARCHAR(400) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id)
);

-- Table: Contracts
CREATE TABLE Contracts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    request_id INT NOT NULL,
    signed BOOLEAN NOT NULL,
    path VARCHAR(300),
    FOREIGN KEY (request_id) REFERENCES Requests(id)
);

-- Table: Files
CREATE TABLE Files (
    id INT AUTO_INCREMENT PRIMARY KEY,
    request_id INT NOT NULL,
    path VARCHAR(300),
    FOREIGN KEY (request_id) REFERENCES Requests(id)
);
