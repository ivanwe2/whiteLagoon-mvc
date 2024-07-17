using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
        }

        public override void Update(Villa entity)
        {
            dbSet.Update(entity);
        }
    }
}
