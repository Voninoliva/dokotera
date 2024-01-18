using System.Data.Common;
using dokotera.Models.connection;

namespace dokotera.Models
{
    public class EffetDesParametres
    {
        public Parametre parametre { get; set; }
        public double effet { get; set; }

        public EffetDesParametres(Parametre parametre, double effet)
        {
            this.parametre = parametre;
            this.effet = effet;
        }
        public static List<EffetDesParametres> Get(string request)
        {
            try
            {
                List<EffetDesParametres> traitements = new();
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Parametre p=Parametre.GetById(reader.GetInt32(reader.GetOrdinal("idparametre")));
                                double effet=reader.GetDouble(reader.GetOrdinal("effet"));
                                traitements.Add(new EffetDesParametres(p,effet));
                            }
                        }
                    }
                }
                return traitements;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<EffetDesParametres> GetByIdMedicament(int idm){
            try{
                string r="select * from parametre_des_medicaments where idmedicament="+idm;
                return EffetDesParametres.Get(r);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
