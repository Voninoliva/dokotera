using System.Data.Common;
using dokotera.Models.connection;

namespace dokotera.Models
{
    public class Medicament
    {
        public int idmedicament { get; set; }
        public string nom { get; set; }
        public double prix { get;set; }
        public List<EffetDesParametres> effets{get;set;}

        public Medicament(int idmedicament, string nom, double prix)
        {
            this.idmedicament = idmedicament;
            this.nom = nom;
            this.prix = prix;
            this.effets=EffetDesParametres.GetByIdMedicament(this.idmedicament);
        }
        public double GetDosePerscrit(int idparametre,double niveau){
            double reponse=0;
            for(int i=0;i<this.effets.Count;i++){
                if(this.effets[i].parametre.idparametre==idparametre){
                    double e=niveau/this.effets[i].effet;
                    int reponseFloat=(int)e;
                    if(reponseFloat!=e){
                        reponse=reponseFloat+1;
                    }
                    else{
                        reponse=reponseFloat;
                    }
                }
            }
            return reponse;

        }
        public double GetEffetForPArametre(int idparametre){
            double reponse=0;
            for(int i=0;i<this.effets.Count;i++){
                if(this.effets[i].parametre.idparametre==idparametre){
                    reponse=this.effets[i].effet;
                }
            }
            return reponse;
        }
        public static List<Medicament> Get(string request){
            try{
                List<Medicament> liste=new List<Medicament>();
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read()){
                                int id=reader.GetInt32(reader.GetOrdinal("idmedicament"));
                                string nom=reader.GetString(reader.GetOrdinal("nom"));
                                double prix=reader.GetDouble(reader.GetOrdinal("prix"));
                                liste.Add(new Medicament(id,nom,prix));
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
        public static List<Medicament> GetAll(){
            try{
                string request="select * from medicaments";
                return Medicament.Get(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
