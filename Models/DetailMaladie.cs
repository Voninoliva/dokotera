using dokotera.Models.connection;
using System.Data.Common;

namespace dokotera.Models
{
    public class DetailMaladie
    {
        public int iddetails_maladie{get;set;}
        public double agedebut{get;set;}
        public double agefin{get;set;}
        public Parametre parametre{get;set;}
        public double iDebut{get;set;}
        public double iFin{get;set;}

        public DetailMaladie(int iddetails_maladie, double agedebut, double agefin, Parametre parametre, double iDebut, double iFin)
        {
            this.iddetails_maladie = iddetails_maladie;
            this.agedebut = agedebut;
            this.agefin = agefin;
            this.parametre = parametre;
            this.iDebut = iDebut;
            this.iFin = iFin;
        }
        // mbola mila amboarina
        public DetailMaladie(int iddetails_maladie, double agedebut, double agefin,int parametre, double iDebut, double iFin)
        {
            this.iddetails_maladie = iddetails_maladie;
            this.agedebut = agedebut;
            this.agefin = agefin;
            this.parametre = Parametre.GetById(parametre);
            this.iDebut = iDebut;
            this.iFin = iFin;
        }
        // le view detailmaladie no apesaina
        public static List<DetailMaladie> Get(string request){
            try{
                List<DetailMaladie> liste=new List<DetailMaladie>();
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read()){
                                int id=reader.GetInt32(reader.GetOrdinal("iddetails_maladie"));
                                double ad=reader.GetDouble(reader.GetOrdinal("agedebut"));
                                double af=reader.GetDouble(reader.GetOrdinal("agefin"));
                                int idp=reader.GetInt32(reader.GetOrdinal("idparametre"));
                                double idebut=reader.GetDouble(reader.GetOrdinal("idebut"));
                                double ifin=reader.GetDouble(reader.GetOrdinal("ifin"));
                                liste.Add(new DetailMaladie(id,ad,af,idp,idebut,ifin));
                            }
                        }
                    }
                }
                return liste;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<DetailMaladie> GetByIdMaladie(int idmaladie){
            try{
                string request="select * from detailmaladie where idmaladie="+idmaladie;
                return DetailMaladie.Get(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
