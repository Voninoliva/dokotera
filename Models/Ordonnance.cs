namespace dokotera.Models
{
    public class Ordonnance
    {
       public Medicament medicament{get;set;}
       public double dose{get;set;}
       public double prix{get;set;}

        public Ordonnance(Medicament medicament, double dose)
        {
            this.medicament = medicament;
            this.dose = dose;
            this.prix = this.medicament.prix * this.dose;
        }
    }
}
