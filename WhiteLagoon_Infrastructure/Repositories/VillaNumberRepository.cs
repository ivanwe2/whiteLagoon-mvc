using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    internal class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
        }

        public override void Update(VillaNumber entity)
        {
            dbSet.Update(entity);
        }
    }
}
