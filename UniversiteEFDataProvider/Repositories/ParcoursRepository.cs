using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        p.Inscrits.Add(e);
        await Context.SaveChangesAsync();
        return p;
    }
    
    public Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        return AddEtudiantAsync(parcours.Id, etudiant.Id);
    }
    
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        foreach (long idEtudiant in idEtudiants)
        {
            Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
            p.Inscrits.Add(e);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    
    public Task<Parcours> AddEtudiantAsync(Parcours parcours, List<Etudiant> etudiants)
    {
        long[] IdEtudiants = new long[etudiants.Count()];
        for (int i = 0; i < etudiants.Count(); i++)
        {
                IdEtudiants[i] = etudiants.ElementAt(i).Id;
        }
        return AddEtudiantAsync(parcours.Id, IdEtudiants); 
    }
    
    public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Ue u = (await Context.Ues.FindAsync(idUe))!;
        p.UesEnseignees.Add(u);
        await Context.SaveChangesAsync();
        return p;
    }
    
    public Task<Parcours> AddUeAsync(Parcours parcours, Ue ue)
    {
        return AddUeAsync(parcours.Id, ue.Id); 
    }
    
    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        foreach (long idUe in idUes)
        {
            Ue u = (await Context.Ues.FindAsync(idUe))!;
            p.UesEnseignees.Add(u);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    
    public Task<Parcours> AddUeAsync(Parcours parcours, List<Ue> ues)
    {
        long[] IdUes = new long[ues.Count()];
        for (int i = 0; i < ues.Count(); i++)
        {
            IdUes[i] = ues.ElementAt(i).Id;
        }
        return AddEtudiantAsync(parcours.Id, IdUes); 
    }
}