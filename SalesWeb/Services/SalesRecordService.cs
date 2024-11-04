using Microsoft.EntityFrameworkCore;
using SalesWeb.Data;
using SalesWeb.Models;

namespace SalesWeb.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebContext _context;

        public SalesRecordService(SalesWebContext context)
        {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {

            IQueryable<SalesRecord> query = _context.SalesRecord
                .Include(sr => sr.Seller)
                .ThenInclude(s => s.Department);

   
            if (minDate.HasValue)
            {
                query = query.Where(sr => sr.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                query = query.Where(sr => sr.Date <= maxDate.Value);
            }

            var salesRecords = await query
                .OrderByDescending(sr => sr.Date)
                .ToListAsync();

            return salesRecords
                .GroupBy(sr => sr.Seller.Department)
                .ToList();
        }
    }
}



