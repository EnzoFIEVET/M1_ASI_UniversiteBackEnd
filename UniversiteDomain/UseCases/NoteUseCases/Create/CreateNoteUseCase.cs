using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase(INoteRepository noteRepository)
{
    public async Task<Note> ExecuteAsync(Etudiant etudiant, Ue ue, long idEtudiant, long idUe, float valeur)
    {
        var note = new Note{Etudiant = etudiant, Ue = ue, IdEtudiant = idEtudiant, IdUe = idUe};
        return await ExecuteAsync(note);
    }
    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note et = await noteRepository.CreateAsync(note);
        noteRepository.SaveChangesAsync().Wait();
        return et;
    }
    
    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.IdEtudiant);
        ArgumentNullException.ThrowIfNull(note.IdUe);
        ArgumentNullException.ThrowIfNull(noteRepository);
        
        // On recherche une note avec le même étudiant et la même Ue 
        List<Note> existe = await noteRepository.FindByConditionAsync(e => e.IdEtudiant.Equals(note.IdEtudiant) && e.IdUe.Equals(note.IdUe));

        // Si une note avec le même étudiant et la même Ue existe déjà, on lève une exception personnalisée
        if (existe is {Count:>0}) throw new DuplicateIdEtudiantAndIdUeNoteException("Une note est déjà affectée à l'étudiant : " + note.Etudiant.Prenom + " " + note.Etudiant.Nom + " pour l'Ue : " + note.Ue.Intitule);
        
        // Vérification que l'étudiant est bien inscrit dans l'Ue
        if ()
        
        // On vérifie si l'email est déjà utilisé
        existe = await noteRepository.FindByConditionAsync(e=>e.Email.Equals(note.Email));
        // Une autre façon de tester la vacuité de la liste
        if (existe is {Count:>0}) throw new DuplicateEmailException(note.Email +" est déjà affecté à un étudiant");
        // Le métier définit que les nom doite contenir plus de 3 lettres
        if (note.Nom.Length < 3) throw new InvalidNomNoteException(note.Nom +" incorrect - Le nom d'un étudiant doit contenir plus de 3 caractères");
    }
}