CREATE TABLE Application (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name varchar(255) NOT NULL UNIQUE,
    CreatedDate DateTime,
);

INSERT INTO Application VALUES ('App1', '10-10-2020')
INSERT INTO Application VALUES ('App2', '10-10-2020')

ALTER TABLE Application ALTER COLUMN CreatedDate DateTime

DROP TABLE Application

SELECT * FROM Application