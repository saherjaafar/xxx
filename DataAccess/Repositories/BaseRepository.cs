using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Repositories;

namespace DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;

        public BaseRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest)
        {
            _context = context;
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public T GetById(long id)
        {
            return _context.Set<T>().Find(id); 
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }
    }
}
