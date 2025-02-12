using Application.Common.Repositories;
using Domain.Common;

namespace Application.Common.CQS.Queries;

public interface IQueryContext : IEntityDbSet
{
    IQueryable<T> Set<T>() where T : class;
    IQueryable<T> SetWithTenantFilter<T>() where T : class, IHasTenantId;
}

