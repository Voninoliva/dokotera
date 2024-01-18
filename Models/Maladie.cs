using System.Data.Common;
using dokotera.Models.connection;

namespace dokotera.Models
{
    public class Maladie
    {
        public int idmaladie { get; set; }
        public string nom { get; set; }
        public List<DetailMaladie> details { get; set; }
        public Maladie() { }

        public Maladie(int idmaladie, string nom)
        {
            this.idmaladie = idmaladie;
            this.nom = nom;
            this.details=DetailMaladie.GetByIdMaladie(this.idmaladie);
        }
        public static List<Maladie> Get(string request){
            try{
                List<Maladie> liste=new List<Maladie>();
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read()){
                                int id=reader.GetInt32(reader.GetOrdinal("idmaladie"));
                                string nom=reader.GetString(reader.GetOrdinal("nom"));
                                liste.Add(new Maladie(id,nom));
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
        public static List<Maladie> GetAll(){
            try{
                string request="select * from maladies";
                return Maladie.Get(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Maladie GetById(int id){
            try{
                string request="select * from maladies where id="+id;
                return Maladie.Get(request)[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
    }
}