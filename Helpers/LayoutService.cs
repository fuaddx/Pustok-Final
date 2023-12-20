using Pustok2.Models;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;

namespace Pustok2.Helpers
{
    public class LayoutService
    {
        PustokDbContext _db { get; }
        public LayoutService(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<Setting> GetSettingAsync()
        => await _db.Settings.FindAsync(1);
            
    }
}
