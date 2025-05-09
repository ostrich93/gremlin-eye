using gremlin_eye.Server.Repositories;

namespace gremlin_eye.Server.Data
{
    public class UnitOfWork
    {
        private readonly DataContext _context;

        private IUserRepository? _userRepository;
        private IGameLogRepository? _logRepository;
        private IGameRepository? _gameRepository;
        private IPlaythroughRepository? _playthroughRepository;
        private IReviewRepository? _reviewRepository;
        private IListingRepository? _listingRepository;

        public DataContext Context { get { return _context; } }

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get 
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public IGameLogRepository GameLogs
        {
            get
            {
                if (_logRepository == null)
                {
                    _logRepository = new GameLogRepository(_context);
                }
                return _logRepository;
            }
        }

        public IGameRepository Games
        {
            get
            {
                if (_gameRepository == null)
                {
                    _gameRepository = new GameRepository(_context);
                }
                return _gameRepository;
            }
        }

        public IPlaythroughRepository Playthroughs
        {
            get
            {
                if (_playthroughRepository == null)
                {
                    _playthroughRepository = new PlaythroughRepository(_context);
                }
                return _playthroughRepository;
            }
        }

        public IListingRepository Lists
        {
            get
            {
                if (_listingRepository == null)
                {
                    _listingRepository = new ListingRepository(_context);
                }
                return _listingRepository;
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_context);
                }
                return _reviewRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    }
}
