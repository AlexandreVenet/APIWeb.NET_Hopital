-- -----------------------------------

-- création bdd Postgresql "Hopital"

CREATE DATABASE "Hopital"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'French_France.1252'
    LC_CTYPE = 'French_France.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- -----------------------------------

SET timezone = "+00:00";

-- -----------------------------------

-- instructions POSTGRESQL (et NON SQL)

DROP TABLE IF EXISTS roles;

CREATE TABLE roles(
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	role VARCHAR(255) NOT NULL
);


DROP TABLE IF EXISTS utilisateurs;

CREATE TABLE utilisateurs(
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	id_role BIGINT NOT NULL,
	nom VARCHAR(255) NOT NULL,
 	CONSTRAINT fk_roles FOREIGN KEY(id_role) REFERENCES roles(id)
);

DROP TABLE IF EXISTS dossiers;

CREATE TABLE dossiers(
	id BIGSERIAL NOT NULL UNIQUE PRIMARY KEY,
	date_entree TIMESTAMP,
	date_sortie TIMESTAMP,
	motif VARCHAR(255),
	id_utilisateur BIGINT NOT NULL,
	CONSTRAINT fk_utilisateur FOREIGN KEY(id_utilisateur) REFERENCES utilisateurs(id)
);

-- -----------------------------------

-- fin de fichier