using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Common
{
    public interface IBaseRepository<TEntity>
    {
        TEntity Add(TEntity newEntity);

        TEntity Update(TEntity toUpdate);

        TEntity GetById(Guid id);

        bool Delete(TEntity toDelete);
    }
}
