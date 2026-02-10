using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;

namespace UniversiteRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtudiantController(IRepositoryFactory repositoryFactory) : ControllerBase
    {
        // GET: api/<EtudiantController> READ ALL
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EtudiantController>/5 READ
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // Crée un nouvel étudiant sans parcours
        // POST api/<EtudiantApi>
        [HttpPost]
        public async Task<Etudiant> PostAsync([FromBody] Etudiant etudiant)
        {
            CreateEtudiantUseCase uc=new CreateEtudiantUseCase(repositoryFactory.EtudiantRepository());
            return await uc.ExecuteAsync(etudiant);
        }

        // PUT api/<EtudiantController>/5 UPDATE
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EtudiantController>/5 DELETE
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
