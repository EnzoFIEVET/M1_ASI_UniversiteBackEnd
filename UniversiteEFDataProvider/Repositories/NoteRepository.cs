using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    public async Task<Note> AddEtudiantAsync(long idNote, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        Note n = (await Context.Notes.FindAsync(idNote))!;
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        n.Etudiant = e;
        n.EtudiantId = idEtudiant;
        await Context.SaveChangesAsync();
        return n;
    }
    
    public Task<Note> AddEtudiantAsync(Note note, Etudiant etudiant)
    {
        return AddEtudiantAsync(note.Id, etudiant.Id);
    }

    public async Task<Note> AddUeAsync(long idNote, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Note n = (await Context.Notes.FindAsync(idNote))!;
        Ue u = (await Context.Ues.FindAsync(idUe))!;
        n.Ue = u;
        n.UeId = idUe;
        await Context.SaveChangesAsync();
        return n;
    }

    public Task<Note> AddUeAsync(Parcours note, Ue ue)
    {
        return AddEtudiantAsync(note.Id, ue.Id);
    }
}