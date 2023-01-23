// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;
using QuickApp.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using QuickApp.Helpers;
using DAL.Repositories;
using DAL.Models;

namespace QuickApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Repository<Product> _repo;


        public ProductController(
            ManagementDbContext context,
            IMapper mapper, 
            ILogger<ProductController> logger
            )
        {
            _mapper = mapper;
            _logger = logger;
            _repo = new Repository<Product>(context);
        }



        // GET: api/values
        [HttpGet]
        public IActionResult GetAll()
        {
            var all = _repo.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(all));
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]long id)
        {
            var value = _repo.Get(id);
            return Ok(_mapper.Map<ProductViewModel>(value));
        }



        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ProductViewModel model)
        {
            var entity =  _mapper.Map<Product>(model);
            _repo.Add(entity);
            return Ok();
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]long id, [FromBody]ProductViewModel model)
        {
            var entity = _repo.Get(id);
            if(entity==null){
                return NotFound();
            }
            _repo.ClearTrack();
            entity = _mapper.Map<Product>(model);
            _repo.Update(entity);
            return Ok();
        }



        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]long id)
        {
            var entity = _repo.Get(id);
            if(entity==null){
                return NotFound();
            }
            _repo.Remove(entity);
            return Ok();
        }
    }
}
