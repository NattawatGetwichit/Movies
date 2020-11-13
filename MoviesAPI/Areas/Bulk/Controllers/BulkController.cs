using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace MoviesAPI
{
    [ApiController]
    [Route("bulk")]
    public class BulkController : ControllerBase
    {
        private readonly BulkDBContext _context;

        public BulkController(BulkDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Insert()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForInsert();
            _context.BulkInsert(bulk);
            return Ok(bulk);
        }

        [HttpPut]
        public IActionResult Update()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForUpdate();
            _context.BulkUpdate(bulk);
            return Ok(bulk);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForDelete();
            _context.BulkDelete(bulk);
            return Ok(bulk);
        }

        public static List<Bulk> GetDataForInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkName = "BulkName_" + i, BulkCode = "BulkCode_" + i });
            }
            return bulk;
        }

        private List<Bulk> GetDataForUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Update BulkName_" + i, BulkCode = "Update BulkCode_" + i });
            }
            return bulk;
        }

        private List<Bulk> GetDataForDelete()
        {
            return _context.Bulk.ToList();
        }
    }
}