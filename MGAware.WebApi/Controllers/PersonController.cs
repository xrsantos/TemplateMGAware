using System.Linq;
using System.Data;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MGAware.Security.JWT;
using MGAware.Database.DTO;
using MGAware.Database;
using MGAware.Database.Context;
using MGAware.Database.DAL;
using MGAware.Database.Repository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MGAware.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PersonController : ODataController
{
    private UnitOfWork<MGADBContext> _unitOfWork;
    private PersonRepository _repositoryPerson;
    private ContactRepository _contactRepository;
    private ILogger<LoginController> _logger;

    public PersonController(
            [FromServices] MGADBContext mGADBContext,
            [FromServices] ILogger<LoginController> logger 
    )
    {
        _unitOfWork = new UnitOfWork<MGADBContext>(mGADBContext);
        _repositoryPerson = new PersonRepository(_unitOfWork);
        _contactRepository = new ContactRepository(_unitOfWork);
        _logger = logger;
    } 

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy)]
    public ActionResult<Person> Get()
    {
        return Ok(_repositoryPerson.GetAllAndContracts());
    }


    [AllowAnonymous]
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]

    public IActionResult GetAll(ODataQueryOptions<Person> options)
    {
        
        

        var objs = _repositoryPerson.GetAll().AsQueryable();
        
        var total = 0;
        if (options.Count != null && options.Count.Value)
            total = objs.Count();

        return Ok
                    (new 
                        { 
                            total = total, 
                            value = options.ApplyTo(objs)
                        }
                    );
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [EnableQuery]
    public ActionResult<Person> Get(
        [FromRoute] int id       
    )
    {
        try
        {
            var item = _repositoryPerson.GetById(id);
            if(item != null)
                return item;
            
            return NotFound();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"");
            return BadRequest();
        }
    }


    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    
    public ActionResult<Person> Post(
        [FromBody] Person person
    )
    {
        try
        {
            _repositoryPerson.Insert(person);

            if (person.Contacts != null)
            {
                foreach (var item in person.Contacts)
                {
                    item.Person = person;
                    _contactRepository.Insert(item);
                }
            }


            _unitOfWork.Save();
            return person;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"");
            return BadRequest();
        }
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public ActionResult<Person> Put(
        int id,
        [FromBody] Person person
    )
    {
        try
        {
            person.Id = id;
            _repositoryPerson.Update(person);
            if (person.Contacts != null)
            {
                foreach (var item in person.Contacts)
                {
                    item.Person = person;
                    if (item.Id == 0)
                        _contactRepository.Insert(item);
                    else
                        _contactRepository.Update(item);
                }
            }

            _unitOfWork.Save();
            return person;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"");
            return BadRequest();
        }
 
    }

    [AllowAnonymous]
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public ActionResult Delete(
        [FromRoute] int id
    )
    {
        try
        {
            var item = _repositoryPerson.GetById(id);
            if(item == null)
                return NotFound();
            _repositoryPerson.Delete(item);
            _unitOfWork.Save();
            return Ok(new { Description = "Success" });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"");
            return BadRequest();
        }

    }
}