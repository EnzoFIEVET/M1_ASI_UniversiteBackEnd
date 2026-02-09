using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeDansNoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.UeDansNote;

public class AddUeDansNoteUseCase(IRepositoryFactory repositoryFactory)
{
    // Rajout d'un ue dans une note
      public async Task<Note> ExecuteAsync(Note note, Ue ue)
      {
          ArgumentNullException.ThrowIfNull(note);
          ArgumentNullException.ThrowIfNull(ue);
          return await ExecuteAsync(note.Id, ue.Id); 
      }  
      public async Task<Note> ExecuteAsync(long idNote, long idUe)
      {
          await CheckBusinessRules(idNote, idUe); 
          return await repositoryFactory.NoteRepository().AddUeAsync(idNote, idUe);
      }

    private async Task CheckBusinessRules(long idNote, long idUe)
    {
        // Vérification des paramètres
        ArgumentNullException.ThrowIfNull(idNote);
        ArgumentNullException.ThrowIfNull(idUe);
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idNote);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idUe);
        
        // Vérifions tout d'abord que nous sommes bien connectés aux datasources
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.NoteRepository());
        
        // On recherche l'ue
        List<Ue> ue = await repositoryFactory.UeRepository().FindByConditionAsync(u=>u.Id.Equals(idUe));;
        if (ue is { Count: 0 }) throw new UeNotFoundException(idUe.ToString());
        
        // On recherche la note
        List<Note> note = await repositoryFactory.NoteRepository().FindByConditionAsync(n=>n.Id.Equals(idNote));;
        if (note is { Count: 0 }) throw new NoteNotFoundException(idNote.ToString());
        
        // On vérifie que l'ue n'est pas déjà dans la note
        List<Ue> inscrit = await repositoryFactory.UeRepository()
            .FindByConditionAsync(u => !u.Id.Equals(idUe) || !u.Notes.Contains(note.First()));
        if (inscrit is { Count: > 0 }) throw new DuplicateAssignationException(idUe+" est déjà assigné dans la note: "+idNote);     
    }
}