using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAspNetEmptyApp.Controllers
{    
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private ILogger<TodoController> _logger;
        
        public TodoController(ILogger<TodoController> logger){
            _logger = logger;            
        }   
             
        static readonly List<TodoItem> _items = new List<TodoItem>()
        {
            new TodoItem { Id = 1, Title = "First Item" }
        };

        // GET: api/values
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            _logger.LogInformation("\"List TodoItem\" requested at {requestTime}", DateTime.Now);
            return _items;
        }

        // GET api/values/5
        [HttpGet("{id:int}", Name = "GetByIdRoute")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("\"Get TodoItem.ID={id}\" requested at {requestTime}", id, DateTime.Now);
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
            }
            else
            {
                item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
                _items.Add(item);

                string url = Url.RouteUrl("GetByIdRoute", new { id = item.Id },
                    Request.Scheme, Request.Host.ToUriComponent());

                HttpContext.Response.StatusCode = 201;
                HttpContext.Response.Headers["Location"] = url;
            }
        }

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            _items.Remove(item);
            return new HttpStatusCodeResult(204); // 201 No Content
        }
    }
}
