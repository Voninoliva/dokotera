create database dokotera;
\c dokotera;
create table parametres(
    idparametre serial primary key,
    nom varchar(100)
);
create table maladies(
    idmaladie serial primary key,
    nom varchar(100)
);
create table patients(
    idpatient serial primary key,
    nom varchar(200),
    age float
);
create table details_maladie(
    iddetails_maladie serial primary key,
    agedebut float,
    agefin float
);
create table parametre_des_maladies(
    idpdm serial primary key,
    idmaladie int references maladies(idmaladie),
    iddetails_maladie int references details_maladie(iddetails_maladie),
    idparametre int references parametres(idparametre),
    idebut float,
    ifin float
);

create or replace view detailmaladie as(
    select d.*,p.idebut,p.ifin,p.idparametre,p.idmaladie
    from details_maladie d 
    join parametre_des_maladies p on d.iddetails_maladie=p.iddetails_maladie
);

create table medicaments(
    idmedicament serial primary key,
    nom varchar(200),
    prix float,
    iddetails_maladie int references details_maladie(iddetails_maladie)
);
create table parametre_des_medicaments(
    idpmedicaments serial primary key,
    idmedicament int references medicaments(idmedicament),
    idparametre int references parametres(idparametre),
    effet float
);
create table donnees_des_patients(
    idddp serial primary key,
    idpatient int references patients(idpatient),
    idparametre int references parametres(idparametre),
    date date default current_date,
    niveau float
);

create or replace view maladie_des_patients as(
select idpatient,idmaladie,count(idmaladie) as isa
from ( 
    select d.idmaladie,datas.idpatient
    from detailmaladie d 
    join donnees_des_patients datas on d.idparametre=datas.idparametre
    join patients p on datas.idpatient=p.idpatient
    where datas.niveau<=d.ifin and datas.niveau>=d.idebut 
    and p.age<=agefin and p.age>=agedebut
) as aretina 
group by idpatient,idmaladie
order by  isa desc);

create or replace view medicaments_des_patients as(
    select p.idmedicament,count(p.idparametre) as nb_communes,m.prix,d.idpatient
    from  parametre_des_medicaments p
    join donnees_des_patients d on p.idparametre=d.idparametre
    join medicaments m on p.idmedicament=m.idmedicament
    where p.effet>0
    group by d.idpatient,p.idmedicament,m.prix
    order by nb_communes desc,m.prix asc
);

create or replace view dmedicaments as(
    select medicaments.*,agedebut,agefin 
    from medicaments 
    join details_maladie on medicaments.iddetails_maladie=details_maladie.iddetails_maladie
);

create or replace view mp  as(
    select d.* ,patients.idpatient
    from  dmedicaments  d
    join medicaments_des_patients  m on d.idmedicament=m.idmedicament 
    join patients on m.idpatient=patients.idpatient
    where d.agefin>=patients.age and d.agedebut<=patients.age
);


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

WITH maladies_communes AS (
    SELECT
        p1.idmaladie,
        p2.idmedicament,
        COUNT(p1.idparametre) AS nb_communes
    FROM
        parametre_des_maladies p1
    JOIN
        parametre_des_medicaments p2 ON p1.idparametre = p2.idparametre
    GROUP BY
        p1.idmaladie, p2.idmedicament
),
rang_maladies AS (
    SELECT
        idmaladie,
        idmedicament,
        ROW_NUMBER() OVER (PARTITION BY idmaladie ORDER BY nb_communes DESC) AS rang
    FROM
        maladies_communes
)
SELECT
    r.idmaladie,
    r.idmedicament
FROM
    rang_maladies r
WHERE
    r.rang = 1;
