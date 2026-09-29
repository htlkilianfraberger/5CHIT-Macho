DROP DATABASE IF EXISTS Transaktionslog;
CREATE DATABASE Transaktionslog;
USE Transaktionslog;

CREATE TABLE konto (
                       konto_id INT PRIMARY KEY,
                       kontostand DECIMAL(10,2) NOT NULL
);

CREATE TABLE transaktionslog (
                                 transaktions_id INT AUTO_INCREMENT PRIMARY KEY,
                                 zeitpunkt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                                 konto_id INT,
                                 benutzer VARCHAR(100),
                                 aktion VARCHAR(10),
                                 old_wert DECIMAL(10,2),
                                 new_wert DECIMAL(10,2)
);

DELIMITER //

CREATE TRIGGER konto_insert_log
    AFTER INSERT ON konto
    FOR EACH ROW
BEGIN
    INSERT INTO transaktionslog
    (konto_id, benutzer, aktion, old_wert, new_wert)
    VALUES
        (NEW.konto_id, USER(), 'INSERT', NULL, NEW.kontostand);
END //

CREATE TRIGGER konto_update_log
    AFTER UPDATE ON konto
    FOR EACH ROW
BEGIN
    INSERT INTO transaktionslog
    (konto_id, benutzer, aktion, old_wert, new_wert)
    VALUES
        (NEW.konto_id, USER(), 'UPDATE', OLD.kontostand, NEW.kontostand);
END //

CREATE TRIGGER konto_delete_log
    AFTER DELETE ON konto
    FOR EACH ROW
BEGIN
    INSERT INTO transaktionslog
    (konto_id, benutzer, aktion, old_wert, new_wert)
    VALUES
        (OLD.konto_id, USER(), 'DELETE', OLD.kontostand, NULL);
END //

DELIMITER ;

START TRANSACTION;

INSERT INTO konto (konto_id, kontostand)
VALUES
    (1, 100.00),
    (2, 50.00);

COMMIT;

START TRANSACTION;

UPDATE konto
SET kontostand = kontostand - 10
WHERE konto_id = 1;

UPDATE konto
SET kontostand = kontostand + 10
WHERE konto_id = 2;

COMMIT;

START TRANSACTION;

INSERT INTO konto (konto_id, kontostand)
VALUES (3, 200.00);

COMMIT;

START TRANSACTION;

DELETE FROM konto
WHERE konto_id = 3;

COMMIT;

SELECT * FROM konto;

SELECT * FROM transaktionslog;