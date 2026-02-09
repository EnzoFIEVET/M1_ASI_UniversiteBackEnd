using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(INoteRepository noteRepository)
{
 
    public async Task<Note> ExecuteAsync(Etudiant etudiant, Ue ue, float valnote)
    {
        var note = new Note{Etudiant = etudiant, Ue = ue, Valeur = valnote};
        return await ExecuteAsync(note);
    }
    
    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note createdNote = await noteRepository.CreateAsync(note);
        noteRepository.SaveChangesAsync().Wait();
        return createdNote;
    }
    
    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.Etudiant, nameof(note.Etudiant));
        ArgumentNullException.ThrowIfNull(note.Ue, nameof(note.Ue));

        // Règle 1 : La note doit être comprise entre 0 et 20
        if (note.Valeur < 0 || note.Valeur > 20)
        {
            throw new InvalidNoteValueException("La note doit être comprise entre 0 et 20.");
        }

        // Règle 2 : Un étudiant n'a qu'une note au maximum par UE
        var existingNote = await noteRepository.FindByConditionAsync(n => n.EtudiantId == note.EtudiantId && n.UeId == note.UeId);
        if (existingNote.Any())
        {
            throw new DuplicateIdEtudiantAndIdUeNoteException($"Une note pour l'UE {note.Ue.Intitule} a déjà été attribuée à l'étudiant {note.Etudiant.Nom} {note.Etudiant.Prenom}.");
        }

        // Règle 3 : Un étudiant ne peut avoir une note que dans une UE de son parcours
        if (note.Etudiant.ParcoursSuivi == null || note.Etudiant.ParcoursSuivi.UesEnseignees == null || !note.Etudiant.ParcoursSuivi.UesEnseignees.Any(ue => ue.Id == note.UeId))
        {
            throw new UeNotInParcoursException($"L'étudiant {note.Etudiant.Nom} {note.Etudiant.Prenom} n'est pas inscrit à l'UE {note.Ue.Intitule} dans son parcours.");
        }
    }
}
