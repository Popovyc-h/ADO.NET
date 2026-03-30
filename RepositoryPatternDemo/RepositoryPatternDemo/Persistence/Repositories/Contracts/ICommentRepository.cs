using RepositoryPatternDemo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternDemo.Persistence.Repositories.Contracts;

internal interface ICommentRepository: IRepository<Comment>
{
}
