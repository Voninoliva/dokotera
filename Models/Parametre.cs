using System.Data.Common;
using dokotera.Models.connection;

namespace dokotera.Models
{
    public class Parametre
    {
        public int idparametre { get; set; }
        public string nom { get; set; }
        public double niveauVraie{get;set;}
        public Parametre(int idparametre, string nom)
        {
            this.idparametre = idparametre;
            this.nom = nom;
            this.niveauVraie=0;
        }
        public static List<Parametre> Get(string request){
            try{
                List<Parametre> parametres=new List<Parametre>();
                using(DbConnection? co = Connect.GetConnection()){
                    using(DbCommand cmd=co.CreateCommand()){
                        cmd.CommandText=request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read()){
                                int id = reader.GetInt32(reader.GetOrdinal("idparametre"));
                                string nom=reader.GetString(reader.GetOrdinal("nom"));
                                parametres.Add(new Parametre(id,nom));
                            }
                        }
                    }
                }
                return parametres;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Parametre> GetAll(){
            try{
                string request="select * from parametres";
                return Parametre.Get(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Parametre GetById(int id){
            try{
                string request="select * from parametres where idparametre="+id;
                return Parametre.Get(request)[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}