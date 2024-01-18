insert into parametres (nom) values 
('maux de tete'),
('temperature'),
('les yeux'),
('essouflement'),
('ventre');

insert into maladies(nom) values
('fievre'),
('diarrhee'),
('covid');




insert into details_maladie (agedebut,agefin) values
(0,12),
(13,60),
(61,280);

-- Insérer 20 lignes de données dans la table parametre_des_maladies avec des valeurs d'intervalle variantes de 0 à 10
INSERT INTO parametre_des_maladies (idmaladie, iddetails_maladie, idparametre, idebut, ifin)
VALUES
    -- Maladie 1
    (1, 1, 1, 1.5, 4.7),
    (1, 1, 2, 0.8, 3.2),
    (1, 1, 3, 2.1, 5.5),
    (1, 1, 4, 0.5, 3.9),
    (1, 1, 5, 1.2, 4.8),
    
    (1, 2, 1, 0.7, 6.2),
    (1, 2, 2, 1.0, 4.3),
    (1, 2, 3, 1.7, 4.5),
    (1, 2, 4, 0.2, 3.1),
    (1, 2, 5, 1.6, 4.2),

    (1, 3, 1, 2.3, 4.0),
    (1, 3, 2, 0.9, 6.0),
    (1, 3, 3, 2.5, 5.0),
    (1, 3, 4, 0.4, 5.7),
    (1, 3, 5, 1.3, 4.7),
    
    -- Maladie 2
    (2, 1, 1, 0.8, 3.6),
    (2, 1, 2, 0.5, 3.1),
    (2, 1, 3, 1.2, 5.8),
    (2, 1, 4, 0.6, 3.9),
    (2, 1, 5, 2.0, 3.8),

    (2, 2, 1, 1.0, 4.3),
    (2, 2, 2, 1.8, 4.9),
    (2, 2, 3, 1.3, 4.7),
    (2, 2, 4, 0.9, 6.5),
    (2, 2, 5, 0.4, 5.0);

insert into medicaments (nom,prix,iddetails_maladie) values 
('paracetamol',200,2),
('grt',1000,2),
('efferalgant',500,2);

insert into medicaments (nom,prix,iddetails_maladie) values ('para4',2500,2);
INSERT INTO parametre_des_medicaments (idmedicament, idparametre, effet) VALUES
    (1, 1, 1), -- paracetamol - maux de tete
    (1, 2, 2), -- paracetamol - temperature
    (2, 3, 3), -- grt - les yeux
    (2, 2, -1), -- grt - les yeux
    (3, 5, 5); -- efferalgant - ventre

INSERT INTO parametre_des_medicaments (idmedicament, idparametre, effet) VALUES(4,4,3);

