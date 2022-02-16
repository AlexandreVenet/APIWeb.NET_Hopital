-- Quelques instructions de test

-- ------------
-- INSERTIONS
-- ------------

-- INSERT INTO dossiers (date_entree, date_sortie, motif , id_utilisateur) 
-- VALUES('2000-01-01', '2020-01-01', 'Pédagogie', 3);

-- INSERT INTO dossiers (date_entree, id_utilisateur) 
-- VALUES('2022-01-01 09:15:00', 3); -- certaines valeurs sont optionnelles

-- INSERT INTO dossiers (id_utilisateur) VALUES(3);

-- ------------
-- SELECTIONS
-- ------------

-- SELECT dossiers.id, dossiers.date_entree, dossiers.date_sortie, dossiers.motif, utilisateurs.nom, roles.role FROM dossiers
-- INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id
-- INNER JOIN roles ON utilisateurs.id_role = roles.id
-- WHERE roles.role = 'Patient';

-- SELECT dossiers.id, dossiers.date_entree, dossiers.date_sortie, dossiers.motif, utilisateurs.nom, roles.role 
-- FROM dossiers
-- INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id
-- INNER JOIN roles ON utilisateurs.id_role = roles.id;

-- SELECT dossiers.id, dossiers.date_entree, dossiers.date_sortie, dossiers.motif, utilisateurs.nom, roles.role FROM dossiers
-- INNER JOIN utilisateurs ON dossiers.id_utilisateur = utilisateurs.id
-- INNER JOIN roles ON utilisateurs.id_role = roles.id
-- WHERE utilisateurs.nom = 'Toto';

-- ------------
-- MDOFICATIONS
-- ------------

-- UPDATE dossiers SET date_entree = null, date_sortie = null, motif = null WHERE id = 3 RETURNING TRUE;

SELECT * FROM dossiers;

