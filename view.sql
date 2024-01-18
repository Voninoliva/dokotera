select  idmedicament from (
SELECT
        p1.idmaladie,
        p2.idmedicament,
        COUNT(p1.idparametre) AS nb_communes
    FROM
        parametre_des_maladies p1
    JOIN
        parametre_des_medicaments p2 ON p1.idparametre = p2.idparametre
    GROUP BY
        p1.idmaladie, p2.idmedicament) as a  order by nb_communes desc


select d.idmaladie,datas.idpatient,
    from detailmaladie d 
    join donnees_des_patients datas on d.idparametre=datas.idparametre
    join patients p on datas.idpatient=p.idpatient
    where datas.niveau<=d.ifin and datas.niveau>=d.idebut 
    and p.age<=agefin and p.age>=agedebut




