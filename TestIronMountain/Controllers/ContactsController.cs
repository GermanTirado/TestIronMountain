using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestIronMountain.Models;
using TestIronMountain.Models.Response;

namespace TestIronMountain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly NewDbContext _context;

        public ContactsController(NewDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts GetAll Contacts
        [HttpGet]
        public async Task<MyResponse> GetContact()
        {
            MyResponse res = new MyResponse();
            List<Contact> lst = await (from d in _context.Contacts
                                       where d.active == 1
                                       select new Contact
                                       {
                                           id = d.id,
                                           regDate = d.regDate,
                                           name = d.name,
                                           address = d.address,
                                           phone = d.phone,
                                           curp = d.curp,
                                           active = d.active
                                       }).ToListAsync();
            res.Message = "Data Founded";
            res.Success = 200;
            res.Data = lst;
            return res;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5 Edit CONTACT
        [HttpPut("{id}")]
        public async Task<MyResponse> PutContact(int id, Contact contact)
        {
            MyResponse res = new MyResponse();
            if (id != contact.id)
            {
                res.Message = "Id does not match";
                res.Success = 500;
                return res;
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                res.Message = "Contact updated successfully";
                res.Success = 200;
                res.Data = contact;
                return res;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    res.Message = "Contact not found";
                    res.Success = 404;
                    return res;
                }
                else
                {
                    throw;
                }
            }

            res.Message = "Not contact";
            res.Success = 500;
            return res;
        }

        // POST: api/Contacts CREATE CONTACT
        [HttpPost]
        public async Task<MyResponse> PostContact([FromBody]Contact contact)
        {
            MyResponse res = new MyResponse();
            try
            {
                contact.regDate = DateTime.Now.Date.ToString();
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                res.Message = "Contact created successfully";
                res.Success = 200;
                res.Data = contact;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = 500;
            }

            return res;
        }

        // Logic DELETE: api/Contacts/5
        [HttpPut("[action]")]
        public async Task<MyResponse> DeleteContact(Contact contact)
        {
            MyResponse res = new MyResponse();
            contact.active = 0;
            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                res.Message = "Contact successfully deleted";
                res.Success = 200;
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Success = 500;
            }
            return res;
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.id == id);
        }
    }
}
