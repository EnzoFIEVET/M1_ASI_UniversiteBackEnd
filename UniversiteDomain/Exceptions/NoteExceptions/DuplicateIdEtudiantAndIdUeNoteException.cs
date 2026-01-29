namespace UniversiteDomain.Exceptions.NoteExceptions;

public class DuplicateIdEtudiantAndIdUeNoteException : Exception
{
    public DuplicateIdEtudiantAndIdUeNoteException() : base() { }
    public DuplicateIdEtudiantAndIdUeNoteException(string message) : base(message) { }
    public DuplicateIdEtudiantAndIdUeNoteException(string message, Exception inner) : base(message, inner) { }
}