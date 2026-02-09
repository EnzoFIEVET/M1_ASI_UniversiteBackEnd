using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Create;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;

namespace UniversiteDomainUnitTest;

public class NoteUnitTests
{
    private Mock<INoteRepository> _mockNoteRepo;
    private Mock<IRepositoryFactory> _mockFactory;
    private CreateNoteUseCase _useCase;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task CreateNoteUseCase()
    {
        Ue ue = new Ue { Id = 1, Intitule = "ASI"};
        Parcours parcours = new Parcours { Id = 1, NomParcours = "MIAGE", UesEnseignees = new List<Ue> { ue } };
        
        long id = 1;
        String numEtud = "et1";
        string nom = "Durant";
        string prenom = "Jean";
        string email = "jean.durant@etud.u-picardie.fr";

        Etudiant etudiant = new Etudiant{Id = id,NumEtud = numEtud, Nom = nom, Prenom = prenom, Email = email, ParcoursSuivi = parcours};
        
        Note noteSansId = new Note { Valeur = 15, Etudiant = etudiant, Ue = ue, EtudiantId = etudiant.Id, UeId = ue.Id };
        
        var mock = new Mock<INoteRepository>();
        
        var reponseFindByCondition = new List<Note>();
        
        mock.Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Note, bool>>>())).ReturnsAsync(reponseFindByCondition);
        mock.Setup(repo => repo.CreateAsync(It.IsAny<Note>())).ReturnsAsync((Note note) => { note.Id = 1; return note; });
        
        var fauxNoteRepository = mock.Object;
        
        CreateNoteUseCase useCase = new CreateNoteUseCase(fauxNoteRepository);
        
        Note noteTeste = await useCase.ExecuteAsync(noteSansId);
        
        Note noteCree = new Note { Id = 1, Valeur = 15, Etudiant = etudiant, Ue = ue, EtudiantId = etudiant.Id, UeId = ue.Id };
        
        Assert.That(noteTeste.Id, Is.EqualTo(noteCree.Id));
        Assert.That(noteTeste.Etudiant, Is.EqualTo(noteCree.Etudiant));
        Assert.That(noteTeste.Ue, Is.EqualTo(noteCree.Ue));
        Assert.That(noteTeste.EtudiantId, Is.EqualTo(noteCree.EtudiantId));
        Assert.That(noteTeste.UeId, Is.EqualTo(noteCree.UeId));
    }
    
    [Test]
    public void AddEtudiantDansNoteUseCase()
    {
        Ue ue = new Ue { Id = 1, Intitule = "ASI" };
        Parcours parcours = new Parcours { Id = 1, NomParcours = "MIAGE", UesEnseignees = new List<Ue> { ue } };
        Etudiant etudiant = new Etudiant { Id = 1, NumEtud = "et1", Nom = "Durant", Prenom = "Jean", Email = "jean.durant@etud.u-picardie.fr", ParcoursSuivi = parcours };
        Note note = new Note { Id = 1, Valeur = 15, Ue = ue, UeId = ue.Id };
        
        note.Etudiant = etudiant;
        note.EtudiantId = etudiant.Id;
        
        Assert.That(note.Etudiant, Is.Not.Null);
        Assert.That(note.Etudiant, Is.EqualTo(etudiant));
        Assert.That(note.EtudiantId, Is.EqualTo(etudiant.Id));
    }

    [Test]
    public void AddUeDansNote()
    {
        Ue ue = new Ue { Id = 1, Intitule = "ASI" };
        Note note = new Note { Id = 1, Valeur = 15 };
        
        note.Ue = ue;
        note.UeId = ue.Id;
        
        Assert.That(note.Ue, Is.Not.Null);
        Assert.That(note.Ue, Is.EqualTo(ue));
        Assert.That(note.UeId, Is.EqualTo(ue.Id));
    }
}