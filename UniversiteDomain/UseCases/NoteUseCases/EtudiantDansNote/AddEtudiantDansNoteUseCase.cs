using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantDansNoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.EtudiantDansNote;

public class AddEtudiantDansNoteUseCase(IRepositoryFactory repositoryFactory)
{
    // Rajout d'un étudiant dans une note
      public async Task<Note> ExecuteAsync(Note note, Etudiant etudiant)
      {
          ArgumentNullException.ThrowIfNull(note);
          ArgumentNullException.ThrowIfNull(etudiant);
          return await ExecuteAsync(note.Id, etudiant.Id); 
      }  
      public async Task<Note> ExecuteAsync(long idNote, long idEtudiant)
      {
          await CheckBusinessRules(idNote, idEtudiant); 
          return await repositoryFactory.NoteRepository().AddEtudiantAsync(idNote, idEtudiant);
      }

    private async Task CheckBusinessRules(long idNote, long idEtudiant)
    {
        // Vérification des paramètres
        ArgumentNullException.ThrowIfNull(idNote);
        ArgumentNullException.ThrowIfNull(idEtudiant);
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idNote);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idEtudiant);
        
        // Vérifions tout d'abord que nous sommes bien connectés aux datasources
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.EtudiantRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.NoteRepository());
        
        // On recherche l'étudiant
        List<Etudiant> etudiant = await repositoryFactory.EtudiantRepository().FindByConditionAsync(e=>e.Id.Equals(idEtudiant));;
        if (etudiant is { Count: 0 }) throw new EtudiantNotFoundException(idEtudiant.ToString());
        
        // On recherche la note
        List<Note> note = await repositoryFactory.NoteRepository().FindByConditionAsync(n=>n.Id.Equals(idNote));;
        if (note is { Count: 0 }) throw new NoteNotFoundException(idNote.ToString());
        
        // On vérifie que l'étudiant n'est pas déjà dans la note
        List<Etudiant> inscrit = await repositoryFactory.EtudiantRepository()
            .FindByConditionAsync(e => !e.Id.Equals(idEtudiant) || !e.NotesObtenues.Contains(note.First()));
        if (inscrit is { Count: > 0 }) throw new DuplicateAssignationException(idEtudiant+" est déjà assigné dans la note: "+idNote);     
    }
}