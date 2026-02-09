namespace UniversiteDomain.Exceptions.EtudiantDansNoteExceptions;

[Serializable]
public class DuplicateAssignationException : Exception
{
    public DuplicateAssignationException() : base() { }
    public DuplicateAssignationException(string message) : base(message) { }
    public DuplicateAssignationException(string message, Exception inner) : base(message, inner) { }
}