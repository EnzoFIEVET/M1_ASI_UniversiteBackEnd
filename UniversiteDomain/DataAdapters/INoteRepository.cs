using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
    Task<Note> AddEtudiantAsync(Note note, Etudiant etudiant);
    Task<Note> AddEtudiantAsync(long idNote, long idEtudiant);
    Task<Note> AddUeAsync(Parcours note, Ue ue);
    Task<Note> AddUeAsync(long idNote, long idUe);
}