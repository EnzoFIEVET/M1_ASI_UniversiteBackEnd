namespace UniversiteDomain.Entities
{
    public class Note
    {
        public long Id { get; set; }
        public float Valeur { get; set; }
        
        public Etudiant Etudiant { get; set; } = null!;
        public Ue Ue { get; set; } = null!;
        
        public long EtudiantId { get; set; }
        public long UeId { get; set; }
        
        
        public override string ToString()
        {
            return $"Note associée à l'étudiant : Etudiant={Etudiant.NumEtud}, Ue={Ue.NumeroUe}, Valeur={Valeur}";
        }
    }
}