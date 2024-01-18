using System.Globalization;
using System.Data.Common;
using dokotera.Models.connection;

namespace dokotera.Models
{
    public class Patient
    {
        public int idpatient { get; set; }
        public string nom { get; set; }
        public double age { get; set; }
        public List<Parametre> parametres { get; set; }
        public List<double> niveau { get; set; }
        public List<Ordonnance>  ordonnances{get;set;}
    
        public Patient(int idpatient, string nom, double age, List<Parametre> parametres, List<double> niveau)
        {
            this.idpatient = idpatient;
            this.nom = nom;
            this.age = age;
            this.parametres = parametres;
            this.niveau = niveau;
           this.ordonnances=new List<Ordonnance>();
        }
        public Patient(int idpatient, string nom, double age)
        {
            try
            {
                this.idpatient = idpatient;
                this.nom = nom;
                this.age = age;
                this.parametres = new List<Parametre>();
                this.niveau = new List<double>();
                 this.ordonnances=new List<Ordonnance>();
                string request = "select * from donnees_des_patients where idpatient=" + this.idpatient;
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Parametre p = Parametre.GetById(reader.GetInt32(reader.GetOrdinal("idparametre")));
                                double niveau = reader.GetDouble(reader.GetOrdinal("niveau"));
                                this.parametres.Add(p);
                                this.niveau.Add(niveau);
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static List<Patient> Get(string request)
        {
            try
            {
                List<Patient> traitements = new();
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = request;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(reader.GetOrdinal("idpatient"));
                                string nom = reader.GetString(reader.GetOrdinal("nom"));
                                double age=reader.GetDouble(reader.GetOrdinal("age"));
                                traitements.Add(new Patient(id, nom,age));
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
        public static List<Medicament> GetHisM(int idpatient){
            try{
                string r="select * from mp where idpatient="+idpatient;
                return Medicament.Get(r);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool Sitrana(){
            int i=0;
            for(int j=0;j<this.niveau.Count;j++){
                if(this.niveau[j]==0){
                    i++;
                }
            }
            if(i==this.niveau.Count){
                return true;
            }
            return false;
        }
        public double GetPrixTotal(){
            double p=0;
            for(int i=0;i<this.ordonnances.Count;i++){
                p+=this.ordonnances[i].prix;
            }
            return p;
        }
        public List<Ordonnance> GetMedic(){
            try{
                List<Parametre> marary=new List<Parametre>();
                List<int> hafa=new List<int>();
                Patient patient=Patient.Get("select * from patients where idpatient="+this.idpatient)[0];
                List<Medicament> medicaments=Patient.GetHisM(idpatient);
                for(int i=0;i<medicaments.Count;i++){
                    if(patient.Sitrana()==true){
                        break;
                    }
                    Medicament medoc=medicaments[i];
                    double doseAminIzao=medoc.GetDosePerscrit(patient.parametres[0].idparametre,patient.niveau[0]);
                    // mitady an le tena dose
                    for(int j=0;j<patient.parametres.Count;j++){
                        double dose_eto=medoc.GetDosePerscrit(patient.parametres[j].idparametre,patient.niveau[j]);
                        if(dose_eto>doseAminIzao){
                            doseAminIzao=dose_eto;
                        }     
                    }
                    marary=patient.parametres;
                    // manova an le niveau an le parametre
                    for(int j=0;j<marary.Count;j++){
                        // misy effet le medicament sady mbola tsy traitee
                        if(medoc.GetEffetForPArametre((marary[j].idparametre))!=0 && hafa.Contains(j)==false){ 
                             Console.WriteLine();  
                            double n = patient.niveau[j]-(medoc.GetEffetForPArametre((marary[j].idparametre))*doseAminIzao);
                            Console.WriteLine(marary[j].nom+ ",patient.effet = "+medoc.GetEffetForPArametre((marary[j].idparametre))+ "   "+(doseAminIzao));
                            if(patient.niveau[j]<=(medoc.GetEffetForPArametre((marary[j].idparametre))*doseAminIzao)){
                                
                                Console.WriteLine(j+ "    "+marary[j].nom+"   "+n+"    miala");
                                patient.niveau[j]=0; 
                                hafa.Add(j);
                            }
                            else
                            {
                                 patient.niveau[j]=n; 
                            }
                        }    
                    }
                    Console.WriteLine();
                    Console.WriteLine("medicament =  "+medoc.nom+"  dose : "+ doseAminIzao);
                    for(int t=0;t<patient.niveau.Count;t++){
                        Console.WriteLine();
                        Console.WriteLine("niveau du "+patient.parametres[t].nom+"  = "+patient.niveau[t]);
                    }
                    patient.ordonnances.Add(new Ordonnance(medoc,doseAminIzao));
                    for (int it = 0; it < patient.ordonnances.Count; it++)
                    {
                          Console.WriteLine(patient.ordonnances[it].medicament.nom+"   transcri"); 
                    }
                }
                return patient.ordonnances;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Maladie> GetHisMaladies(int idpatient){
            try{
                string request="select maladies.* from maladie_des_patients  m join maladies on m.idmaladie=maladies.idmaladie where m.idpatient="+idpatient;
                List<Maladie> l =Maladie.Get(request);
                return l;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Patient GetById(int idpatient){
            try{
                string r="select * from patients where idpatient="+idpatient;
                return Patient.Get(r)[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int InsererLesDonnees(string nom, double age, List<int> p, List<double> niveau)
        {
            try
            {
                int id = Patient.InsertPatient(nom, age);
                for (int i = 0; i < p.Count; i++)
                {
                    Patient.InsertUneDonnee(id, p[i], niveau[i]);
                }
                return id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); // Rethrow the exception for proper handling
            }
        }
        public static void InsertUneDonnee(int idpatient, int idpa, double niveau)
        {
            try
            {
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO donnees_des_patients (idpatient,idparametre,niveau) values (@id,@p,@n)";
                        DbParameter paramNom = cmd.CreateParameter();
                        paramNom.ParameterName = "@id";
                        paramNom.Value = idpatient;
                        cmd.Parameters.Add(paramNom);

                        DbParameter paramNom0 = cmd.CreateParameter();
                        paramNom0.ParameterName = "@p";
                        paramNom0.Value = idpa;
                        cmd.Parameters.Add(paramNom0);

                        DbParameter paramNom1 = cmd.CreateParameter();
                        paramNom1.ParameterName = "@n";
                        paramNom1.Value = niveau;
                        cmd.Parameters.Add(paramNom1);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); // Rethrow the exception for proper handling
            }
        }
        public static int InsertPatient(string nom, double age)
        {
            try
            {
                using (DbConnection? o = Connect.GetConnection())
                {
                    using (DbCommand cmd = o.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO patients (nom,age) VALUES (@Nom,@age) returning idpatient";
                        DbParameter paramNom = cmd.CreateParameter();
                        paramNom.ParameterName = "@Nom";
                        paramNom.Value = nom;
                        cmd.Parameters.Add(paramNom);

                        DbParameter paramNom0 = cmd.CreateParameter();
                        paramNom0.ParameterName = "@age";
                        paramNom0.Value = age;
                        cmd.Parameters.Add(paramNom0);

                        object result = cmd.ExecuteScalar();
                        int idp = (int)result;

                        return idp;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); // Rethrow the exception for proper handling
            }
        }
    }
}